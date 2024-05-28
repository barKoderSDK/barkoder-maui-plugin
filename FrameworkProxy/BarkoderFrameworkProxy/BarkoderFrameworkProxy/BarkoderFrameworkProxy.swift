//
//  BarkoderFrameworkProxy.swift
//  BarkoderFrameworkProxy
//
//  Created by Slobodan Marinkovik on 26.9.23.
//

import Foundation
import UIKit
import BarkoderSDK
import Barkoder

@objc(BarkoderView)
public class BarkoderProxy: NSObject {
    
    private var barkoderView: BarkoderView!
    
    @objc
    private var resultCallbackDecoderResult: ((_ data: DecoderPayload) -> Void)?
    
    @objc
    public var view: UIView {
        barkoderView = BarkoderView(frame: CGRect(
            x: 0,
            y: 0,
            width: UIScreen.main.bounds.width,
            height: 400
        ))
        return barkoderView
    }
    
    @objc
    public var config: BKDConfig? {
        didSet {
            if let config {
                self.barkoderView.config = BarkoderConfig(licenseKey: config.licenseKey, licenseCheckHandler: { _ in
                    print("License added")
                })
                
                config.setEnabledDecodersCallback { symbologies in
                    // Timer after X second because of conflicting with directly changing enabled decoders
                    Timer.scheduledTimer(withTimeInterval: 0.3, repeats: false) { _ in
                        self.barkoderView.config?.decoderConfig?.setEnabledDecoders(symbologies.compactMap { Int32($0) })
                    }
                }
                
                barkoderView.config?.pinchToZoomEnabled = config.pinchToZoomEnabled
                barkoderView.config?.closeSessionOnResultEnabled = config.closeSessionOnResultEnabled
                barkoderView.config?.beepOnSuccessEnabled = config.beepOnSuccessEnabled
                barkoderView.config?.vibrateOnSuccessEnabled = config.vibrateOnSuccessEnabled
                barkoderView.config?.locationInPreviewEnabled = config.locationInPreviewEnabled
                barkoderView.config?.roiLineColor = config.roiLineColor
                barkoderView.config?.roiLineWidth = config.roiLineWidth
                barkoderView.config?.roiOverlayBackgroundColor = config.roiOverlayBackgroundColor
                barkoderView.config?.regionOfInterestVisible = config.regionOfInterestVisible
                barkoderView.config?.locationLineColor = config.locationLineColor
                barkoderView.config?.locationLineWidth = config.locationLineWidth
                barkoderView.config?.imageResultEnabled = config.imageResultEnabled
                barkoderView.config?.locationInImageResultEnabled = config.locationInImageResultEnabled
                barkoderView.config?.thresholdBetweenDuplicatesScans = config.thresholdBetweenDuplicatesScans
                barkoderView.config?.decoderConfig?.enableMisshaped1D = config.decoderConfig?.enableMisshaped1D ?? false
                barkoderView.config?.decoderConfig?.upcEanDeblur = config.decoderConfig?.upcEanDeblur ?? false
                barkoderView.config?.decoderConfig?.enableVINRestrictions = config.decoderConfig?.enableVINRestrictions ?? false
                barkoderView.config?.decoderConfig?.aztec.enabled = config.decoderConfig?.aztec.enabled ?? false
                barkoderView.config?.decoderConfig?.aztecCompact.enabled = config.decoderConfig?.aztecCompact.enabled ?? false
                barkoderView.config?.decoderConfig?.qr.enabled = config.decoderConfig?.qr.enabled ?? false
                barkoderView.config?.decoderConfig?.qrMicro.enabled = config.decoderConfig?.qrMicro.enabled ?? false
                barkoderView.config?.decoderConfig?.code128.enabled = config.decoderConfig?.code128.enabled ?? false
                barkoderView.config?.decoderConfig?.code93.enabled = config.decoderConfig?.code93.enabled ?? false
                barkoderView.config?.decoderConfig?.code39.enabled = config.decoderConfig?.code39.enabled ?? false
                barkoderView.config?.decoderConfig?.codabar.enabled = config.decoderConfig?.codabar.enabled ?? false
                barkoderView.config?.decoderConfig?.code11.enabled = config.decoderConfig?.code11.enabled ?? false
                barkoderView.config?.decoderConfig?.msi.enabled = config.decoderConfig?.msi.enabled ?? false
                barkoderView.config?.decoderConfig?.upcA.enabled = config.decoderConfig?.upcA.enabled ?? false
                barkoderView.config?.decoderConfig?.upcE.enabled = config.decoderConfig?.upcE.enabled ?? false
                barkoderView.config?.decoderConfig?.upcE1.enabled = config.decoderConfig?.upcE1.enabled ?? false
                barkoderView.config?.decoderConfig?.ean13.enabled = config.decoderConfig?.ean13.enabled ?? false
                barkoderView.config?.decoderConfig?.ean8.enabled = config.decoderConfig?.ean8.enabled ?? false
                barkoderView.config?.decoderConfig?.pdf417.enabled = config.decoderConfig?.pdf417.enabled ?? false
                barkoderView.config?.decoderConfig?.pdf417Micro.enabled = config.decoderConfig?.pdf417Micro.enabled ?? false
                barkoderView.config?.decoderConfig?.datamatrix.enabled = config.decoderConfig?.datamatrix.enabled ?? false
                barkoderView.config?.decoderConfig?.datamatrix.dpmMode = Int32(config.decoderConfig?.datamatrix.dpmMode ?? 0)
                barkoderView.config?.decoderConfig?.code25.enabled = config.decoderConfig?.code25.enabled ?? false
                barkoderView.config?.decoderConfig?.interleaved25.enabled = config.decoderConfig?.interleaved25.enabled ?? false
                barkoderView.config?.decoderConfig?.itf14.enabled = config.decoderConfig?.itf14.enabled ?? false
                barkoderView.config?.decoderConfig?.iata25.enabled = config.decoderConfig?.iata25.enabled ?? false
                barkoderView.config?.decoderConfig?.matrix25.enabled = config.decoderConfig?.matrix25.enabled ?? false
                barkoderView.config?.decoderConfig?.datalogic25.enabled = config.decoderConfig?.datalogic25.enabled ?? false
                barkoderView.config?.decoderConfig?.coop25.enabled = config.decoderConfig?.coop25.enabled ?? false
                barkoderView.config?.decoderConfig?.code32.enabled = config.decoderConfig?.code32.enabled ?? false
                barkoderView.config?.decoderConfig?.telepen.enabled = config.decoderConfig?.telepen.enabled ?? false
				barkoderView.config?.decoderConfig?.dotcode.enabled = config.decoderConfig?.dotcode.enabled ?? false

                config.setModelDidChangeCallback {
                    // Forcing to enter 'didSet' again
                    self.config = config
                }
            }
        }
    }
    
