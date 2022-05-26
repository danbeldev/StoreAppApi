using Microsoft.AspNetCore.Http;
using StoreAppApi.Repository.company.Event.promoVideo;
using System.IO;

namespace StoreAppApi.Repository.company.Event.promoVideo
{
    public class PromoVideoEventRepositoryImpl : PromoVideoEventRepository
    {
        public byte[] GetFileVideo(
            string companyTitle, int companyId, int productId, string productTitle, int eventId, string eventTitle)
        {
            string path = $"companies/{companyTitle}_{companyId}" +
                $"/products/{productTitle}_{productId}" +
                $"/events/{eventTitle}_{eventId}/promoVideo" +
                $"/event_{eventTitle}_{eventId}.mp4";

            if (File.Exists(path))
                return File.ReadAllBytes(path);
            else
                return null;
        }

        public async void UploadFileVideo(
            IFormFile fileVideo, string companyTitle,
            int companyId, int productId, string productTitle,
            int eventId, string eventTitle)
        {
            string path = $"companies/{companyTitle}_{companyId}" +
                $"/products/{productTitle}_{productId}" +
                   $"/events/{eventTitle}_{eventId}/promoVideo/";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (var fileStream = new FileStream("" +
                $"{path}event_{eventTitle}_{eventId}.mp4", FileMode.Create))
            {
                await fileVideo.CopyToAsync(fileStream);
            }
        }
    }
}
