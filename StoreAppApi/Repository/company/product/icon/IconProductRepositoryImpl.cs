using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using StoreAppApi.common.extensions;
using System.IO;

namespace StoreAppApi.Repository.product.icon
{
    public class IconProductRepositoryImpl : IconProductRepository
    {
        const string ProductDir = "products/";

        public void DeleteProductIcon(
            int productId, string productTitle, string companyTitle, int companyId)
        {
            var path = $"companies/" +
                $"{companyTitle}_{companyId}" +
                $"/{ProductDir}{productTitle}_{productId}/icon/{productTitle}_{productId}.jpg";
            
            if (File.Exists(path))
                File.Delete(path);

        }

        public byte[] GetProductIcon(int productId,
            string productTitle, string companyTitle, int companyId)
        {
            var path = $"companies/" +
                $"{companyTitle}_{companyId}/" +
                $"{ProductDir}{productTitle}_{productId}" +
                $"/icon/{productTitle}_{productId}.jpg";
            
            if (File.Exists(path))
                return File.ReadAllBytes(path);
            else
                return null;
        }

        public string GetProductIconSize(int productId,
            string productTitle, string companyTitle, int companyId)
        {
            var path = $"companies/" +
                $"{companyTitle}_{companyId}/" +
                $"{ProductDir}{productTitle}_{productId}" +
                $"/icon/{productTitle}_{productId}.jpg";

            if (!File.Exists(path))
                return null;

            FileInfo file = new FileInfo(path);

            return file.Length.BytesToString();
        }

        public void PostProductIcon(
            byte[] imgBytes, int productId,
            string productTitle, string companyTitle, int companyId)
        {
            string path = $"companies/" +
                    $"{companyTitle}_{companyId}/{ProductDir}" +
                    $"{productTitle}_{productId}/icon/";

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
            image.Save($"{path}{productTitle}_{productId}.jpg");
        }
    }
}
