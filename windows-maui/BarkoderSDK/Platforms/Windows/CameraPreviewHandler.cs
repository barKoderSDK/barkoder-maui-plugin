using System.Runtime.InteropServices.WindowsRuntime;
using Barkoder.Windows.Models;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Media.Imaging;
using WinImage = Microsoft.UI.Xaml.Controls.Image;

namespace Barkoder.Windows.UI
{
    /// <summary>
    /// MAUI handler that maps CameraPreview to a WinUI3 Image,
    /// rendering BGRA8 frames via a WriteableBitmap with ROI, location, and AR overlays.
    /// </summary>
    public class CameraPreviewHandler : ViewHandler<CameraPreview, WinImage>
    {
        private WriteableBitmap? _bitmap;
        private int _bitmapWidth;
        private int _bitmapHeight;

        /// <summary>
        /// Property mapper for CameraPreview.
        /// </summary>
        public static IPropertyMapper<CameraPreview, CameraPreviewHandler> Mapper =
            new PropertyMapper<CameraPreview, CameraPreviewHandler>(ViewMapper);

        /// <summary>
        /// Command mapper for CameraPreview.
        /// </summary>
        public static CommandMapper<CameraPreview, CameraPreviewHandler> Commands =
            new(ViewCommandMapper)
            {
                [nameof(CameraPreview.UpdateFrame)] = MapUpdateFrame,
            };

        /// <summary>
        /// Creates a new CameraPreviewHandler.
        /// </summary>
        public CameraPreviewHandler() : base(Mapper, Commands) { }

        /// <inheritdoc />
        protected override WinImage CreatePlatformView()
        {
            return new WinImage
            {
                Stretch = Microsoft.UI.Xaml.Media.Stretch.UniformToFill
            };
        }

        private static void MapUpdateFrame(CameraPreviewHandler handler, CameraPreview view, object? args)
        {
            if (args is not FrameData frame) return;

            // (Re)create WriteableBitmap if the resolution changed
            if (handler._bitmap == null ||
                handler._bitmapWidth != frame.Width ||
                handler._bitmapHeight != frame.Height)
            {
                handler._bitmapWidth = frame.Width;
                handler._bitmapHeight = frame.Height;
                handler._bitmap = new WriteableBitmap(frame.Width, frame.Height);
                handler.PlatformView.Source = handler._bitmap;
            }

            // Work on a copy so we can draw overlays without modifying the original
            var pixels = frame.Pixels;
            var overlay = frame.Overlay;

            if (overlay != null)
            {
                pixels = new byte[frame.Pixels.Length];
                Buffer.BlockCopy(frame.Pixels, 0, pixels, 0, pixels.Length);

                int w = frame.Width, h = frame.Height;

                // Draw ROI overlay
                if (overlay.RoiVisible && overlay.Roi.HasValue)
                {
                    var roi = overlay.Roi.Value;
                    int roiX = (int)(roi.Left / 100f * w);
                    int roiY = (int)(roi.Top / 100f * h);
                    int roiW = (int)(roi.Width / 100f * w);
                    int roiH = (int)(roi.Height / 100f * h);

                    // Dim area outside ROI
                    DimOutsideRect(pixels, w, h, roiX, roiY, roiW, roiH, overlay.RoiDimColor);

                    // Draw ROI border
                    DrawRect(pixels, w, h, roiX, roiY, roiW, roiH,
                        overlay.RoiColor, overlay.RoiLineWidth);
                }

                // Draw ROI center mark (visual indicator for center-only scan mode).
                DrawRoiCenterMark(pixels, w, h, overlay);

                // Draw barcode location polygons (non-AR mode)
                if (overlay.LocationEnabled && overlay.Locations != null && !overlay.AREnabled)
                {
                    foreach (var loc in overlay.Locations)
                    {
                        DrawLine(pixels, w, h, loc.TopLeft, loc.TopRight, overlay.LocationColor, overlay.LocationLineWidth);
                        DrawLine(pixels, w, h, loc.TopRight, loc.BottomRight, overlay.LocationColor, overlay.LocationLineWidth);
                        DrawLine(pixels, w, h, loc.BottomRight, loc.BottomLeft, overlay.LocationColor, overlay.LocationLineWidth);
                        DrawLine(pixels, w, h, loc.BottomLeft, loc.TopLeft, overlay.LocationColor, overlay.LocationLineWidth);
                    }
                }

                // Draw AR overlays
                if (overlay.AREnabled && overlay.AREntries != null && overlay.ARConfig != null)
                {
                    // Frozen state: draw semi-transparent dark shade over the frame
                    if (overlay.IsFrozen)
                    {
                        var shadeColor = ((byte)0, (byte)0, (byte)0, (byte)128); // 50% black
                        FillRect(pixels, w, h, 0, 0, w, h, shadeColor);
                    }

                    DrawAROverlays(pixels, w, h, overlay.AREntries, overlay.ARConfig);
                }
            }

            // Copy pixels into the WriteableBitmap's buffer
            using var stream = handler._bitmap.PixelBuffer.AsStream();
            stream.Position = 0;
            stream.Write(pixels, 0, pixels.Length);
            handler._bitmap.Invalidate();
        }

