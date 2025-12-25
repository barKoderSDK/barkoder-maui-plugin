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
                barkoderView.config?.arConfig.emitResultsAtSessionEndOnly = config.AREmitResultsAtSessionEndOnly
                barkoderView.config?.arConfig.continueScanningOnLimit = config.ARContinueScanningOnLimit
                barkoderView.config?.beepOnSuccessEnabled = config.beepOnSuccessEnabled
                barkoderView.config?.scanningIndicatorAlwaysVisible = config.scanningIndicatorAlwaysVisible
                barkoderView.config?.vibrateOnSuccessEnabled = config.vibrateOnSuccessEnabled
                barkoderView.config?.locationInPreviewEnabled = config.locationInPreviewEnabled
                barkoderView.config?.roiLineColor = config.roiLineColor
                barkoderView.config?.scanningIndicatorColor = config.scanningIndicatorColor
                barkoderView.config?.roiLineWidth = config.roiLineWidth
                barkoderView.config?.scanningIndicatorWidth = config.scanningIndicatorWidth
                barkoderView.config?.roiOverlayBackgroundColor = config.roiOverlayBackgroundColor
                barkoderView.config?.regionOfInterestVisible = config.regionOfInterestVisible
                barkoderView.config?.locationLineColor = config.locationLineColor
                barkoderView.config?.locationLineWidth = config.locationLineWidth
                barkoderView.config?.imageResultEnabled = config.imageResultEnabled
                barkoderView.config?.locationInImageResultEnabled = config.locationInImageResultEnabled
                barkoderView.config?.thresholdBetweenDuplicatesScans = config.thresholdBetweenDuplicatesScans
                barkoderView.config?.scanningIndicatorAnimation = config.scanningIndicatorAnimationMode
                barkoderView.config?.decoderConfig?.enableComposite = Int32(config.decoderConfig?.enableComposite ?? 0)
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
                barkoderView.config?.decoderConfig?.ocrText.enabled = config.decoderConfig?.ocrText.enabled ?? false
                barkoderView.config?.decoderConfig?.postalIMB.enabled = config.decoderConfig?.postalIMB.enabled ?? false
                barkoderView.config?.decoderConfig?.postnet.enabled = config.decoderConfig?.postnet.enabled ?? false
                barkoderView.config?.decoderConfig?.planet.enabled = config.decoderConfig?.planet.enabled ?? false
                barkoderView.config?.decoderConfig?.australianPost.enabled = config.decoderConfig?.australianPost.enabled ?? false
                barkoderView.config?.decoderConfig?.royalMail.enabled = config.decoderConfig?.royalMail.enabled ?? false
                barkoderView.config?.decoderConfig?.kix.enabled = config.decoderConfig?.kix.enabled ?? false
                barkoderView.config?.decoderConfig?.japanesePost.enabled = config.decoderConfig?.japanesePost.enabled ?? false
                barkoderView.config?.decoderConfig?.ean8.enabled = config.decoderConfig?.ean8.enabled ?? false
                barkoderView.config?.decoderConfig?.pdf417.enabled = config.decoderConfig?.pdf417.enabled ?? false
                barkoderView.config?.decoderConfig?.pdf417Micro.enabled = config.decoderConfig?.pdf417Micro.enabled ?? false
                barkoderView.config?.decoderConfig?.datamatrix.enabled = config.decoderConfig?.datamatrix.enabled ?? false
                barkoderView.config?.decoderConfig?.datamatrix.dpmMode = Int32(config.decoderConfig?.datamatrix.dpmMode ?? 0)
                barkoderView.config?.decoderConfig?.qr.dpmMode = Int32(config.decoderConfig?.qr.dpmMode ?? 0)
                barkoderView.config?.decoderConfig?.qrMicro.dpmMode = Int32(config.decoderConfig?.qrMicro.dpmMode ?? 0)
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
                barkoderView.config?.decoderConfig?.maxiCode.enabled = config.decoderConfig?.maxicode.enabled ?? false
                barkoderView.config?.decoderConfig?.idDocument.enabled = config.decoderConfig?.idDocument.enabled ?? false
                barkoderView.config?.decoderConfig?.databar14.enabled = config.decoderConfig?.databar14.enabled ?? false
                barkoderView.config?.decoderConfig?.databarLimited.enabled = config.decoderConfig?.databarLimited.enabled ?? false
                barkoderView.config?.decoderConfig?.databarExpanded.enabled = config.decoderConfig?.databarExpanded.enabled ?? false

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
    
    @objc(scanImageWithCompletion:bkdConfig:completion:)
    public func scanImage(image: UIImage, config: BKDConfig, completion: @escaping (DecoderPayload) -> Void) {
        // Ensure `barkoderView` exists and is properly configured
        guard let viewConfig = barkoderView?.config else {
            return
        }
        // Assign the provided completion handler to the class-level variable
        self.resultCallbackDecoderResult = completion

        // Use BarkoderSDK to start scanning
        BarkoderHelper.scanImage(image, bkdConfig: viewConfig, resultDelegate: self)
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
    public func configureCloseButton(
        visible: Bool,
        position: NSValue? = nil,
        iconSize: NSNumber? = nil,
        tintColor: UIColor? = nil,
        backgroundColor: UIColor? = nil,
        cornerRadius: NSNumber? = nil,
        padding: NSNumber? = nil,
        useCustomIcon: NSNumber? = nil,
        customIcon: UIImage? = nil,
        onClose: (@convention(block) () -> Void)? = nil
    ) {
        // Pass all received arguments directly to the barkoderView's function
        barkoderView.configureCloseButton(
            visible: visible,
            position: position,
            iconSize: iconSize,
            tintColor: tintColor,
            backgroundColor: backgroundColor,
            cornerRadius: cornerRadius,
            padding: padding,
            useCustomIcon: useCustomIcon,
            customIcon: customIcon,
            onClose: onClose
        )
    }
    
    @objc
    public func configureFlashButton(
        visible: Bool,
        position: NSValue? = nil,
        iconSize: NSNumber? = nil,
        tintColor: UIColor? = nil,
        backgroundColor: UIColor? = nil,
        cornerRadius: NSNumber? = nil,
        padding: NSNumber? = nil,
        useCustomIcon: NSNumber? = nil,
        customIconFlashOn: UIImage? = nil,
        customIconFlashOff: UIImage? = nil
    ) {
        // Pass all received arguments directly to the barkoderView's function
        barkoderView.configureFlashButton(
            visible: visible,
            position: position,
            iconSize: iconSize,
            tintColor: tintColor,
            backgroundColor: backgroundColor,
            cornerRadius: cornerRadius,
            padding: padding,
            useCustomIcon: useCustomIcon,
            customIconFlashOn: customIconFlashOn,
            customIconFlashOff: customIconFlashOff
        )
    }
    
    @objc
    public func configureZoomButton(
        visible: Bool,
        position: NSValue? = nil,
        iconSize: NSNumber? = nil,
        tintColor: UIColor? = nil,
        backgroundColor: UIColor? = nil,
        cornerRadius: NSNumber? = nil,
        padding: NSNumber? = nil,
        useCustomIcon: NSNumber? = nil,
        customIconZoomedIn: UIImage? = nil,
        customIconZoomedOut: UIImage? = nil,
        zoomedInFactor: NSNumber? = nil,
        zoomedOutFactor: NSNumber? = nil
    ) {
        // Pass all received arguments directly to the barkoderView's function
        barkoderView.configureZoomButton(
            visible: visible,
            position: position,
            iconSize: iconSize,
            tintColor: tintColor,
            backgroundColor: backgroundColor,
            cornerRadius: cornerRadius,
            padding: padding,
            useCustomIcon: useCustomIcon,
            customIconZoomedIn: customIconZoomedIn,
            customIconZoomedOut: customIconZoomedOut,
            zoomedInFactor: zoomedInFactor,
            zoomedOutFactor: zoomedOutFactor
        )
    }
         
    @objc
    public func captureImage() {
        barkoderView.captureImage()
    }

    @objc
    public func pauseScanning() {
        barkoderView.pauseScanning()
    }
    
    @objc
    public func freezeScanning() {
        barkoderView.freezeScanning()
    }
    
    @objc
    public func unfreezeScanning() {
        barkoderView.unfreezeScanning()
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
    public func setARNonSelectedLocationLineColor(arg: String) {
        barkoderView.config?.arConfig.nonSelectedLocationColor = Util.initColorWith(hexString: arg)
    }
    
    @objc
    public func setARSelectedLocationLineColor(arg: String) {
        barkoderView.config?.arConfig.selectedLocationColor = Util.initColorWith(hexString: arg)
    }
    
    @objc
    public func setARNonSelectedHeaderTextColor(arg: String) {
        barkoderView.config?.arConfig.headerTextColorNonSelected = Util.initColorWith(hexString: arg)
    }
    
    @objc
    public func setShowDuplicatesLocation(arg: Bool) {
        barkoderView.config?.showDuplicatesLocations = arg
    }
    
    @objc
    public func setARSelectedHeaderTextColor(arg: String) {
        barkoderView.config?.arConfig.headerTextColorSelected = Util.initColorWith(hexString: arg)
    }
    
    @objc
    public func setARSelectedLocationLineWidth(arg: Float) {
        barkoderView.config?.arConfig.selectedLocationLineWidth = arg
    }
    
    @objc
    public func setARNonSelectedLocationLineWidth(arg: Float) {
        barkoderView.config?.arConfig.nonSelectedLocationLineWidth = arg
    }
    
    @objc
    public func setARHeaderMaxTextHeight(arg: Float) {
        barkoderView.config?.arConfig.headerMaxTextHeight = arg
    }
    @objc
    public func setARHeaderMinTextHeight(arg: Float) {
        barkoderView.config?.arConfig.headerMinTextHeight = arg
    }
    
    @objc
    public func setARHeaderHeight(arg: Float) {
        barkoderView.config?.arConfig.headerHeight = arg
    }
    
    @objc
    public func setARHeaderVerticalTextMargin(arg: Float) {
        barkoderView.config?.arConfig.headerVerticalTextMargin = arg
    }
    
    @objc
    public func setARHeaderHorizontalTextMargin(arg: Float) {
        barkoderView.config?.arConfig.headerHorizontalTextMargin = arg
    }
    
    @objc
    public func setARDoubleTapToFreezeEnabled(arg: Bool) {
        barkoderView.config?.arConfig.doubleTapToFreezeEnabled = arg
    }
    
    @objc
    public func setResultDisappearanceDelayMs(arg: Int) {
        barkoderView.config?.arConfig.resultDisappearanceDelayMs = arg
    }
    
    @objc
    public func setARResultLimit(arg: Int) {
        barkoderView.config?.arConfig.resultLimit = arg
    }
    
    @objc
    public func setARLocationTransitionSpeed(arg: Float) {
        barkoderView.config?.arConfig.locationTransitionSpeed = arg
    }
    
    @objc
    public func setARHeaderTextFormat(arg: String) {
        barkoderView.config?.arConfig.headerTextFormat = arg
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
    public func setARContinueScanningOnLimit(arg: Bool) {
        barkoderView.config?.arConfig.continueScanningOnLimit = arg
    }
    
    @objc
    public func setAREmitResultsAtSessionEndOnly(arg: Bool) {
        barkoderView.config?.arConfig.emitResultsAtSessionEndOnly = arg
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
    public func getARSelectedLocationLineColor() -> String {
        return barkoderView.config?.arConfig.selectedLocationColor.toHex() ?? ""
    }
    
    @objc
    public func getARNonSelectedLocationLineColor() -> String {
        return barkoderView.config?.arConfig.nonSelectedLocationColor.toHex() ?? ""
    }
    
    @objc
    public func getARHeaderHeight() -> Float {
        return barkoderView.config?.arConfig.headerHeight ?? 0.0
    }
    
    @objc
    public func getARSelectedLocationWidth() -> Float {
        return barkoderView.config?.arConfig.selectedLocationLineWidth ?? 0.0
    }
    
    @objc
    public func getARNonSelectedLocationWidth() -> Float {
        return barkoderView.config?.arConfig.nonSelectedLocationLineWidth ?? 0.0
    }
    
    @objc
    public func getARMode() -> BarkoderARMode {
        return barkoderView.config?.arConfig.arMode ?? BarkoderARMode.off
    }
    
    @objc
    public func getARLocationType() -> BarkoderARLocationType {
        return barkoderView.config?.arConfig.locationType ?? BarkoderARLocationType.tight
    }
    
    @objc
    public func getARHeaderShowMode() -> BarkoderARHeaderShowMode {
        return barkoderView.config?.arConfig.headerShowMode ?? BarkoderARHeaderShowMode.onSelected
    }
    
    @objc
    public func getResultDisappereanceDelayMs() -> Int {
        return barkoderView.config?.arConfig.resultDisappearanceDelayMs ?? 0
    }
    @objc
    public func getARResultLimit() -> Int {
        return barkoderView.config?.arConfig.resultLimit ?? 0
    }
    
    @objc
    public func getLocationTransitionSpeed() -> Float {
        return barkoderView.config?.arConfig.locationTransitionSpeed ?? 0.0
    }
    
    @objc
    public func getAROverlayRefresh() -> BarkoderAROverlayRefresh {
        return barkoderView.config?.arConfig.overlayRefresh ?? BarkoderAROverlayRefresh.smooth
    }
    
    @objc
    public func getARDoubleTapToFreeze() -> Bool {
        return barkoderView.config?.arConfig.doubleTapToFreezeEnabled ?? true
    }
    
    @objc
    public func getARHeaderMaxTextHeight() -> Float {
        return barkoderView.config?.arConfig.headerMaxTextHeight ?? 0.0
    }
    
    @objc
    public func getARHeaderMinTextHeight() -> Float {
        return barkoderView.config?.arConfig.headerMinTextHeight ?? 0.0
    }
    
    @objc
    public func getARSelectedHeaderTextColor() -> String {
        return barkoderView.config?.arConfig.headerTextColorSelected.toHex() ?? ""
    }
    
    @objc
    public func getARNonSelectedHeaderTextColor() -> String {
        return barkoderView.config?.arConfig.headerTextColorNonSelected.toHex() ?? ""
    }
    
    @objc
    public func getARHeaderHorizontalTextMargin() -> Float {
        return barkoderView.config?.arConfig.headerHorizontalTextMargin ?? 0.0
    }
    
    @objc
    public func getARHeaderVerticalTextMargin() -> Float {
        return barkoderView.config?.arConfig.headerVerticalTextMargin ?? 0.0
    }
    
    @objc
    public func getARHeaderTextFormat() -> String {
        return barkoderView.config?.arConfig.headerTextFormat ?? ""
    }
    
    @objc
    public func getScanningIndicatorColorHex() -> String {
        return barkoderView.config?.scanningIndicatorColor.toHex() ?? ""
    }
    
    @objc
    public var roiLineWidth: Int {
        return Int(barkoderView.config?.roiLineWidth ?? 0)
    }
    
    @objc
    public var scanningIndicatorWidth: Int {
        return Int(barkoderView.config?.scanningIndicatorWidth ?? 0)
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
    public var aRContinueScanningOnLimit: Bool {
        return barkoderView.config?.arConfig.continueScanningOnLimit ?? false
    }
    
    @objc
    public var aREmitResultsAtSessionEndOnly: Bool {
        return barkoderView.config?.arConfig.emitResultsAtSessionEndOnly ?? false
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
    public var libVersion: String {
        return iBarkoder.getLibVersion()
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
      public func setBarkoderARMode(arg: Int) {
        if let barkoderARMode = BarkoderARMode.init(rawValue: arg) {
          barkoderView.config?.arConfig.arMode = barkoderARMode
        }
      }
    
    @objc
      public func setBarkoderARLocationType(arg: Int) {
          if let barkoderARLocationType = BarkoderARLocationType.init(rawValue: arg) {
              barkoderView.config?.arConfig.locationType = barkoderARLocationType
        }
      }
    
    @objc
      public func setBarkoderARoverlayRefresh(arg: Int) {
          if let barkoderAROverlayRefresh = BarkoderAROverlayRefresh.init(rawValue: arg) {
              barkoderView.config?.arConfig.overlayRefresh = barkoderAROverlayRefresh
        }
      }
    
    @objc
      public func setBarkoderARHeaderShowMode(arg: Int) {
          if let barkoderHeaderShowMode = BarkoderARHeaderShowMode.init(rawValue: arg) {
              barkoderView.config?.arConfig.headerShowMode = barkoderHeaderShowMode
        }
      }
    
    @objc
    public var barkoderARMode: Int {
        return barkoderView.config?.arConfig.arMode.rawValue ?? 0
    }
    
    @objc
    public var barkoderARLocationType: Int {
        return barkoderView.config?.arConfig.locationType.rawValue ?? 0
    }
    
    @objc
    public var barkoderARHeaderShowMode: Int {
        return barkoderView.config?.arConfig.headerShowMode.rawValue ?? 0
    }
    
    @objc
    public var barkoderAROverlayRefresh: Int {
        return barkoderView.config?.arConfig.overlayRefresh.rawValue ?? 0
    }
    
    
    @objc
    public var barkoderResolution: Int {
        return barkoderView.config?.barkoderResolution.rawValue ?? 0
    }
    
    @objc
    public func setCamera(arg: BarkoderView.BarkoderCameraPosition) {
        barkoderView.setCamera(arg)
        
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
    public func setDynamicExposure(arg: Int) {
        barkoderView.setDynamicExposure(arg);
    }
    
    @objc
    public func sadlImage(fromExtra extra: [AnyHashable: Any]) -> UIImage? {
        return BarkoderHelper.sadlImage(fromExtra: extra)
    }
    
    @objc
    public func setCentricFocusAndExposure(arg: Bool) {
        barkoderView.setCentricFocusAndExposure(arg);
    }
    
    @objc
    public func setUPCEexpandToUPCA(arg: Bool) {
        barkoderView.config?.decoderConfig?.upcE.expandToUPCA = arg;
    }
    
    @objc
    public func setUPCE1expandToUPCA(arg: Bool) {
        barkoderView.config?.decoderConfig?.upcE1.expandToUPCA = arg;
    }
    
    @objc
    public func setVideoStabilization(arg: Bool) {
        barkoderView.setVideoStabilization(arg);
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
    public func setIdDocumentMasterChecksumEnabled(arg: Bool) {
        barkoderView.config?.decoderConfig?.idDocument.masterChecksum = arg ? StandardChecksum(1) : StandardChecksum(0)
    }
    
    @objc
    public func setCustomOption(option: String, value: Int32) {
        guard let decoderConfig = barkoderView.config?.decoderConfig else {
            print("DecoderConfig is not initialized.")
            return
        }
        decoderConfig.setcustomOption(option, value: value)
        print("Custom option set: \(option) = \(value)")
    }
    
    @objc
    public static func setCustomOptionGlobal(option: String, value: Int32) {
        Config.setcustomOptionGlobal(option, value: value)
    }
    
    @objc
    public func selectVisibleBarcodes() {
        barkoderView.selectVisibleBarcodes()
    }
    
    @objc
    public func setScanningIndicatorAnimationMode(arg: Int) {
        barkoderView.config?.scanningIndicatorAnimation = arg;
    }
    
    @objc
    public func setEnableComposite(arg: Int) {
        barkoderView.config?.decoderConfig?.enableComposite = Int32(arg)
    }
    
    @objc
    public func getLicenseInfo() -> [String: String] {
        return Config.getLicenseInfo()
    }
    
    @objc
    public func setScanningIndicatorColorHex(arg: String) {
        barkoderView.config?.scanningIndicatorColor = Util.initColorWith(hexString: arg)
    }
    
    @objc
    public func setScanningIndicatorWidth(arg: Float) {
        barkoderView.config?.scanningIndicatorWidth = arg
    }
    
    @objc
    public func setScanningIndicatorAlwaysVisible(arg: Bool) {
        barkoderView.config?.scanningIndicatorAlwaysVisible = arg
    }
  
	@objc
	public func setDatamatrixDpmModeEnabled(arg: Bool) {
		barkoderView.config?.decoderConfig?.datamatrix.dpmMode = arg ? 1 : 0
	}
    
    @objc
    public func setQRDpmModeEnabled(arg: Bool) {
        barkoderView.config?.decoderConfig?.qr.dpmMode = arg ? 1 : 0
    }
    
    @objc(setQRMicroDpmModeEnabledWithArg:)
    public func setQRMicroDpmModeEnabled(arg: Bool) {
        barkoderView.config?.decoderConfig?.qrMicro.dpmMode = arg ? 1 : 0
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
    public func setARBarcodeThumbnailOnResultEnabled(arg: Bool) {
        barkoderView.config?.arConfig.barcodeThumbnailOnResult = arg
    }
    
    @objc
    public func setARImageResultEnabled(arg: Bool) {
        barkoderView.config?.arConfig.imageResultEnabled = arg
    }
    
    @objc
    public func getCurrentZoomFactor() -> Float {
        return barkoderView.getCurrentZoomFactor()
    }
    
    @objc
    public var barcodeThumbnailOnResultEnabled: Bool {
        return barkoderView.config?.barcodeThumbnailOnResult ?? false
    }
	
	@objc 
	public var isARBarcodeThumbnailOnResultEnabled: Bool {
        return barkoderView.config?.arConfig.barcodeThumbnailOnResult ?? false
	}
    
    @objc
    public var isARImageResultEnabled: Bool {
        return barkoderView.config?.arConfig.imageResultEnabled ?? false
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
    public var idDocumentMasterChecksumEnabled: Bool {
        return (barkoderView.config?.decoderConfig?.idDocument.masterChecksum == StandardChecksum(1))
    }
    
    @objc
    public var qrDpmModeEnabled: Bool {
        return (barkoderView.config?.decoderConfig?.qr.dpmMode == 1)
    }
    
    @objc
    public var qrMicroDpmModeEnabled: Bool {
        return (barkoderView.config?.decoderConfig?.qrMicro.dpmMode == 1)
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
        case OCRText:
            return decoderConfig.ocrText.enabled
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
        case IDDocument:
            return decoderConfig.idDocument.enabled
        case Databar14:
            return decoderConfig.databar14.enabled
        case DatabarLimited:
            return decoderConfig.databarLimited.enabled
        case DatabarExpanded:
            return decoderConfig.databarExpanded.enabled
        case PostalIMB:
            return decoderConfig.postalIMB.enabled
        case Postnet:
            return decoderConfig.postnet.enabled
        case Planet:
            return decoderConfig.planet.enabled
        case AustralianPost:
            return decoderConfig.australianPost.enabled
        case RoyalMail:
            return decoderConfig.royalMail.enabled
        case KIX:
            return decoderConfig.kix.enabled
        case JapanesePost:
            return decoderConfig.japanesePost.enabled
        case MaxiCode:
            return decoderConfig.maxiCode.enabled
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
        case OCRText:
            decoderConfig.ocrText.enabled = enabled
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
        case IDDocument:
            decoderConfig.idDocument.enabled = enabled
        case Databar14:
            decoderConfig.databar14.enabled = enabled
        case DatabarLimited:
            decoderConfig.databarLimited.enabled = enabled
        case DatabarExpanded:
            decoderConfig.databarExpanded.enabled = enabled
        case PostalIMB:
            decoderConfig.postalIMB.enabled = enabled
        case Postnet:
            decoderConfig.postnet.enabled = enabled
        case Planet:
            decoderConfig.planet.enabled = enabled
        case AustralianPost:
            decoderConfig.australianPost.enabled = enabled
        case RoyalMail:
            decoderConfig.royalMail.enabled = enabled
        case KIX:
            decoderConfig.kix.enabled = enabled
        case JapanesePost:
            decoderConfig.japanesePost.enabled = enabled
        case MaxiCode:
            decoderConfig.maxiCode.enabled = enabled
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
            
            if let colorHexCode = barkoderConfigAsDictionary?["scanningIndicatorColor"] as? String {
                barkoderConfigAsDictionary?["scanningIndicatorColor"] = Util.parseColor(hexColor: colorHexCode)
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
    public var ARContinueScanningOnLimit: Bool = false {
        didSet {
            modelDidChange?()
        }
    }
    public var AREmitResultsAtSessionEndOnly: Bool = false {
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
    public var scanningIndicatorAlwaysVisible: Bool = false {
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
    
    @objc public var scanningIndicatorColor: UIColor = BarkoderConfigDefaults.scanningIndicatorColor {
        didSet {
            modelDidChange?()
        }
    }

    @objc public var roiLineWidth: Float = BarkoderConfigDefaults.roiLineWidth {
        didSet {
            modelDidChange?()
        }
    }
    
    @objc public var scanningIndicatorWidth: Float = BarkoderConfigDefaults.scanningIndicatorWidth {
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
    
    @objc public var scanningIndicatorAnimationMode: Int = BarkoderConfigDefaults.scanningIndicatorAnimationMode {
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
    
    @objc public var enableComposite: Int = BarkoderConfigDefaults.enableComposite {
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
    public var qr: DatamatrixConfig  {
        didSet {
            qr.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var qrMicro: DatamatrixConfig  {
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
    public var ocrText: SymbologyConfig  {
        didSet {
            ocrText.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var postalIMB: SymbologyConfig  {
        didSet {
            postalIMB.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var postnet: SymbologyConfig  {
        didSet {
            postnet.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var planet: SymbologyConfig  {
        didSet {
            planet.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var australianPost: SymbologyConfig  {
        didSet {
            australianPost.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var royalMail: SymbologyConfig  {
        didSet {
            royalMail.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var kix: SymbologyConfig  {
        didSet {
            kix.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var japanesePost: SymbologyConfig  {
        didSet {
            japanesePost.setModelDidChangeCallback {
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
    
    @objc
    public var maxicode: SymbologyConfig  {
        didSet {
            maxicode.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var idDocument: SymbologyConfig  {
        didSet {
            idDocument.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var databar14: SymbologyConfig  {
        didSet {
            databar14.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var databarLimited: SymbologyConfig  {
        didSet {
            databarLimited.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    @objc
    public var databarExpanded: SymbologyConfig  {
        didSet {
            databarExpanded.setModelDidChangeCallback {
                self.modelDidChange?()
            }
        }
    }
    
    override public init() {
        aztec = SymbologyConfig()
        aztecCompact = SymbologyConfig()
        qr = DatamatrixConfig()
        qrMicro = DatamatrixConfig()
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
        ocrText = SymbologyConfig()
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
        idDocument = SymbologyConfig()
        databar14 = SymbologyConfig()
        databarLimited = SymbologyConfig()
        databarExpanded = SymbologyConfig()
        postalIMB = SymbologyConfig()
        postnet = SymbologyConfig()
        planet = SymbologyConfig()
        australianPost = SymbologyConfig()
        royalMail = SymbologyConfig()
        kix = SymbologyConfig()
        japanesePost = SymbologyConfig()
        maxicode = SymbologyConfig()
        
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
    public var thumbnails: [UIImage] = []
    
    @objc public var locations: [[NSValue]] = []
    
    @objc
    public var imageInBase64: String = ""
    
    @objc
    public var mainImageInBase64: String = ""
    
    @objc public var binaryDataAsBase64: [String] = []
    
    @objc
    public var documentImageInBase64: String = ""
    
    @objc
    public var signatureImageInBase64: String = ""
    
    @objc
    public var pictureImageInBase64: String = ""
}

@objc(BKDecoderResult)
public extension DecoderResult {}

extension BarkoderProxy: BarkoderResultDelegate {
    
    public func scanningFinished(_ decoderResults: [DecoderResult], thumbnails: [UIImage]?, image: UIImage?) {
        let decoderPayload = DecoderPayload()
        decoderPayload.results = decoderResults
        decoderPayload.thumbnails = thumbnails ?? []
            
        if let imageData = image?.jpegData(compressionQuality: 1.0) {
            let base64String = imageData.base64EncodedString()
            decoderPayload.imageInBase64 = base64String
        }
       
        decoderPayload.binaryDataAsBase64 = []
           for decoderResult in decoderResults {
               decoderPayload.binaryDataAsBase64.append(
                   decoderResult.binaryData?.base64EncodedString() ?? ""
               )
           }
        
        for decoderResult in decoderResults {
                   let points = Array(UnsafeBufferPointer(start: decoderResult.getLocationPoints(), count: 4))

                   let nsValues = points.map { NSValue(cgPoint: $0) }
                   decoderPayload.locations.append(nsValues)
               }
        
        
        // Safely check for at least one result
        if let firstResult = decoderPayload.results.first, let images = firstResult.images {
            for image in images {
                switch image.name {
                case "main":
                    if let imageData = image.image.pngData() {
                        decoderPayload.mainImageInBase64 = imageData.base64EncodedString()
                    }
                case "document":
                    if let imageData = image.image.pngData() {
                        decoderPayload.documentImageInBase64 = imageData.base64EncodedString()
                    }
                case "signature":
                    if let imageData = image.image.pngData() {
                        decoderPayload.signatureImageInBase64 = imageData.base64EncodedString()
                    }
                case "picture":
                    if let imageData = image.image.pngData() {
                        decoderPayload.pictureImageInBase64 = imageData.base64EncodedString()
                    }
                default:
                    break
                }
            }
        }
        
        DispatchQueue.main.async {
            self.resultCallbackDecoderResult?(decoderPayload)
        }
    }
}

class BarkoderConfigDefaults {
    
    static let closeSessionOnResult = true
    static let ARContinueScanningOnLimit = false
    static let AREmitResultsAtSessionEndOnly = false
    static let imageResultEnabled = false
    static let locationInImageResultEnabled = false
    static let locationInPreviewEnabled = true
    static let decoderThreads = 2
    static let regionOfInterest: CGRect = CGRect(x: 3, y: 30, width: 94, height: 40)
    static let pinchToZoomEnabled: Bool = false
    static let beepOnSuccessEnabled: Bool = true
    static let scanningIndicatorAlwaysVisible: Bool = false
    static let vibrateOnSuccessEnabled: Bool = true
    static let regionOfInterestVisible: Bool = true
    static let barcodeThumbnailOnResult: Bool = true
    static let enableVINRestrictions: Bool = false
    static let thresholdBetweenDuplicatesScans: Int = 5
    static let scanningIndicatorAnimationMode: Int = 0
    static let enableComposite: Int = 0
    static let enableMisshaped1D: Bool = false
    static let upcEanDeblur: Bool = false
    static let locationLineColor: UIColor = #colorLiteral(red: 0.8, green: 0.003921568627, blue: 0.09803921569, alpha: 1)
    static let locationLineWidth: Float = 2
    static let roiLineColor: UIColor = #colorLiteral(red: 0.8, green: 0.003921568627, blue: 0.09803921569, alpha: 1)
    static let scanningIndicatorColor: UIColor = #colorLiteral(red: 0.8, green: 0.003921568627, blue: 0.09803921569, alpha: 1)
    static let roiLineWidth: Float = 2
    static let scanningIndicatorWidth: Float = 2
    static let roiOverlayBackgroundColor: UIColor = #colorLiteral(red: 0, green: 0, blue: 0, alpha: 0.3950227649)
	static let barcodeThumbnailOnResultEnabled: Bool = true
	static let multicodeCachingEnabled: Bool = false
	static let multicodeCachingDuration: Int = 3000
}
