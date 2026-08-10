using System.Text.Json;
using Barkoder.Windows.Config;
using Barkoder.Windows.Decoder;
using Barkoder.Windows.Models;

namespace Barkoder.Windows.Utils
{
    /// <summary>
    /// Static utility methods for the Barkoder SDK.
    /// Mirrors BarkoderHelper from the Android/iOS SDKs.
    /// </summary>
    public static class BarkoderHelper
    {
        /// <summary>
        /// Scan barcodes from an image file (PNG, JPEG, BMP, etc.).
        /// Decodes raw BGRA8 pixel data.
        /// </summary>
        /// <param name="bgra8Data">Raw BGRA8 pixel bytes of the image.</param>
        /// <param name="width">Image width in pixels.</param>
        /// <param name="height">Image height in pixels.</param>
        /// <param name="config">Barkoder configuration with enabled decoders.</param>
        /// <param name="resultCallback">Callback to receive results.</param>
        public static void ScanImage(byte[] bgra8Data, int width, int height, BarkoderConfig config, BarkoderResultCallback resultCallback)
        {
            if (bgra8Data == null) throw new ArgumentNullException(nameof(bgra8Data));
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (resultCallback == null) throw new ArgumentNullException(nameof(resultCallback));

            using var decoder = new BarcodeDecoder();
            decoder.Initialize(config.LicenseKey);
            decoder.ApplyConfig(config.DecoderConfig, config);

            var results = decoder.Decode(bgra8Data, width, height);
            resultCallback.Invoke(results ?? Array.Empty<BarcodeResult>(), null, null);
        }

        /// <summary>
        /// Apply configuration from a predefined template.
        /// </summary>
        public static void ApplyConfigSettingsFromTemplate(BarkoderConfig config, BarkoderConfigTemplate template)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            switch (template)
            {
                case BarkoderConfigTemplate.All:
                    config.DecoderConfig.SetEnabledDecoders(
                        DecoderType.QR, DecoderType.QRMicro, DecoderType.Code128, DecoderType.Code93,
                        DecoderType.Code39, DecoderType.Codabar, DecoderType.Code11, DecoderType.Msi,
                        DecoderType.EAN13, DecoderType.EAN8, DecoderType.UPCa, DecoderType.UPCe,
                        DecoderType.PDF417, DecoderType.PDF417Micro, DecoderType.DataMatrix,
                        DecoderType.Aztec, DecoderType.AztecCompact, DecoderType.Interleaved2of5,
                        DecoderType.ITF14, DecoderType.DotCode, DecoderType.Databar14,
                        DecoderType.DatabarExpanded, DecoderType.MaxiCode);
                    break;

                case BarkoderConfigTemplate.QR:
                    config.DecoderConfig.SetEnabledDecoders(DecoderType.QR, DecoderType.QRMicro);
                    break;

                case BarkoderConfigTemplate.Retail1D:
                    config.DecoderConfig.SetEnabledDecoders(
                        DecoderType.EAN13, DecoderType.EAN8, DecoderType.UPCa, DecoderType.UPCe);
                    break;

                case BarkoderConfigTemplate.Industrial1D:
                    config.DecoderConfig.SetEnabledDecoders(
                        DecoderType.Code128, DecoderType.Code93, DecoderType.Code39, DecoderType.Code11,
                        DecoderType.Codabar, DecoderType.Interleaved2of5, DecoderType.ITF14);
                    break;

                case BarkoderConfigTemplate.All2D:
                    config.DecoderConfig.SetEnabledDecoders(
                        DecoderType.QR, DecoderType.PDF417, DecoderType.DataMatrix, DecoderType.Aztec,
                        DecoderType.QRMicro, DecoderType.PDF417Micro);
                    break;

                case BarkoderConfigTemplate.PDF417:
                    config.DecoderConfig.SetEnabledDecoders(DecoderType.PDF417, DecoderType.PDF417Micro);
                    break;

                case BarkoderConfigTemplate.DotCode:
                    config.DecoderConfig.SetEnabledDecoders(DecoderType.DotCode);
                    break;
            }
        }

        /// <summary>
        /// Apply configuration from a JSON string.
        /// </summary>
        public static void ApplyConfigSettingsFromJson(BarkoderConfig config, string jsonString)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (string.IsNullOrWhiteSpace(jsonString)) throw new ArgumentException("JSON string cannot be empty.", nameof(jsonString));