        // --- AR Overlay Drawing ---

        private static void DrawAROverlays(byte[] pixels, int w, int h,
            AROverlayEntry[] entries, AROverlayConfig config)
        {
            foreach (var entry in entries)
            {
                var color = entry.IsSelected ? config.SelectedColor : config.NonSelectedColor;
                var lineWidth = entry.IsSelected ? config.SelectedLineWidth : config.NonSelectedLineWidth;

                // Apply alpha fade
                if (entry.Alpha < 1.0f)
                {
                    color = (color.B, color.G, color.R, (byte)(color.A * entry.Alpha));
                }

                if (config.LocationType == BarkoderARLocationType.None)
                    continue;

                if (config.LocationType == BarkoderARLocationType.Tight && entry.HasPolygon &&
                    entry.P0 != null && entry.P1 != null && entry.P2 != null && entry.P3 != null)
                {
                    // Draw tight polygon
                    DrawLine(pixels, w, h, entry.P0, entry.P1, color, lineWidth);
                    DrawLine(pixels, w, h, entry.P1, entry.P2, color, lineWidth);
                    DrawLine(pixels, w, h, entry.P2, entry.P3, color, lineWidth);
                    DrawLine(pixels, w, h, entry.P3, entry.P0, color, lineWidth);

                    // Fill with semi-transparent color (13 = ~5% of 255)
                    var fillColor = (color.B, color.G, color.R, (byte)13);
                    FillQuad(pixels, w, h, entry.P0, entry.P1, entry.P2, entry.P3, fillColor);
                }
                else
                {
                    // Draw bounding box
                    int rx = (int)entry.Left;
                    int ry = (int)entry.Top;
                    int rw = (int)(entry.Right - entry.Left);
                    int rh = (int)(entry.Bottom - entry.Top);

                    if (rw <= 0 || rh <= 0) continue;

                    DrawRect(pixels, w, h, rx, ry, rw, rh, color, lineWidth);

                    // Semi-transparent fill
                    var fillColor = (color.B, color.G, color.R, (byte)13);
                    FillRect(pixels, w, h, rx, ry, rw, rh, fillColor);
                }

                // Draw header along the top edge of the barcode
                bool showHeader = config.HeaderShowMode == BarkoderARHeaderShowMode.Always ||
                    (config.HeaderShowMode == BarkoderARHeaderShowMode.OnSelected && entry.IsSelected);

                if (showHeader && !string.IsNullOrEmpty(entry.HeaderText))
                {
                    DrawARHeader(pixels, w, h, entry, config, color);
                }
            }
        }