    @objc
    public var decoderConfig: Config? {
        didSet {
            barkoderView.config?.decoderConfig = decoderConfig
        }
    }
        
    @objc
    public func startScanning(completion: @escaping (DecoderPayload) -> Void) {
        do {
            try barkoderView.startScanning(self)
            self.resultCallbackDecoderResult = completion
        } catch {
            // TODO: - Handle error
        }
    }
    
    @objc
    public func startCamera() {
        barkoderView.startCamera()
    }
    
    @objc
    public func stopScanning() {
        barkoderView.stopScanning()
    }
         
    @objc
    public func pauseScanning() {
        barkoderView.pauseScanning()
    }
    
    @objc
    public func isFlashAvailable(completion: @escaping (Bool) -> Void) {
        barkoderView.isFlashAvailable({ flashAvailable in
            completion(flashAvailable)
        })
    }
    
    @objc
    public func setFlashEnabled(arg: Bool) {
        barkoderView.setFlash(arg)
    }
    
    @objc
    public func getMaxZoomFactor(completion: @escaping (Float) -> Void) {
        barkoderView.getMaxZoomFactor { maxZoomFactor in
            completion(maxZoomFactor)
        }
    }
    
    @objc
    public func setZoomFactor(arg: Float) {
        barkoderView.setZoomFactor(arg)
    }
    
    @objc
    public func setLocationLineWidth(arg: Float) {
        barkoderView.config?.locationLineWidth = arg
    }
    
    @objc
    public func getLocationLineColorHex() -> String {
        barkoderView.config?.locationLineColor.toHex() ?? ""
    }
    
    @objc
    public func setLocationLineColor(arg: String) {
        barkoderView.config?.locationLineColor = .red
    }
    
    @objc
    public func setRoiLineColor(arg: String) {
        barkoderView.config?.roiLineColor = Util.initColorWith(hexString: arg)
    }
    
    @objc
    public func setRoiLineWidth(arg: Float) {
        barkoderView.config?.roiLineWidth = arg
    }
    
    @objc
    public func setRoiOverlayBackgroundColor(arg: String) {
        barkoderView.config?.roiOverlayBackgroundColor = Util.initColorWith(hexString: arg)
    }
    
    @objc
    public func setCloseSessionOnResultEnabled(arg: Bool) {
        barkoderView.config?.closeSessionOnResultEnabled = arg
    }
    
    @objc
    public func setImageResultEnabled(arg: Bool) {
        barkoderView.config?.imageResultEnabled = arg
    }
    
    @objc
    public func setLocationInImageResultEnabled(arg: Bool) {
        barkoderView.config?.locationInImageResultEnabled = arg
    }
    
    @objc
    public func setThresholdBetweenDuplicatesScans(arg: Int) {
        barkoderView.config?.thresholdBetweenDuplicatesScans = arg
    }
    
    @objc
    public func setLocationInPreviewEnabled(arg: Bool) {
        barkoderView.config?.locationInPreviewEnabled = arg
    }
    
    @objc
    public func setPinchToZoomEnabled(arg: Bool) {
        barkoderView.config?.pinchToZoomEnabled = arg
    }
    
    @objc
    public func setRegionOfInterestVisible(arg: Bool) {
        barkoderView.config?.regionOfInterestVisible = arg
    }
    
