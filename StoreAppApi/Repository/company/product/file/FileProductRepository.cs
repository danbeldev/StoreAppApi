using Microsoft.AspNetCore.Http;

namespace StoreAppApi.Repository.product.file
{
    public interface FileProductRepository
    {
        void UploadFile(IFormFile file, string extension, string companyTitle, string productTitle,
            int productId);
    }
}
