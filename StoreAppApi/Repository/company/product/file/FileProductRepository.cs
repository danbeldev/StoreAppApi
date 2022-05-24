using Microsoft.AspNetCore.Http;

namespace StoreAppApi.Repository.product.file
{
    public interface FileProductRepository
    {
        void UploadFile(IFormFile file, string companyTitle, string productTitle,
            int productId, int companyId);

        byte[] GetFile(string extension, string companyTitle, string productTitle,
            int productId, int companyId);
    }
}
