using Microsoft.AspNetCore.Http;

namespace StoreAppApi.Repository.product.video
{
    public interface VideoProductRepository
    {
        void UploadFile(IFormFile fileVideo, string companyTitle, string productTitle,
            int productId, int companyId);

        byte[] GetFile(string companyTitle, string productTitle,
            int productId, int companyId);
    }
}
