using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace FoodOrderSystem.App.Helper
{
    public static class ImageHelper
    {
        public static byte[] ConvertFromImageUrl(string imgUrl)
        {
            var base64Data = Regex.Match(imgUrl, @"data:image/(?<type>.+?);(?<format>.+?),(?<data>.+)").Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);

            using (var image = Image.Load(binData))
            using (var memStream = new MemoryStream())
            {
                image.SaveAsJpeg(memStream);
                return memStream.ToArray();
            }
        }

    }
}