    @objc
    public func setRegionOfInterest(
        left: Int,
        top: Int,
        width: Int,
        height: Int
    ) {
        let roi = CGRect(x: left, y: top, width: width, height: height)

        do {
            try barkoderView.config?.setRegionOfInterest(roi)
        } catch {
            // TODO: -
        }
    }
    
    @objc
    public func setBeepOnSuccessEnabled(arg: Bool) {
        barkoderView.config?.beepOnSuccessEnabled = arg
    }
    
    @objc
    public func setVibrateOnSuccessEnabled(arg: Bool) {
        barkoderView.config?.vibrateOnSuccessEnabled = arg
    }
    
    @objc
    public var encodingCharacterSet: String {
        return barkoderView.config?.decoderConfig?.encodingCharacterSet ?? ""
    }
    
    @objc
    public var decodingSpeed: Int {
        return Int(barkoderView.config?.decoderConfig?.decodingSpeed.rawValue ?? 0)
    }
    
    @objc
    public var formattingType: Int {
        return Int(barkoderView.config?.decoderConfig?.formatting.rawValue ?? 0)
    }
    
    @objc
    public var locationLineWidth: Int {
        return Int(barkoderView.config?.locationLineWidth ?? 0)
    }
    
    @objc
    public func getRoiLineColorHex() -> String {
        return barkoderView.config?.roiLineColor.toHex() ?? ""
    }
    
    @objc
    public var roiLineWidth: Int {
        return Int(barkoderView.config?.roiLineWidth ?? 0)
    }
    
    @objc
    public func getRoiOverlayBackgroundColorHex() -> String {
        return barkoderView.config?.roiOverlayBackgroundColor.toHex() ?? ""
    }
    
    @objc
    public var closeSessionOnResultEnabled: Bool {
        return barkoderView.config?.closeSessionOnResultEnabled ?? false
    }
    
    @objc
    public var imageResultEnabled: Bool {
        return barkoderView.config?.imageResultEnabled ?? false
    }
    
    @objc
    public var locationInImageResultEnabled: Bool {
        return barkoderView.config?.locationInImageResultEnabled ?? false
    }
    
    @objc
    public func getRegionOfInterest() -> [CGFloat] {
        guard let roi = barkoderView.config?.getRegionOfInterest() else {
            return []
        }
        
        return [roi.minX, roi.minY, roi.width, roi.height]
    }
    
    @objc
    public var threadsLimit: Int {
        return barkoderView.config?.getThreadsLimit() ?? 0
    }
    
    @objc
    public func setThreadsLimit(threadsLimit: Int) {
        do {
            try barkoderView.config?.setThreadsLimit(threadsLimit)
        } catch {
            // TODO: -
        }
    }
    
    @objc
    public var isLocationInPreviewEnabled: Bool {
        return barkoderView.config?.locationInPreviewEnabled ?? false
    }
    
    @objc
    public var isPinchToZoomEnabled: Bool {
        return barkoderView.config?.pinchToZoomEnabled ?? false
    }
    
    @objc
    public var isRegionOfInterestVisible: Bool {
        barkoderView.config?.regionOfInterestVisible ?? false
    }
    
    @objc
    public var isBeepOnSuccessEnabled: Bool {
        return barkoderView.config?.beepOnSuccessEnabled ?? false
    }
    
    @objc
    public var isVibrateOnSuccessEnabled: Bool {
        return barkoderView.config?.vibrateOnSuccessEnabled ?? false
    }
    
    @objc
    public var version: String {
        return iBarkoder.GetVersion()
    }
    
    @objc
    public var msiChecksumType: MsiChecksum {
        return barkoderView.config?.decoderConfig?.msi.checksum ?? MsiChecksum(0)
    }
    
    @objc
    public func setMsiChecksumType(arg: MsiChecksum) {
        barkoderView.config?.decoderConfig?.msi.checksum = arg
    }
    
    @objc
    public var code39ChecksumType: Code39Checksum {
        return barkoderView.config?.decoderConfig?.code39.checksum ?? Code39Checksum(0)
    }
    
    @objc
    public func setCode39ChecksumType(arg: Code39Checksum) {
        barkoderView.config?.decoderConfig?.code39.checksum = arg
    }
    
    @objc
    public var code11ChecksumType: Code11Checksum {
        return barkoderView.config?.decoderConfig?.code11.checksum ?? Code11Checksum(0)
    }
    
    @objc
    public func setCode11ChecksumType(arg: Code11Checksum) {
        barkoderView.config?.decoderConfig?.code11.checksum = arg
    }
    
    @objc
    public func setBarkoderResolution(arg: Int) {
        if let barkoderResolution = BarkoderView.BarkoderResolution.init(rawValue: arg) {
            barkoderView.config?.barkoderResolution = barkoderResolution
        }
    }
    
    @objc
    public var barkoderResolution: Int {
        return barkoderView.config?.barkoderResolution.rawValue ?? 0
    }
    
    @objc
    public func setDecodingSpeed(arg: Int) {
        barkoderView.config?.decoderConfig?.decodingSpeed = DecodingSpeed.init(UInt32(arg))
    }
    
