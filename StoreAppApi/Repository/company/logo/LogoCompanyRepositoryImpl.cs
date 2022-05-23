using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StoreAppApi.Repository.company.logo
{
    public class LogoCompanyRepositoryImpl : LogoCompanyRepository
    {
        public void DeleteProductLogo(int productId, string companyTitle, int companyId)
        {
            string path = $"companies/{companyTitle}_{companyId}/logo/logo_{companyTitle}_{companyId}";
            if (File.Exists(path))
                File.Delete(path);
        }

        public byte[] GetProductLogo(string companyTitle, int companyId)
        {
            string path = $"companies/{companyTitle}_{companyId}/logo/logo_{companyTitle}_{companyId}";
            if (File.Exists(path))
                return File.ReadAllBytes(path);
            else
                return null;
        }

        public void PostProductLogo(byte[] imgBytes, string companyTitle, int companyId)
        {
            string path = $"companies/{companyTitle}_{companyId}/logo/";

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

            image.Save($"{path}/logo_{companyTitle}_{companyId}");
        }
    }
}
