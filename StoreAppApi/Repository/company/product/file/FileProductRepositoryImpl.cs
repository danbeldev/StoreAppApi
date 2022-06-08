﻿using Microsoft.AspNetCore.Http;
using StoreAppApi.common.extensions;
using System;
using System.IO;

namespace StoreAppApi.Repository.product.file
{
    public class FileProductRepositoryImpl : FileProductRepository
    {
        public byte[] GetFile(
            string extension, string companyTitle,
            string productTitle, int productId, int companyId
            )
        {
            string startupPath = Directory.GetCurrentDirectory();

            string path = startupPath + $"/companies/" +
                $"{companyTitle}_{companyId}/products/" +
                $"{productTitle}_{productId}/file" +
                $"/file_{productTitle}_{productId}{extension}";

            Console.WriteLine(path);

            if (File.Exists(path))
                return File.ReadAllBytes(path);
            else
                return null;
        }

        public string GetFileSize(
            string extension, string companyTitle,
            string productTitle, int productId,
            int companyId
            )
        {
            string startupPath = Directory.GetCurrentDirectory();

            string path = startupPath + $"/companies/" +
                $"{companyTitle}_{companyId}/products/" +
                $"{productTitle}_{productId}/file" +
                $"/file_{productTitle}_{productId}{extension}";

            if (!File.Exists(path))
                return null;

            FileInfo file = new FileInfo(path);

            return file.Length.BytesToString();
        }

        public async void UploadFile(
            IFormFile file, string companyTitle, string productTitle,
            int productId, int companyId
            )
        {
            string startupPath = Directory.GetCurrentDirectory();

            string extension = Path.GetExtension(file.FileName);

            string path = startupPath + $"/companies/" +
                $"{companyTitle}_{companyId}/products/" +
                $"{productTitle}_{productId}/file/";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (var fileStream = new FileStream("" +
                $"{path}file_{productTitle}_{productId}{extension}", FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }
    }
}