    @objc
    public func setEncodingCharacterSet(arg: String) {
        barkoderView.config?.decoderConfig?.encodingCharacterSet = arg
    }
    
    @objc
    public func setFormattingType(arg: Int) {
        barkoderView.config?.decoderConfig?.formatting = Formatting.init(UInt32(arg))
    }
    
    @objc
    public func setMaximumResultsCount(arg: Int) {
        barkoderView.config?.decoderConfig?.maximumResultsCount = Int32(arg)
    }

    @objc
    public func setDuplicatesDelayMs(arg: Int) {
        barkoderView.config?.decoderConfig?.duplicatesDelayMs = Int32(arg)
    }

    @objc
    public func setMulticodeCachingDuration(arg: Int) {
        barkoderView.config?.decoderConfig?.duplicatesDelayMs = Int32(arg)
    }

    @objc
    public func setMulticodeCachingEnabled(arg: Bool) {
        barkoderView.config?.setMulticodeCachingEnabled(arg)
    }
	
	@objc
	public func setDatamatrixDpmModeEnabled(arg: Bool) {
		barkoderView.config?.decoderConfig?.datamatrix.dpmMode = arg ? 1 : 0
	}
	
	@objc
	public func setUpcEanDeblurEnabled(arg: Bool) {
		barkoderView.config?.decoderConfig?.upcEanDeblur = arg
	}
    
    @objc
    public func setEnableVINRestrictions(arg: Bool) {
        barkoderView.config?.decoderConfig?.enableVINRestrictions = arg
    }
	
	@objc
	public func setMisshaped1DEnabled(arg: Bool) {
		barkoderView.config?.decoderConfig?.enableMisshaped1D = arg
	}
	
	@objc
	public func setBarcodeThumbnailOnResultEnabled(arg: Bool) {
		barkoderView.config?.barcodeThumbnailOnResult = arg
	}
	
	@objc 
	public var barcodeThumbnailOnResultEnabled: Bool {
		return barkoderView.config?.barcodeThumbnailOnResult ?? false
	}
	
	@objc 
	public var multicodeCachingEnabled: Bool {
		return barkoderView.config?.getMulticodeCachingEnabled() ?? BarkoderConfigDefaults.multicodeCachingEnabled
	}
    
    @objc
    public var datamatrixDpmModeEnabled: Bool {
        return (barkoderView.config?.decoderConfig?.datamatrix.dpmMode == 1)
    }
    
    @objc
    public var upcEanDeblurEnabled: Bool {
        return barkoderView.config?.decoderConfig?.upcEanDeblur ?? false
    }
    
    @objc
    public var enableMisshaped1D: Bool {
        return barkoderView.config?.decoderConfig?.enableMisshaped1D ?? false
    }
	
	@objc 
	public var multicodeCachingDuration: Int {
		return barkoderView.config?.getMulticodeCachingDuration() ?? BarkoderConfigDefaults.multicodeCachingDuration
	}

    @objc
    public var maximumResultsCount: Int {
        return Int(barkoderView.config?.decoderConfig?.maximumResultsCount ?? 0)
    }
    
    @objc
    public var duplicatesDelayMs: Int {
        return Int(barkoderView.config?.decoderConfig?.duplicatesDelayMs ?? 0)
    }
    
    @objc
    public var isImageResultEnabled: Bool {
        return barkoderView.config?.imageResultEnabled ?? false
    }
    
    @objc
    public var isCloseSessionOnResultEnabled: Bool {
        return barkoderView.config?.closeSessionOnResultEnabled ?? false
    }
    
    @objc
    public var isLocationInImageResultEnabled: Bool {
        return barkoderView.config?.locationInImageResultEnabled ?? false
    }
    
    @objc
    public func isBarcodeTypeEnabled(barcodeType: BarcodeType) -> Bool {
        guard let decoderConfig = barkoderView.config?.decoderConfig,
              let specificConfig = SpecificConfig(decoderType: .init(rawValue: barcodeType.rawValue)) else {
            // TODO: - Handle error for invalid barkoder config
            return false
        }
                            
        switch specificConfig.decoderType() {
        case Aztec:
            return decoderConfig.aztec.enabled
        case AztecCompact:
            return decoderConfig.aztecCompact.enabled
        case QR:
            return decoderConfig.qr.enabled
        case QRMicro:
            return decoderConfig.qrMicro.enabled
        case Code128:
            return decoderConfig.code128.enabled
        case Code93:
            return decoderConfig.code93.enabled
        case Code39:
            return decoderConfig.code39.enabled
        case Codabar:
            return decoderConfig.codabar.enabled
        case Code11:
            return decoderConfig.code11.enabled
        case Msi:
            return decoderConfig.msi.enabled
        case UpcA:
            return decoderConfig.upcA.enabled
        case UpcE:
            return decoderConfig.upcE.enabled
        case UpcE1:
            return decoderConfig.upcE1.enabled
        case Ean13:
            return decoderConfig.ean13.enabled
        case Ean8:
            return decoderConfig.ean8.enabled
        case PDF417:
            return decoderConfig.pdf417.enabled
        case PDF417Micro:
            return decoderConfig.pdf417Micro.enabled
        case Datamatrix:
            return decoderConfig.datamatrix.enabled
        case Code25:
            return decoderConfig.code25.enabled
        case Interleaved25:
            return decoderConfig.interleaved25.enabled
        case ITF14:
            return decoderConfig.itf14.enabled
        case IATA25:
            return decoderConfig.iata25.enabled
        case Matrix25:
            return decoderConfig.matrix25.enabled
        case Datalogic25:
            return decoderConfig.datalogic25.enabled
        case COOP25:
            return decoderConfig.coop25.enabled
        case Code32:
            return decoderConfig.code32.enabled
        case Telepen:
            return decoderConfig.telepen.enabled
		case Dotcode:
			return decoderConfig.dotcode.enabled
        default:
            // TODO: - Handle error for invalid barkoder config
            return false
        }
    }
    
