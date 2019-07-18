using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Coco.Common.Helper
{
    public static class ImageHelper
    {
        public static Image Base64ToImage(string base64String)
        {
            string base64 = base64String.Substring(base64String.IndexOf(',') + 1);
            byte[] imageBytes = Convert.FromBase64String(base64);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }

        public static string ToBase64String(this Bitmap bmp)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                string base64String = string.Empty;
                bmp.Save(memoryStream, ImageFormat.Png);

                memoryStream.Position = 0;
                byte[] byteBuffer = memoryStream.ToArray();

                memoryStream.Close();

                base64String = Convert.ToBase64String(byteBuffer);
                byteBuffer = null;

                return base64String;
            }
        }

        public static string CropBase64Image(string base64String, double x, double y, double width, double height)
        {
            var image = Base64ToImage(base64String);
            var xAxis = (int)(x * image.Width);
            var yAxis = (int)(y * image.Height);
            var newWidth = (int)(width * image.Width);
            var newHeight = (int)(height * image.Height);

            var cropRect = new Rectangle(xAxis, yAxis, newWidth, newHeight);
            using (var target = new Bitmap(newWidth, newHeight))
            {
                using (var graphic = Graphics.FromImage(target))
                {
                    graphic.DrawImage(image, new Rectangle(0, 0, target.Width, target.Height), cropRect,
                        GraphicsUnit.Pixel);

                    var targetImage = ToBase64String(target);
                    return targetImage;
                }
            }
        }
    }
}