        /// <summary>
        /// Draw the AR header bar along the top edge of the barcode location.
        /// The header follows the barcode orientation.
        /// </summary>
        private static void DrawARHeader(byte[] pixels, int w, int h,
            AROverlayEntry entry, AROverlayConfig config,
            (byte B, byte G, byte R, byte A) overlayColor)
        {
            var textColor = entry.IsSelected ? config.HeaderTextColorSelected : config.HeaderTextColorNonSelected;
            int headerH = (int)config.HeaderHeight;

            // Find the top edge based on location type
            Models.PointF topLeft, topRight;

            if (config.LocationType == BarkoderARLocationType.Tight &&
                entry.HasPolygon && entry.P0 != null && entry.P1 != null && entry.P2 != null && entry.P3 != null)
            {
                // Tight mode: find the top edge from the 4 polygon edges
                var edges = new[] {
                    (entry.P0, entry.P1), (entry.P1, entry.P2),
                    (entry.P2, entry.P3), (entry.P3, entry.P0)
                };

                // Top edge = edge with smallest average Y
                int bestIdx = 0;
                float bestAvgY = float.MaxValue;
                for (int i = 0; i < edges.Length; i++)
                {
                    float avgY = (edges[i].Item1.Y + edges[i].Item2.Y) / 2f;
                    if (avgY < bestAvgY)
                    {
                        bestAvgY = avgY;
                        bestIdx = i;
                    }
                }

                // Ensure left-to-right ordering
                var (ea, eb) = edges[bestIdx];
                if (ea.X <= eb.X) { topLeft = ea; topRight = eb; }
                else { topLeft = eb; topRight = ea; }
            }
            else
            {
                // BoundingBox mode: top edge is horizontal
                topLeft = new Models.PointF(entry.Left, entry.Top);
                topRight = new Models.PointF(entry.Right, entry.Top);
            }

            // Compute the direction vector of the top edge
            float dx = topRight.X - topLeft.X;
            float dy = topRight.Y - topLeft.Y;
            float edgeLen = MathF.Sqrt(dx * dx + dy * dy);
            if (edgeLen < 1f) return;

            // Normal vector pointing "up" (perpendicular, away from barcode center)
            float nx = -dy / edgeLen;
            float ny = dx / edgeLen;

            // Ensure normal points upward (toward smaller Y)
            float centerY = (entry.Top + entry.Bottom) / 2f;
            float midY = (topLeft.Y + topRight.Y) / 2f;
            if (midY + ny > centerY) { nx = -nx; ny = -ny; }

            // Header quad corners: extend from top edge outward by headerH
            var h0 = new Models.PointF(topLeft.X, topLeft.Y);
            var h1 = new Models.PointF(topRight.X, topRight.Y);
            var h2 = new Models.PointF(topRight.X + nx * headerH, topRight.Y + ny * headerH);
            var h3 = new Models.PointF(topLeft.X + nx * headerH, topLeft.Y + ny * headerH);

            // Draw filled header background
            FillQuad(pixels, w, h, h0, h1, h2, h3, overlayColor);

            // Draw text rotated along the top edge, centered in the header
            // Direction along the edge (unit vector)
            float dirX = dx / edgeLen;
            float dirY = dy / edgeLen;

            // Scale: fit text height within headerH
            int scale = Math.Max(1, Math.Min(headerH / 7, 3));
            int charW = 5 * scale + scale; // glyph width + spacing
            int charH = 7 * scale;

            // Max chars that fit along the edge
            int maxChars = (int)(edgeLen / Math.Max(charW, 1));
            if (maxChars <= 0) return;

            string displayText = entry.HeaderText.Length > maxChars
                ? (maxChars > 3 ? entry.HeaderText[..(maxChars - 3)] + "..." : entry.HeaderText[..maxChars])
                : entry.HeaderText;

            int totalTextW = displayText.Length * charW - scale; // subtract trailing spacing

            // Start position: center of the header midline, offset back by half the text width
            // Midline of header = top edge midpoint offset by half headerH along the normal
            float midBaseX = topLeft.X + dirX * edgeLen / 2f + nx * headerH / 2f;
            float midBaseY = topLeft.Y + dirY * edgeLen / 2f + ny * headerH / 2f;

            // Move to start of text (back along edge by half text width, back along normal by half char height)
            float startX = midBaseX - dirX * totalTextW / 2f - nx * charH / 2f;
            float startY = midBaseY - dirY * totalTextW / 2f - ny * charH / 2f;

            // Draw each character rotated along the edge
            float curX = startX;
            float curY = startY;
            foreach (char ch in displayText)
            {
                DrawCharRotated(pixels, w, h, curX, curY, ch, textColor, scale, dirX, dirY, nx, ny);
                curX += dirX * charW;
                curY += dirY * charW;
            }
        }