    @objc
    public func setBarcodeTypeEnabled(barcodeType: BarcodeType, enabled: Bool) {
        guard let decoderConfig = barkoderView.config?.decoderConfig,
              let specificConfig = SpecificConfig(decoderType: .init(rawValue: barcodeType.rawValue)) else {
            // TODO: - Handle error for invalid barkoder config
            return
        }

        switch specificConfig.decoderType() {
        case Aztec:
            decoderConfig.aztec.enabled = enabled
        case AztecCompact:
            decoderConfig.aztecCompact.enabled = enabled
        case QR:
            decoderConfig.qr.enabled = enabled
        case QRMicro:
            decoderConfig.qrMicro.enabled = enabled
        case Code128:
            decoderConfig.code128.enabled = enabled
        case Code93:
            decoderConfig.code93.enabled = enabled
        case Code39:
            decoderConfig.code39.enabled = enabled
        case Codabar:
            decoderConfig.codabar.enabled = enabled
        case Code11:
            decoderConfig.code11.enabled = enabled
        case Msi:
            decoderConfig.msi.enabled = enabled
        case UpcA:
            decoderConfig.upcA.enabled = enabled
        case UpcE:
            decoderConfig.upcE.enabled = enabled
        case UpcE1:
            decoderConfig.upcE1.enabled = enabled
        case Ean13:
            decoderConfig.ean13.enabled = enabled
        case Ean8:
            decoderConfig.ean8.enabled = enabled
        case PDF417:
            decoderConfig.pdf417.enabled = enabled
        case PDF417Micro:
            decoderConfig.pdf417Micro.enabled = enabled
        case Datamatrix:
            decoderConfig.datamatrix.enabled = enabled
        case Code25:
            decoderConfig.code25.enabled = enabled
        case Interleaved25:
            decoderConfig.interleaved25.enabled  = enabled
        case ITF14:
            decoderConfig.itf14.enabled = enabled
        case IATA25:
            decoderConfig.iata25.enabled = enabled
        case Matrix25:
            decoderConfig.matrix25.enabled = enabled
        case Datalogic25:
            decoderConfig.datalogic25.enabled = enabled
        case COOP25:
            decoderConfig.coop25.enabled = enabled
        case Code32:
            decoderConfig.code32.enabled = enabled
        case Telepen:
            decoderConfig.telepen.enabled = enabled
		case Dotcode:
			decoderConfig.dotcode.enabled = enabled
        default:
            // TODO: - Handle error for invalid barkoder config
            break
        }
    }
    
    @objc
    public func setBarcodeTypeLengthRange(barcodeType: BarcodeType, minValue: Int, maxValue: Int) {
        guard let decoderConfig = barkoderView.config?.decoderConfig,
              let specificConfig = SpecificConfig(decoderType: .init(rawValue: barcodeType.rawValue)) else {
            // TODO: - Handle error for invalid barkoder config
            return
        }
        
        switch specificConfig.decoderType() {
        case Code128:
            decoderConfig.code128.setLengthRangeWithMinimum(Int32(minValue), maximum: Int32(maxValue))
        case Code93:
            decoderConfig.code93.setLengthRangeWithMinimum(Int32(minValue), maximum: Int32(maxValue))
        case Code39:
            decoderConfig.code39.setLengthRangeWithMinimum(Int32(minValue), maximum: Int32(maxValue))
        case Code11:
            decoderConfig.code11.setLengthRangeWithMinimum(Int32(minValue), maximum: Int32(maxValue))
        case Msi:
            decoderConfig.msi.setLengthRangeWithMinimum(Int32(minValue), maximum: Int32(maxValue))
        case Codabar:
            decoderConfig.codabar.setLengthRangeWithMinimum(Int32(minValue), maximum: Int32(maxValue))
        default:
            // TODO: - Handle error for invalid barkoder config
            break
        }
    }
    