            var json = JsonDocument.Parse(jsonString);
            var root = json.RootElement;

            if (root.TryGetProperty("closeSessionOnResultEnabled", out var closeSession))
                config.CloseSessionOnResultEnabled = closeSession.GetBoolean();

            if (root.TryGetProperty("beepOnSuccessEnabled", out var beep))
                config.BeepOnSuccessEnabled = beep.GetBoolean();

            if (root.TryGetProperty("vibrateOnSuccessEnabled", out var vibrate))
                config.VibrateOnSuccessEnabled = vibrate.GetBoolean();

            if (root.TryGetProperty("pinchToZoomEnabled", out var pinch))
                config.PinchToZoomEnabled = pinch.GetBoolean();

            if (root.TryGetProperty("barkoderResolution", out var resolution))
                config.BarkoderResolution = (BarkoderResolution)resolution.GetInt32();

            if (root.TryGetProperty("imageResultEnabled", out var imageResult))
                config.ImageResultEnabled = imageResult.GetBoolean();

            if (root.TryGetProperty("locationInPreviewEnabled", out var locPreview))
                config.LocationInPreviewEnabled = locPreview.GetBoolean();

            if (root.TryGetProperty("regionOfInterest", out var roi))
            {
                var left = roi.GetProperty("left").GetSingle();
                var top = roi.GetProperty("top").GetSingle();
                var width = roi.GetProperty("width").GetSingle();
                var height = roi.GetProperty("height").GetSingle();
                config.SetRegionOfInterest(left, top, width, height);
            }

            if (root.TryGetProperty("enabledDecoders", out var decoders) && decoders.ValueKind == JsonValueKind.Array)
            {
                var types = new List<DecoderType>();
                foreach (var d in decoders.EnumerateArray())
                {
                    if (Enum.TryParse<DecoderType>(d.GetString(), true, out var dt))
                        types.Add(dt);
                }
                config.DecoderConfig.SetEnabledDecoders(types.ToArray());
            }

            if (root.TryGetProperty("formattingType", out var formattingType))
            {
                if (formattingType.ValueKind == JsonValueKind.String)
                {
                    config.DecoderConfig.Formatting = FormattingTypeParser.FromString(formattingType.GetString());
                }
                else if (formattingType.ValueKind == JsonValueKind.Number && formattingType.TryGetInt32(out var formattingTypeInt))
                {
                    config.DecoderConfig.Formatting = Enum.IsDefined(typeof(FormattingType), formattingTypeInt)
                        ? (FormattingType)formattingTypeInt
                        : FormattingType.Automatic;
                }
            }

            if (root.TryGetProperty("roiCenterMark", out var roiCenterMark))
            {
                if (roiCenterMark.ValueKind == JsonValueKind.String)
                {
                    config.DecoderConfig.RoiCenterMark = BarkoderRoiCenterMarkParser.FromString(roiCenterMark.GetString());
                }
                else if (roiCenterMark.ValueKind == JsonValueKind.Number && roiCenterMark.TryGetInt32(out var roiCenterMarkInt))
                {
                    config.DecoderConfig.RoiCenterMark = Enum.IsDefined(typeof(BarkoderRoiCenterMark), roiCenterMarkInt)
                        ? (BarkoderRoiCenterMark)roiCenterMarkInt
                        : BarkoderRoiCenterMark.None;
                }
            }

            if (root.TryGetProperty("threadsLimit", out var threads))
                config.ThreadsLimit = threads.GetInt32();

            if (root.TryGetProperty("powerSavingMode", out var psm))
                config.PowerSavingMode = psm.GetInt32();
        }

        /// <summary>
        /// Apply configuration from a JSON file.
        /// </summary>
        public static async Task ApplyConfigSettingsFromFile(BarkoderConfig config, string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Config file not found.", filePath);

            var json = await File.ReadAllTextAsync(filePath);
            ApplyConfigSettingsFromJson(config, json);
        }
    }

    /// <summary>
    /// Predefined configuration templates matching iOS/Android SDKs.
    /// </summary>
    public enum BarkoderConfigTemplate
    {
        All,
        QR,
        Retail1D,
        Industrial1D,
        All2D,
        PDF417,
        DotCode
    }
}