        /// <summary>
        /// Render text using a simple built-in 5x7 pixel font.
        /// </summary>
        private static void DrawSimpleText(byte[] pixels, int w, int h,
            int x, int y, int maxWidth, int maxHeight,
            string text, (byte B, byte G, byte R, byte A) color)
        {
            if (maxWidth <= 0 || maxHeight <= 0) return;

            // Scale factor: try to fit text within maxHeight
            int scale = Math.Max(1, Math.Min(maxHeight / 7, 3));
            int charW = 5 * scale + scale; // width + spacing
            int maxChars = maxWidth / Math.Max(charW, 1);

            if (maxChars <= 0) return;

            string displayText = text.Length > maxChars
                ? (maxChars > 3 ? text[..(maxChars - 3)] + "..." : text[..maxChars])
                : text;

            int cx = x;
            foreach (char ch in displayText)
            {
                if (cx + 5 * scale > x + maxWidth) break;
                DrawChar(pixels, w, h, cx, y, ch, color, scale);
                cx += charW;
            }
        }

        /// <summary>
        /// Draw text centered horizontally and vertically within the given bounds.
        /// </summary>
        private static void DrawCenteredText(byte[] pixels, int w, int h,
            int x, int y, int maxWidth, int maxHeight,
            string text, (byte B, byte G, byte R, byte A) color)
        {
            if (maxWidth <= 0 || maxHeight <= 0) return;

            int scale = Math.Max(1, Math.Min(maxHeight / 7, 3));
            int charW = 5 * scale + scale;
            int charH = 7 * scale;
            int maxChars = maxWidth / Math.Max(charW, 1);

            if (maxChars <= 0) return;

            string displayText = text.Length > maxChars
                ? (maxChars > 3 ? text[..(maxChars - 3)] + "..." : text[..maxChars])
                : text;

            int totalTextW = displayText.Length * charW - scale; // subtract trailing spacing
            int startX = x + (maxWidth - totalTextW) / 2;
            int startY = y + (maxHeight - charH) / 2;

            int cx = startX;
            foreach (char ch in displayText)
            {
                if (cx + 5 * scale > x + maxWidth) break;
                DrawChar(pixels, w, h, cx, startY, ch, color, scale);
                cx += charW;
            }
        }