    @objc
    public func getBarcodeTypeLengthRange(barcodeType: BarcodeType) -> [Int] {
        guard let decoderConfig = barkoderView.config?.decoderConfig,
              let specificConfig = SpecificConfig(decoderType: .init(rawValue: barcodeType.rawValue)) else {
            // TODO: - Handle error for invalid barkoder config
            return []
        }
        
        switch specificConfig.decoderType() {
        case Code128:
            return [decoderConfig.code128.minimumLength, decoderConfig.code128.maximumLength]
                .map { Int($0) }
        case Code93:
            return [decoderConfig.code93.minimumLength, decoderConfig.code93.maximumLength]
                .map { Int($0) }
        case Code39:
            return [decoderConfig.code39.minimumLength, decoderConfig.code39.maximumLength]
                .map { Int($0) }
        case Code11:
            return [decoderConfig.code11.minimumLength, decoderConfig.code11.maximumLength]
                .map { Int($0) }
        case Msi:
            return [decoderConfig.code128.minimumLength, decoderConfig.code128.maximumLength]
                .map { Int($0) }
        case Codabar:
            return [decoderConfig.code128.minimumLength, decoderConfig.code128.maximumLength]
                .map { Int($0) }
        default:
            // TODO: - Handle error for invalid barkoder config
            return []
        }
    }
    
    @objc
    public func configureBarkoder(barkoderConfigAsJsonString: String) {
        let barkoderJsonData: Data
        
        // Converting from String -> Data
        // Converting from Data -> Dictionary
        // Changing values for colors from hexes to decimal values
        // Converting from Dictionary -> Data with utf8 encoding
        do {
            let barkoderConfigAsData = Data(barkoderConfigAsJsonString.utf8)
            
            var barkoderConfigAsDictionary = try JSONSerialization.jsonObject(with: barkoderConfigAsData, options: []) as? [String: Any]
            
            if let colorHexCode = barkoderConfigAsDictionary?["roiLineColor"] as? String {
                barkoderConfigAsDictionary?["roiLineColor"] = Util.parseColor(hexColor: colorHexCode)
            }
            
            if let colorHexCode = barkoderConfigAsDictionary?["locationLineColor"] as? String {
                barkoderConfigAsDictionary?["locationLineColor"] = Util.parseColor(hexColor: colorHexCode)
            }
            
            if let colorHexCode = barkoderConfigAsDictionary?["roiOverlayBackgroundColor"] as? String {
                barkoderConfigAsDictionary?["roiOverlayBackgroundColor"] = Util.parseColor(hexColor: colorHexCode)
            }
            
            let jsonData = try JSONSerialization.data(withJSONObject: barkoderConfigAsDictionary as Any, options: .prettyPrinted)
            
            let convertedBarkoderConfigAsString = String(data: jsonData, encoding: .utf8) ?? ""
            
            barkoderJsonData = Data(convertedBarkoderConfigAsString.utf8)
        } catch {
            return
        }
        
        if let config = barkoderView.config {
            BarkoderHelper.applyConfigSettingsFromJson(config, jsonData: barkoderJsonData) { updatedConfig, _ in
                self.barkoderView.config = updatedConfig
            }
        }
    }
        
}

@objc(BarkoderConfig)
public class BKDConfig: NSObject {
    
    public var licenseKey: String
    
    @objc
    public init(licenseKey: String) {
        self.licenseKey = licenseKey
        
        super.init()
    }
    
    @objc
    public var closeSessionOnResultEnabled: Bool = false {
        didSet {
            modelDidChange?()
        }
    }
    
    @objc
    public var vibrateOnSuccessEnabled: Bool = false {
        didSet {
            modelDidChange?()
        }
    }

    @objc
    public var beepOnSuccessEnabled: Bool = false {
        didSet {
            modelDidChange?()
        }
    }

    @objc
    public var pinchToZoomEnabled: Bool = false {
        didSet {
            modelDidChange?()
        }
    }
    
    @objc public var locationInPreviewEnabled: Bool = BarkoderConfigDefaults.locationInPreviewEnabled {
        didSet {
            modelDidChange?()
        }
    }
    
    @objc public var roiLineColor: UIColor = BarkoderConfigDefaults.roiLineColor {
        didSet {
            modelDidChange?()
        }
    }

    @objc public var roiLineWidth: Float = BarkoderConfigDefaults.roiLineWidth {
        didSet {
            modelDidChange?()
        }
    }
    
    @objc public var roiOverlayBackgroundColor: UIColor = BarkoderConfigDefaults.roiOverlayBackgroundColor {
        didSet {
            modelDidChange?()
        }
    }

    @objc public var regionOfInterestVisible: Bool = BarkoderConfigDefaults.regionOfInterestVisible {
        didSet {
            modelDidChange?()
        }
    }

    @objc public var locationLineColor: UIColor = BarkoderConfigDefaults.locationLineColor {
        didSet {
            modelDidChange?()
        }
    }

    @objc public var locationLineWidth: Float = BarkoderConfigDefaults.locationLineWidth {
        didSet {
            modelDidChange?()
        }
    }
    
    @objc public var imageResultEnabled: Bool = BarkoderConfigDefaults.imageResultEnabled {
        didSet {
            modelDidChange?()
        }
    }

    @objc public var locationInImageResultEnabled: Bool = BarkoderConfigDefaults.locationInImageResultEnabled {
        didSet {
            modelDidChange?()
        }
    }
    
    @objc public var thresholdBetweenDuplicatesScans: Int = BarkoderConfigDefaults.thresholdBetweenDuplicatesScans {
        didSet {
            modelDidChange?()
        }
    }
        
