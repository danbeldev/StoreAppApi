﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using StoreAppApi.common.extensions;
using System.IO;

namespace StoreAppApi.Repository.product.image
{
    public class ImageProductRepositoryImpl : ImageProductRepository
    {
        const string ProductDir = "products/";

        public void DeleteProductImage(
            int productId, string productTitle, string companyTitle,
            int productImageId, int companyId)
        {
            var path = $"companies/{companyTitle}_{companyId}" +
                $"/{ProductDir}{productTitle}_{productId}" +
                $"/images/{productTitle}_{productImageId}.jpg";

            if (File.Exists(path))
                File.Delete(path);
        }

        public byte[] GetProductImage(
            int productId, string productTitle, string companyTitle,
            int productImageId, int companyId)
        {
            var path = $"companies/{companyTitle}_{companyId}" +
                $"/{ProductDir}{productTitle}_{productId}" +
                $"/images/{productTitle}_{productImageId}.jpg";

            if (File.Exists(path))
                return File.ReadAllBytes(path);
            else
                return null;
        }

        public string GetProductImageSize(
            int productId, string productTitle, string companyTitle,
            int productImageId, int companyId)
        {
            var path = $"companies/{companyTitle}_{companyId}" +
                $"/{ProductDir}{productTitle}_{productId}" +
                $"/images/{productTitle}_{productImageId}.jpg";

            if (!File.Exists(path))
                return null;

            FileInfo file = new FileInfo(path);

            return file.Length.BytesToString();
        }

        public void PostProductImage(
            byte[] imgBytes, int productId, string productTitle,
            string companyTitle, int productImageId, int companyId)
        {
            var path = $"companies/{companyTitle}_{companyId}" +
                $"/{ProductDir}{productTitle}_{productId}/images/";

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

            image.Save($"{path}{productTitle}_{productImageId}.jpg");
        }
    }
}
