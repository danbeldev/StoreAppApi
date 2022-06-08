using Microsoft.AspNetCore.Http;
using StoreAppApi.common.extensions;
using System.IO;

namespace StoreAppApi.Repository.product.video
{
    public class VideoProductRepositoryImpl : VideoProductRepository
    {
        public byte[] GetFile(
            string companyTitle, string productTitle,
            int productId, int companyId)
        {
            string startupPath = Directory.GetCurrentDirectory();

            string path = startupPath + $"/companies/" +
                $"{companyTitle}_{companyId}/products/" +
                $"{productTitle}_{productId}/video/" +
                $"video_{productTitle}_{productId}.mp4";

            if (File.Exists(path))
                return File.ReadAllBytes(path);
            else
                return null;
        }

        public string GetFileSize(
            string companyTitle, string productTitle,
            int productId, int companyId)
        {
            string startupPath = Directory.GetCurrentDirectory();

            string path = startupPath + $"/companies/" +
                $"{companyTitle}_{companyId}/products/" +
                $"{productTitle}_{productId}/video/" +
                $"video_{productTitle}_{productId}.mp4";

            if (!File.Exists(path))
                return null;

            FileInfo file = new FileInfo(path);

            return file.Length.BytesToString();
        }

        public async void UploadFile(
            IFormFile fileVideo, string companyTitle,
            string productTitle, int productId, int companyId)
        {
            string startupPath = Directory.GetCurrentDirectory();

            string path = startupPath + $"/companies/" +
                $"{companyTitle}_{companyId}/products/" +
                $"{productTitle}_{productId}/video/";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (var fileStream = new FileStream("" +
                $"{path}video_{productTitle}_{productId}.mp4", FileMode.Create))
            {
                await fileVideo.CopyToAsync(fileStream);
            }
        }
    }
}
