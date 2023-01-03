using SkiaSharp;

namespace Camino.Shared.Utils
{
    public static class ImageUtils
    {
        public static bool IsImageUrl(string url)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(url),
            };

            var response = httpClient.GetAsync(url).Result;
            var contentType = response.Content.Headers.ContentType.MediaType.ToLower();
            return contentType.StartsWith("image/");
        }

        public static SKImage Base64ToImage(string base64String)
        {
            string base64 = base64String.Substring(base64String.IndexOf(',') + 1);
            byte[] imageBytes = Convert.FromBase64String(base64);
            return FileDataToImage(imageBytes);
        }

        public static SKImage FileDataToImage(byte[] fileData)
        {
            using var data = SKData.CreateCopy(fileData);
            var image = SKImage.FromEncodedData(data);
            return image;
        }

        public static byte[] Crop(byte[] fileData, double x, double y, double width, double height, double scale, int maxSize = 600)
        {
            using var resizedImage = Resize(fileData, maxSize);

            var xAxis = (int)(x * resizedImage.Width);
            var yAxis = (int)(y * resizedImage.Height);
            var right = (int)((width * resizedImage.Width) + xAxis);
            var newHeight = (int)((height * resizedImage.Height) + yAxis);

            using var subset = resizedImage.Subset(new SKRectI
            {
                Left = xAxis,
                Right = right ,
                Top = yAxis,
                Bottom = newHeight,
            });

            using var data = subset.Encode(SKEncodedImageFormat.Png, (int)SKFilterQuality.High);
            return data.ToArray();
        }

        private static SKImage Resize(byte[] fileData, int maxSize)
        {
            using var image = FileDataToImage(fileData);
            if (image.Width > maxSize && image.Width >= image.Height)
            {
                var ratio = maxSize / (float)image.Width;
                var srcHeight = image.Height * ratio;
                return Resize(fileData, maxSize, (int)srcHeight);
            }
            else if (image.Height > maxSize && image.Height > image.Width)
            {
                var ratio = maxSize / (float)image.Height;
                var srcWidth = image.Width * ratio;
                return Resize(fileData, (int)srcWidth, maxSize);
            }

            return image;
        }

        public static SKImage Resize(byte[] bytes, int width, int height)
        {
            var quality = SKFilterQuality.Medium;
            using var stream = new MemoryStream(bytes);
            using var sourceBitmap = SKBitmap.Decode(stream);

            using var scaledBitmap = sourceBitmap.Resize(new SKImageInfo(width, height), quality);
            var data = scaledBitmap.Encode(SKEncodedImageFormat.Png, (int)quality);
            return FileDataToImage(data.ToArray());
        }
    }
}
