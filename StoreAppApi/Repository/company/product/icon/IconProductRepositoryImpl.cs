using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace StoreAppApi.Repository.product.icon
{
    public class IconProductRepositoryImpl : IconProductRepository
    {
        const string ProductDir = "products/";

        public IconProductRepositoryImpl()
        {
            if (!Directory.Exists(ProductDir))
                Directory.CreateDirectory(ProductDir);
        }

        public void DeleteProductIcon(int productId, string productTitle)
        {
            var path = $"{ProductDir}/{productTitle}/icon/{productId}.jpg";
            if (File.Exists(path))
                File.Delete(path);

        }

        public byte[] GetProductIcon(int productId, string productTitle)
        {
            var path = $"{ProductDir}/{productTitle}/icon/{productId}.jpg";
            if (File.Exists(path))
                return File.ReadAllBytes(path);
            else
                return null;
        }

        public void PostProductIcon(byte[] imgBytes, int productId, string productTitle)
        {
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
            image.Save($"{ProductDir}/{productTitle}/icon/{productId}.jpg");
        }
    }
}
