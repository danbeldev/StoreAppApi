using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using StoreAppApi.common.extensions;
using System.IO;

namespace StoreAppApi.Repository.image
{
    public class ImageUserRepositoryImpl : ImageUserRepository
    {
        const string ImageUserDir = "images/user/";

        public ImageUserRepositoryImpl()
        {
            if (!Directory.Exists(ImageUserDir))
                Directory.CreateDirectory(ImageUserDir);
        }

        public void DeleteUserImage(int userId)
        {
            var path = $"{ImageUserDir}{userId}.jpg";
            if (File.Exists(path))
                File.Delete(path);
        }

        public byte[] GetUserImage(int userId)
        {
            var path = $"{ImageUserDir}{userId}.jpg";

            if (File.Exists(path))
                return File.ReadAllBytes(path);
            else
                return null;
        }

        public string GetUserImageSize(int userId)
        {
            var path = $"{ImageUserDir}{userId}.jpg";

            if (!File.Exists(path))
                return null;

            FileInfo file = new FileInfo(path);

            return file.Length.BytesToString();
        }

        public void PostUserImage(byte[] imgBytes, int userId)
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
            image.Save($"{ImageUserDir}{userId}.jpg");
        }
    }
}
