using Microsoft.AspNetCore.Http;
using System.IO;

namespace StoreAppApi.Repository.product.file
{
    public class FileProductRepositoryImpl : FileProductRepository
    {
        
        public async void UploadFile(
            IFormFile file, string extension, string companyTitle, string productTitle,
            int productId
            )
        {
            string startupPath = Directory.GetCurrentDirectory();

            string path = startupPath + $"/companies/{companyTitle}/products/{productTitle}_{productId}{extension}";

            using(var fileStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }
    }
}
