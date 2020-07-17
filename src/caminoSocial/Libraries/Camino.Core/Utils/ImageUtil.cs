using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace Camino.Core.Utils
{
    public static class ImageUtil
    {
        public static bool IsImageUrl(string url)
        {
            var req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "HEAD";
            using (var response = req.GetResponse())
            {
                var contentType = response.ContentType.ToLower();
                return contentType.StartsWith("image/");
            }
        }

        public static Image Base64ToImage(string base64String)
        {
            string base64 = base64String.Substring(base64String.IndexOf(',') + 1);
            byte[] imageBytes = Convert.FromBase64String(base64);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                var image = Image.FromStream(ms, true);
                return image;
            }
        }

        public static string ToBase64String(this Bitmap bmp)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                string base64String = string.Empty;
                bmp.Save(memoryStream, ImageFormat.Jpeg);

                memoryStream.Position = 0;
                byte[] byteBuffer = memoryStream.ToArray();

                memoryStream.Close();

                base64String = Convert.ToBase64String(byteBuffer);

                return base64String;
            }
        }

        public static string Crop(string base64String, double x, double y, double width, double height, double scale, int maxSize = 600)
        {
            var image = Base64ToImage(base64String);

            return Crop(image, x, y, width, height, scale, maxSize);
        }

        public static string Crop(Image image, double x, double y, double width, double height, double scale, int maxSize = 600)
        {
            if (image.Width > maxSize && image.Width >= image.Height)
            {
                var ratio = maxSize / (float)image.Width;
                var srcHeight = image.Height * ratio;
                image = Resize(image, maxSize, (int)srcHeight);
            }
            else if (image.Height > maxSize && image.Height > image.Width)
            {
                var ratio = maxSize / (float)image.Height;
                var srcWidth = image.Width * ratio;
                image = Resize(image, (int)srcWidth, maxSize);
            }

            var xAxis = (int)(x * image.Width);
            var yAxis = (int)(y * image.Height);
            var newWidth = (int)(width * image.Width);
            var newHeight = (int)(height * image.Height);

            using (var target = new Bitmap(newWidth, newHeight))
            {
                using (var graphic = Graphics.FromImage(target))
                {
                    var srcRect = new Rectangle(xAxis, yAxis, newWidth, newHeight);
                    var descRect = new Rectangle(0, 0, newWidth, newHeight);
                    graphic.DrawImage(image, descRect, srcRect, GraphicsUnit.Pixel);

                    var targetImage = ToBase64String(target);
                    return targetImage;
                }
            }
        }

        public static Bitmap Resize(string base64String, int width, int height)
        {
            var image = Base64ToImage(base64String);

            return Resize(image, width, height);
        }

        public static Bitmap Resize(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
