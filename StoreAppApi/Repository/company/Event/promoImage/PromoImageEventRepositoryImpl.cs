using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace StoreAppApi.Repository.company.Event.promo
{
    public class PromoImageEventRepositoryImpl : PromoImageEventRepository
    {
        public byte[] GetCompanyPromo(
            string companyTitle, int companyId,
            int productId, string productTitle,
            int eventId, string eventTitle)
        {
            string path = $"companies/{companyTitle}_{companyId}" +
                $"/products/{productTitle}_{productId}" +
                $"/events/{eventTitle}_{eventId}/promoImage/event_{eventTitle}_{eventId}.jpg";
            
            if (File.Exists(path))
                return File.ReadAllBytes(path);
            else
                return null;
        }

        public void PostCompanyPromo(
            byte[] imgBytes, string companyTitle, int companyId,
            int productId, string productTitle,
            int eventId, string eventTitle)
        {
            string path = $"companies/{companyTitle}_{companyId}" +
                $"/products/{productTitle}_{productId}" +
                   $"/events/{eventTitle}_{eventId}/promoImage/";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var image = Image.Load(imgBytes);
            image.Mutate(m =>
                m.Resize(
                    new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(512)
                    }
                 )
            );
            image.Save($"{path}event_{eventTitle}_{eventId}.jpg");
        }
    }
}
