using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace StoreAppApi.Repository.product.icon
{
    public class IconProductRepositoryImpl : IconProductRepository
    {
        const string ProductDir = "products/";

        public void DeleteProductIcon(
            int productId, string productTitle, string companyTitle)
        {
            var path = $"companies/{companyTitle}/{ProductDir}{productTitle}/icon/{productTitle}_{productId}.jpg";
            if (File.Exists(path))
                File.Delete(path);

        }

        public byte[] GetProductIcon(int productId, string productTitle, string companyTitle)
        {
            var path = $"companies/{companyTitle}/{ProductDir}{productTitle}/icon/{productTitle}_{productId}.jpg";
            if (File.Exists(path))
                return File.ReadAllBytes(path);
            else
                return null;
        }

        public void PostProductIcon(byte[] imgBytes, int productId, string productTitle, string companyTitle)
        {
            if (!Directory.Exists($"companies/{companyTitle}/{ProductDir}{productTitle}/icon/"))
                Directory.CreateDirectory($"companies/{companyTitle}/{ProductDir}{productTitle}/icon/");

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
            image.Save($"companies/{companyTitle}/{ProductDir}{productTitle}/icon/{productTitle}_{productId}.jpg");
        }
    }
}
