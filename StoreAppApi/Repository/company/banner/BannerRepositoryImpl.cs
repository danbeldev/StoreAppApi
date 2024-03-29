﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace StoreAppApi.Repository.company.banner
{
    public class BannerRepositoryImpl : BannerRepository
    {
        public void DeleteCompanyBanner(string companyTitle, int companyId)
        {
            string path = $"companies/{companyTitle}_{companyId}" +
                $"/banner/banner_{companyTitle}_{companyId}.jpg";
            if (File.Exists(path))
                File.Delete(path);
        }

        public byte[] GetCompanyBanner(string companyTitle, int companyId)
        {
            string path = $"companies/{companyTitle}_{companyId}" +
                $"/banner/banner_{companyTitle}_{companyId}.jpg";
            if (File.Exists(path))
                return File.ReadAllBytes(path);
            else
                return null;
        }

        public void PostCompanyBanner(byte[] imgBytes, string companyTitle, int companyId)
        {
            string path = $"companies/{companyTitle}_{companyId}/banner/";

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

            image.Save($"{path}/banner_{companyTitle}_{companyId}.jpg");
        }
    }
}
