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

        public static string ToBase64String(this Bitmap bmp, ImageFormat imageFormat)
        {
            //MemorySteam ms = new MemorySteam();
            //bmp.Save(ms, ImageFormat.Png);
            //byte[] byteImage = ms.ToArray();
            //Convert.ToBase64String(byteImage);

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

        public static string CropBase64Image(string base64String, int x, int y, int width, int height)
        {
            var image = Base64ToImage(base64String);
            var source = image as Bitmap;

            var target = new Bitmap(width, height);

            using (var graphic = Graphics.FromImage(target))
            {
                graphic.DrawImage(source,
                    new RectangleF(x, y, width, height),
                    new RectangleF(0, 0, source.Width, source.Height),
                    GraphicsUnit.Pixel);

                var targetImage = ToBase64String(target, target.RawFormat);
                return targetImage;
            }
        }
    }
}
