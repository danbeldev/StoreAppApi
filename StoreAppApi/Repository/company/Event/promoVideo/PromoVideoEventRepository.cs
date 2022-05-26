using Microsoft.AspNetCore.Http;

namespace StoreAppApi.Repository.company.Event.promoVideo
{
    public interface PromoVideoEventRepository
    {
        public void UploadFileVideo(
            IFormFile fileVideo, string companyTitle, int companyId,
            int productId, string productTitle,
            int eventId, string eventTitle
            );

        public byte[] GetFileVideo(
            string companyTitle, int companyId,
            int productId, string productTitle,
            int eventId, string eventTitle
            );
    }
}
