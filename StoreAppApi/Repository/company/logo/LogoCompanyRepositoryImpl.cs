using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using StoreAppApi.common.extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StoreAppApi.Repository.company.logo
{
    public class LogoCompanyRepositoryImpl : LogoCompanyRepository
    {
        public void DeleteCompanyLogo(string companyTitle, int companyId)
        {
            string path = $"companies/{companyTitle}_{companyId}" +
                $"/logo/logo_{companyTitle}_{companyId}.jpg";
            if (File.Exists(path))
                File.Delete(path);
        }

        public byte[] GetCompanyLogo(string companyTitle, int companyId)
        {
            string path = $"companies/{companyTitle}_{companyId}" +
                $"/logo/logo_{companyTitle}_{companyId}.jpg";

            if (File.Exists(path))
                return File.ReadAllBytes(path);
            else
                return null;
        }

        public string GetCompanyLogoSize(string companyTitle, int companyId)
        {
            string path = $"companies/{companyTitle}_{companyId}" +
                $"/logo/logo_{companyTitle}_{companyId}.jpg";

            if (!File.Exists(path))
                return null;

            FileInfo file = new FileInfo(path);

            return file.Length.BytesToString();
        }

        public void PostCompanyLogo(byte[] imgBytes, string companyTitle, int companyId)
        {
            string path = $"companies/{companyTitle}_{companyId}/logo/";

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

            image.Save($"{path}/logo_{companyTitle}_{companyId}.jpg");
        }
    }
}
