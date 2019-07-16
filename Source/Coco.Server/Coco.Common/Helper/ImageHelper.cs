using System;
using System.Drawing;
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
    }
}