    @objc public var decoderConfig: BKDDecoderConfig? = nil {
        didSet {
            decoderConfig?.setEnabledDecodersCallback(callback: { [weak self] symbologies in
                self?.enabledDecodersCallback?(symbologies)
            })
            
            decoderConfig?.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }

    var modelDidChange: (() -> Void)?
    var enabledDecodersCallback: (([Int]) -> Void)?

    func setModelDidChangeCallback(callback: @escaping () -> Void) {
        modelDidChange = callback
    }
    
    func setEnabledDecodersCallback(callback: @escaping ([Int]) -> Void) {
        self.enabledDecodersCallback = callback
    }
	

}

@objc(DecoderConfig)
public class BKDDecoderConfig: NSObject {
    
    var modelDidChange: (() -> Void)?
    var enabledDecodersCallback: (([Int]) -> Void)?

    @objc public var enableMisshaped1D: Bool = BarkoderConfigDefaults.enableMisshaped1D {
        didSet {
            modelDidChange?()
        }
    }
    
    @objc public var upcEanDeblur: Bool = BarkoderConfigDefaults.upcEanDeblur {
        didSet {
            modelDidChange?()
        }
    }
    
    @objc public var enableVINRestrictions: Bool = BarkoderConfigDefaults.enableVINRestrictions {
        didSet {
            modelDidChange?()
        }
    }
    