        /// <summary>
        /// Draw a single character from a minimal 5x7 bitmap font.
        /// </summary>
        private static void DrawChar(byte[] pixels, int w, int h,
            int x, int y, char ch, (byte B, byte G, byte R, byte A) color, int scale)
        {
            var glyph = GetGlyph(ch);
            if (glyph == null) return;

            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if ((glyph[row] & (1 << (4 - col))) != 0)
                    {
                        for (int sy = 0; sy < scale; sy++)
                            for (int sx = 0; sx < scale; sx++)
                                SetPixel(pixels, w, h, x + col * scale + sx, y + row * scale + sy, color);
                    }
                }
            }
        }

        /// <summary>
        /// Draw a single character rotated along arbitrary direction vectors.
        /// dirX/dirY = unit vector along the text baseline (edge direction).
        /// normX/normY = unit vector perpendicular (downward within the glyph).
        /// </summary>
        private static void DrawCharRotated(byte[] pixels, int w, int h,
            float originX, float originY, char ch,
            (byte B, byte G, byte R, byte A) color, int scale,
            float dirX, float dirY, float normX, float normY)
        {
            var glyph = GetGlyph(ch);
            if (glyph == null) return;

            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if ((glyph[row] & (1 << (4 - col))) != 0)
                    {
                        for (int sy = 0; sy < scale; sy++)
                        {
                            for (int sx = 0; sx < scale; sx++)
                            {
                                float localX = col * scale + sx;
                                float localY = row * scale + sy;
                                int px = (int)(originX + dirX * localX + normX * localY);
                                int py = (int)(originY + dirY * localX + normY * localY);
                                SetPixel(pixels, w, h, px, py, color);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Minimal 5x7 bitmap font glyphs for printable ASCII characters.
        /// Each byte represents one row; bits 4..0 are pixels left to right.
        /// </summary>
        private static byte[]? GetGlyph(char ch)
        {
            return ch switch
            {
                ' ' => new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                '!' => new byte[] { 0x04, 0x04, 0x04, 0x04, 0x04, 0x00, 0x04 },
                '"' => new byte[] { 0x0A, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00 },
                '#' => new byte[] { 0x0A, 0x1F, 0x0A, 0x0A, 0x1F, 0x0A, 0x00 },
                '.' => new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04 },
                ',' => new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x08 },
                ':' => new byte[] { 0x00, 0x04, 0x00, 0x00, 0x04, 0x00, 0x00 },
                ';' => new byte[] { 0x00, 0x04, 0x00, 0x00, 0x04, 0x08, 0x00 },
                '-' => new byte[] { 0x00, 0x00, 0x00, 0x1F, 0x00, 0x00, 0x00 },
                '+' => new byte[] { 0x00, 0x04, 0x04, 0x1F, 0x04, 0x04, 0x00 },
                '=' => new byte[] { 0x00, 0x00, 0x1F, 0x00, 0x1F, 0x00, 0x00 },
                '/' => new byte[] { 0x01, 0x01, 0x02, 0x04, 0x08, 0x10, 0x10 },
                '\\' => new byte[] { 0x10, 0x10, 0x08, 0x04, 0x02, 0x01, 0x01 },
                '(' => new byte[] { 0x02, 0x04, 0x08, 0x08, 0x08, 0x04, 0x02 },
                ')' => new byte[] { 0x08, 0x04, 0x02, 0x02, 0x02, 0x04, 0x08 },
                '[' => new byte[] { 0x0E, 0x08, 0x08, 0x08, 0x08, 0x08, 0x0E },
                ']' => new byte[] { 0x0E, 0x02, 0x02, 0x02, 0x02, 0x02, 0x0E },
                '_' => new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1F },
                '0' => new byte[] { 0x0E, 0x11, 0x13, 0x15, 0x19, 0x11, 0x0E },
                '1' => new byte[] { 0x04, 0x0C, 0x04, 0x04, 0x04, 0x04, 0x0E },
                '2' => new byte[] { 0x0E, 0x11, 0x01, 0x06, 0x08, 0x10, 0x1F },
                '3' => new byte[] { 0x0E, 0x11, 0x01, 0x06, 0x01, 0x11, 0x0E },
                '4' => new byte[] { 0x02, 0x06, 0x0A, 0x12, 0x1F, 0x02, 0x02 },
                '5' => new byte[] { 0x1F, 0x10, 0x1E, 0x01, 0x01, 0x11, 0x0E },
                '6' => new byte[] { 0x06, 0x08, 0x10, 0x1E, 0x11, 0x11, 0x0E },
                '7' => new byte[] { 0x1F, 0x01, 0x02, 0x04, 0x08, 0x08, 0x08 },
                '8' => new byte[] { 0x0E, 0x11, 0x11, 0x0E, 0x11, 0x11, 0x0E },
                '9' => new byte[] { 0x0E, 0x11, 0x11, 0x0F, 0x01, 0x02, 0x0C },
                'A' or 'a' => new byte[] { 0x0E, 0x11, 0x11, 0x1F, 0x11, 0x11, 0x11 },
                'B' or 'b' => new byte[] { 0x1E, 0x11, 0x11, 0x1E, 0x11, 0x11, 0x1E },
                'C' or 'c' => new byte[] { 0x0E, 0x11, 0x10, 0x10, 0x10, 0x11, 0x0E },
                'D' or 'd' => new byte[] { 0x1E, 0x11, 0x11, 0x11, 0x11, 0x11, 0x1E },
                'E' or 'e' => new byte[] { 0x1F, 0x10, 0x10, 0x1E, 0x10, 0x10, 0x1F },
                'F' or 'f' => new byte[] { 0x1F, 0x10, 0x10, 0x1E, 0x10, 0x10, 0x10 },
                'G' or 'g' => new byte[] { 0x0E, 0x11, 0x10, 0x17, 0x11, 0x11, 0x0F },
                'H' or 'h' => new byte[] { 0x11, 0x11, 0x11, 0x1F, 0x11, 0x11, 0x11 },
                'I' or 'i' => new byte[] { 0x0E, 0x04, 0x04, 0x04, 0x04, 0x04, 0x0E },
                'J' or 'j' => new byte[] { 0x07, 0x02, 0x02, 0x02, 0x02, 0x12, 0x0C },
                'K' or 'k' => new byte[] { 0x11, 0x12, 0x14, 0x18, 0x14, 0x12, 0x11 },
                'L' or 'l' => new byte[] { 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x1F },
                'M' or 'm' => new byte[] { 0x11, 0x1B, 0x15, 0x15, 0x11, 0x11, 0x11 },
                'N' or 'n' => new byte[] { 0x11, 0x19, 0x15, 0x13, 0x11, 0x11, 0x11 },
                'O' or 'o' => new byte[] { 0x0E, 0x11, 0x11, 0x11, 0x11, 0x11, 0x0E },
                'P' or 'p' => new byte[] { 0x1E, 0x11, 0x11, 0x1E, 0x10, 0x10, 0x10 },
                'Q' or 'q' => new byte[] { 0x0E, 0x11, 0x11, 0x11, 0x15, 0x12, 0x0D },
                'R' or 'r' => new byte[] { 0x1E, 0x11, 0x11, 0x1E, 0x14, 0x12, 0x11 },
                'S' or 's' => new byte[] { 0x0E, 0x11, 0x10, 0x0E, 0x01, 0x11, 0x0E },
                'T' or 't' => new byte[] { 0x1F, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04 },
                'U' or 'u' => new byte[] { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x0E },
                'V' or 'v' => new byte[] { 0x11, 0x11, 0x11, 0x11, 0x0A, 0x0A, 0x04 },
                'W' or 'w' => new byte[] { 0x11, 0x11, 0x11, 0x15, 0x15, 0x1B, 0x11 },
                'X' or 'x' => new byte[] { 0x11, 0x11, 0x0A, 0x04, 0x0A, 0x11, 0x11 },
                'Y' or 'y' => new byte[] { 0x11, 0x11, 0x0A, 0x04, 0x04, 0x04, 0x04 },
                'Z' or 'z' => new byte[] { 0x1F, 0x01, 0x02, 0x04, 0x08, 0x10, 0x1F },
                _ => new byte[] { 0x1F, 0x11, 0x11, 0x11, 0x11, 0x11, 0x1F } // fallback: box
            };
        }

        // --- Pixel drawing helpers (BGRA byte array) ---

        private static void SetPixel(byte[] pixels, int w, int h, int x, int y,
            (byte B, byte G, byte R, byte A) color)
        {
            if (x < 0 || x >= w || y < 0 || y >= h) return;
            int i = (y * w + x) * 4;
            // Alpha blend
            float a = color.A / 255f;
            pixels[i] = (byte)(pixels[i] * (1 - a) + color.B * a);
            pixels[i + 1] = (byte)(pixels[i + 1] * (1 - a) + color.G * a);
            pixels[i + 2] = (byte)(pixels[i + 2] * (1 - a) + color.R * a);
            pixels[i + 3] = 255;
        }

        private static void DimOutsideRect(byte[] pixels, int w, int h,
            int rx, int ry, int rw, int rh, (byte B, byte G, byte R, byte A) dimColor)
        {
            int rx2 = Math.Min(rx + rw, w);
            int ry2 = Math.Min(ry + rh, h);
            rx = Math.Max(rx, 0);
            ry = Math.Max(ry, 0);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (x >= rx && x < rx2 && y >= ry && y < ry2)
                        continue; // Inside ROI — skip
                    SetPixel(pixels, w, h, x, y, dimColor);
                }
            }
        }

        private static void DrawRect(byte[] pixels, int w, int h,
            int rx, int ry, int rw, int rh,
            (byte B, byte G, byte R, byte A) color, int thickness)
        {
            for (int t = 0; t < thickness; t++)
            {
                // Top edge
                for (int x = rx - t; x < rx + rw + t; x++)
                    SetPixel(pixels, w, h, x, ry - t, color);
                // Bottom edge
                for (int x = rx - t; x < rx + rw + t; x++)
                    SetPixel(pixels, w, h, x, ry + rh - 1 + t, color);
                // Left edge
                for (int y = ry - t; y < ry + rh + t; y++)
                    SetPixel(pixels, w, h, rx - t, y, color);
                // Right edge
                for (int y = ry - t; y < ry + rh + t; y++)
                    SetPixel(pixels, w, h, rx + rw - 1 + t, y, color);
            }
        }

        private static void FillRect(byte[] pixels, int w, int h,
            int rx, int ry, int rw, int rh,
            (byte B, byte G, byte R, byte A) color)
        {
            int x0 = Math.Max(0, rx);
            int y0 = Math.Max(0, ry);
            int x1 = Math.Min(w, rx + rw);
            int y1 = Math.Min(h, ry + rh);

            for (int y = y0; y < y1; y++)
                for (int x = x0; x < x1; x++)
                    SetPixel(pixels, w, h, x, y, color);
        }

        private static void FillQuad(byte[] pixels, int w, int h,
            Models.PointF p0, Models.PointF p1, Models.PointF p2, Models.PointF p3,
            (byte B, byte G, byte R, byte A) color)
        {
            // Simple scanline fill for a convex quad
            float minY = Math.Min(Math.Min(p0.Y, p1.Y), Math.Min(p2.Y, p3.Y));
            float maxY = Math.Max(Math.Max(p0.Y, p1.Y), Math.Max(p2.Y, p3.Y));
            int iy0 = Math.Max(0, (int)minY);
            int iy1 = Math.Min(h - 1, (int)maxY);

            var points = new[] { p0, p1, p2, p3 };
            var edges = new[] { (p0, p1), (p1, p2), (p2, p3), (p3, p0) };

            for (int y = iy0; y <= iy1; y++)
            {
                float xMin = w, xMax = 0;
                foreach (var (a, b) in edges)
                {
                    if ((a.Y <= y && b.Y > y) || (b.Y <= y && a.Y > y))
                    {
                        float t = (y - a.Y) / (b.Y - a.Y);
                        float x = a.X + t * (b.X - a.X);
                        xMin = Math.Min(xMin, x);
                        xMax = Math.Max(xMax, x);
                    }
                }
                int ix0 = Math.Max(0, (int)xMin);
                int ix1 = Math.Min(w - 1, (int)xMax);
                for (int x = ix0; x <= ix1; x++)
                    SetPixel(pixels, w, h, x, y, color);
            }
        }

        private static void DrawLine(byte[] pixels, int w, int h,
            Models.PointF p1, Models.PointF p2,
            (byte B, byte G, byte R, byte A) color, int thickness)
        {
            // Bresenham line with thickness
            int x0 = (int)p1.X, y0 = (int)p1.Y;
            int x1 = (int)p2.X, y1 = (int)p2.Y;
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;

            while (true)
            {
                // Draw thick point
                int half = thickness / 2;
                for (int ty = -half; ty <= half; ty++)
                    for (int tx = -half; tx <= half; tx++)
                        SetPixel(pixels, w, h, x0 + tx, y0 + ty, color);

                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }
        }

        private static void DrawRoiCenterMark(byte[] pixels, int w, int h, OverlayData overlay)
        {
            if (overlay.RoiCenterMark == BarkoderRoiCenterMark.None)
                return;

            var roi = overlay.Roi ?? (0f, 0f, 100f, 100f);
            int roiX = (int)(roi.Left / 100f * w);
            int roiY = (int)(roi.Top / 100f * h);
            int roiW = (int)(roi.Width / 100f * w);
            int roiH = (int)(roi.Height / 100f * h);

            int cx = roiX + roiW / 2;
            int cy = roiY + roiH / 2;

            // Two-tone drawing keeps the marker visible on both dark and bright backgrounds.
            var outline = ((byte)0, (byte)0, (byte)0, (byte)220);
            var fill = ((byte)255, (byte)255, (byte)255, (byte)220);

            if (overlay.RoiCenterMark == BarkoderRoiCenterMark.Point)
            {
                FillRect(pixels, w, h, cx - 4, cy - 4, 9, 9, outline);
                FillRect(pixels, w, h, cx - 2, cy - 2, 5, 5, fill);
                return;
            }

            // CROSSHAIR
            int arm = Math.Max(8, Math.Min(24, Math.Min(roiW, roiH) / 6));
            int gap = 4;

            DrawLine(pixels, w, h, new Models.PointF(cx - arm, cy), new Models.PointF(cx - gap, cy), outline, 5);
            DrawLine(pixels, w, h, new Models.PointF(cx + gap, cy), new Models.PointF(cx + arm, cy), outline, 5);
            DrawLine(pixels, w, h, new Models.PointF(cx, cy - arm), new Models.PointF(cx, cy - gap), outline, 5);
            DrawLine(pixels, w, h, new Models.PointF(cx, cy + gap), new Models.PointF(cx, cy + arm), outline, 5);

            DrawLine(pixels, w, h, new Models.PointF(cx - arm, cy), new Models.PointF(cx - gap, cy), fill, 3);
            DrawLine(pixels, w, h, new Models.PointF(cx + gap, cy), new Models.PointF(cx + arm, cy), fill, 3);
            DrawLine(pixels, w, h, new Models.PointF(cx, cy - arm), new Models.PointF(cx, cy - gap), fill, 3);
            DrawLine(pixels, w, h, new Models.PointF(cx, cy + gap), new Models.PointF(cx, cy + arm), fill, 3);
        }

        /// <summary>
        /// Apply overlay (ROI, locations, AR) onto raw BGRA8 pixel data.
        /// Returns the modified pixel array (a copy of the original).
        /// This is a public API so external handlers (e.g. plugin wrappers) can use it.
        /// </summary>
        public static byte[] ApplyOverlay(byte[] bgra8Pixels, int width, int height, OverlayData? overlay)
        {
            if (overlay == null) return bgra8Pixels;

            var pixels = new byte[bgra8Pixels.Length];
            Buffer.BlockCopy(bgra8Pixels, 0, pixels, 0, pixels.Length);

            int w = width, h = height;

            // Draw ROI overlay
            if (overlay.RoiVisible && overlay.Roi.HasValue)
            {
                var roi = overlay.Roi.Value;
                int roiX = (int)(roi.Left / 100f * w);
                int roiY = (int)(roi.Top / 100f * h);
                int roiW = (int)(roi.Width / 100f * w);
                int roiH = (int)(roi.Height / 100f * h);

                DimOutsideRect(pixels, w, h, roiX, roiY, roiW, roiH, overlay.RoiDimColor);
                DrawRect(pixels, w, h, roiX, roiY, roiW, roiH, overlay.RoiColor, overlay.RoiLineWidth);
            }

            DrawRoiCenterMark(pixels, w, h, overlay);

            // Draw barcode location polygons (non-AR mode)
            if (overlay.LocationEnabled && overlay.Locations != null && !overlay.AREnabled)
            {
                foreach (var loc in overlay.Locations)
                {
                    DrawLine(pixels, w, h, loc.TopLeft, loc.TopRight, overlay.LocationColor, overlay.LocationLineWidth);
                    DrawLine(pixels, w, h, loc.TopRight, loc.BottomRight, overlay.LocationColor, overlay.LocationLineWidth);
                    DrawLine(pixels, w, h, loc.BottomRight, loc.BottomLeft, overlay.LocationColor, overlay.LocationLineWidth);
                    DrawLine(pixels, w, h, loc.BottomLeft, loc.TopLeft, overlay.LocationColor, overlay.LocationLineWidth);
                }
            }

            // Draw AR overlays
            if (overlay.AREnabled && overlay.AREntries != null && overlay.ARConfig != null)
            {
                if (overlay.IsFrozen)
                {
                    var shadeColor = ((byte)0, (byte)0, (byte)0, (byte)128);
                    FillRect(pixels, w, h, 0, 0, w, h, shadeColor);
                }
                DrawAROverlays(pixels, w, h, overlay.AREntries, overlay.ARConfig);
            }

            return pixels;
        }
    }
}