    @objc
    public var aztec: SymbologyConfig  {
        didSet {
            aztec.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var aztecCompact: SymbologyConfig  {
        didSet {
            aztecCompact.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var qr: SymbologyConfig  {
        didSet {
            qr.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var qrMicro: SymbologyConfig  {
        didSet {
            qrMicro.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var code128: SymbologyConfig  {
        didSet {
            code128.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var code93: SymbologyConfig  {
        didSet {
            code93.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var code39: SymbologyConfig  {
        didSet {
            code39.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var codabar: SymbologyConfig  {
        didSet {
            codabar.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var code11: SymbologyConfig  {
        didSet {
            code11.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var msi: SymbologyConfig  {
        didSet {
            msi.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
        
    @objc
    public var upcA: SymbologyConfig  {
        didSet {
            upcA.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var upcE: SymbologyConfig  {
        didSet {
            upcE.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var upcE1: SymbologyConfig  {
        didSet {
            upcE1.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var ean8: SymbologyConfig  {
        didSet {
            ean8.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }

    @objc
    public var ean13: SymbologyConfig  {
        didSet {
            ean13.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var pdf417: SymbologyConfig  {
        didSet {
            pdf417.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var pdf417Micro: SymbologyConfig  {
        didSet {
            pdf417Micro.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
        
    @objc
    public var datamatrix: DatamatrixConfig  {
        didSet {
            datamatrix.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var code25: SymbologyConfig  {
        didSet {
            code25.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var interleaved25: SymbologyConfig  {
        didSet {
            interleaved25.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var itf14: SymbologyConfig  {
        didSet {
            itf14.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var iata25: SymbologyConfig  {
        didSet {
            iata25.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var matrix25: SymbologyConfig  {
        didSet {
            matrix25.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var datalogic25: SymbologyConfig  {
        didSet {
            datalogic25.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var coop25: SymbologyConfig  {
        didSet {
            coop25.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var code32: SymbologyConfig  {
        didSet {
            code32.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var telepen: SymbologyConfig  {
        didSet {
            telepen.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
	
	@objc
	public var dotcode: SymbologyConfig  {
		didSet {
			dotcode.setModelDidChangeCallback {
				self.modelDidChange?()
			}
		}
	}
    
    override public init() {
        aztec = SymbologyConfig()
        aztecCompact = SymbologyConfig()
        qr = SymbologyConfig()
        qrMicro = SymbologyConfig()
        code128 = SymbologyConfig()
        code93 = SymbologyConfig()
        code39 = SymbologyConfig()
        codabar = SymbologyConfig()
        code11 = SymbologyConfig()
        msi = SymbologyConfig()
        upcA = SymbologyConfig()
        upcE = SymbologyConfig()
        upcE1 = SymbologyConfig()
        ean13 = SymbologyConfig()
        ean8 = SymbologyConfig()
        pdf417 = SymbologyConfig()
        pdf417Micro = SymbologyConfig()
        datamatrix = DatamatrixConfig()
        code25 = SymbologyConfig()
        interleaved25 = SymbologyConfig()
        itf14 = SymbologyConfig()
        iata25 = SymbologyConfig()
        matrix25 = SymbologyConfig()
        datalogic25 = SymbologyConfig()
        coop25 = SymbologyConfig()
        code32 = SymbologyConfig()
        telepen = SymbologyConfig()
		dotcode = SymbologyConfig()
        
        super.init()
    }
    
    @objc
    public func setEnabledDecoders(symbologies: [Int]) {
        enabledDecodersCallback?(symbologies)
    }

    func setEnabledDecodersCallback(callback: @escaping ([Int]) -> Void) {
        self.enabledDecodersCallback = callback
    }
    
    func setModelDidChangeCallback(callback: @escaping () -> Void) {
        modelDidChange = callback
    }
    
}

@objc(SymbologyConfig)
public class SymbologyConfig: NSObject {
    var modelDidChange: (() -> Void)?

    @objc
    public var enabled: Bool = false {
        didSet {
            modelDidChange?()
        }
    }
    
    func setModelDidChangeCallback(callback: @escaping () -> Void) {
        modelDidChange = callback
    }

}

@objc(DatamatrixConfig)
public class DatamatrixConfig: NSObject {
    var modelDidChange: (() -> Void)?

    @objc
    public var enabled: Bool = false {
        didSet {
            modelDidChange?()
        }
    }
    
    @objc
    public var dpmMode: Int = 0 {
        didSet {
            modelDidChange?()
        }
    }
    
    func setModelDidChangeCallback(callback: @escaping () -> Void) {
        modelDidChange = callback
    }

}

extension UIColor {
    
    func toHex() -> String {
        guard let components = cgColor.components, components.count >= 3 else {
            // Should never execute
            return "000000"
        }
        
        let red = Float(components[0])
        let green = Float(components[1])
        let blue = Float(components[2])
        var alpha: Float = 0
        if components.count >= 4 {
            alpha = Float(components[3])
        }
        
        let hexString = String(
            format: "#%02lX%02lX%02lX%02lX",
            lroundf(alpha * 255),
            lroundf(red * 255),
            lroundf(green * 255),
            lroundf(blue * 255)
        )
        
        return hexString
    }
    
}

private class Util {
    
    static func parseColor(hexColor: String) -> Int? {
        Int(hexColor.replacingOccurrences(of: "#", with: ""), radix: 16)
    }
    
    static func initColorWith(hexString: String) -> UIColor {
        let hex = hexString.trimmingCharacters(in: CharacterSet.alphanumerics.inverted)
        var int = UInt64()
        Scanner(string: hex).scanHexInt64(&int)
        let a, r, g, b: UInt64
        switch hex.count {
        case 3: // RGB (12-bit)
            (a, r, g, b) = (255, (int >> 8) * 17, (int >> 4 & 0xF) * 17, (int & 0xF) * 17)
        case 6: // RGB (24-bit)
            (a, r, g, b) = (255, int >> 16, int >> 8 & 0xFF, int & 0xFF)
        case 8: // ARGB (32-bit)
            (a, r, g, b) = (int >> 24, int >> 16 & 0xFF, int >> 8 & 0xFF, int & 0xFF)
        default:
            return .black
        }
        
        return UIColor(red: CGFloat(r) / 255, green: CGFloat(g) / 255, blue: CGFloat(b) / 255, alpha: CGFloat(a) / 255)
    }
    
}

@objc(DecoderPayload)
public class DecoderPayload: NSObject {
    @objc
    public var results: [DecoderResult] = []
    
    @objc
    public var imageInBase64: String = ""
}

@objc(DecoderResult)
public extension DecoderResult {}

extension BarkoderProxy: BarkoderResultDelegate {
    
    public func scanningFinished(_ decoderResults: [DecoderResult], thumbnails: [UIImage]?, image: UIImage?) {
        let decoderPayload = DecoderPayload()
        decoderPayload.results = decoderResults
        
        if let imageData = image?.jpegData(compressionQuality: 1.0) {
            let base64String = imageData.base64EncodedString()
            decoderPayload.imageInBase64 = base64String
        }
        
        DispatchQueue.main.async {
            self.resultCallbackDecoderResult?(decoderPayload)
        }
    }
    
}

class BarkoderConfigDefaults {
    
    static let closeSessionOnResult = true
    static let imageResultEnabled = false
    static let locationInImageResultEnabled = false
    static let locationInPreviewEnabled = true
    static let decoderThreads = 2
    static let regionOfInterest: CGRect = CGRect(x: 3, y: 30, width: 94, height: 40)
    static let pinchToZoomEnabled: Bool = false
    static let beepOnSuccessEnabled: Bool = true
    static let vibrateOnSuccessEnabled: Bool = true
    static let regionOfInterestVisible: Bool = true
    static let barcodeThumbnailOnResult: Bool = true
    static let enableVINRestrictions: Bool = false
    static let thresholdBetweenDuplicatesScans: Int = 5
    static let enableMisshaped1D: Bool = false
    static let upcEanDeblur: Bool = false
    static let locationLineColor: UIColor = #colorLiteral(red: 0.8, green: 0.003921568627, blue: 0.09803921569, alpha: 1)
    static let locationLineWidth: Float = 2
    static let roiLineColor: UIColor = #colorLiteral(red: 0.8, green: 0.003921568627, blue: 0.09803921569, alpha: 1)
    static let roiLineWidth: Float = 2
    static let roiOverlayBackgroundColor: UIColor = #colorLiteral(red: 0, green: 0, blue: 0, alpha: 0.3950227649)
	static let barcodeThumbnailOnResultEnabled: Bool = true
	static let multicodeCachingEnabled: Bool = false
	static let multicodeCachingDuration: Int = 3000
}
