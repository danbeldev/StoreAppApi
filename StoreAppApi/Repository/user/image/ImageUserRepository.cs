namespace StoreAppApi.Repository
{
    public interface ImageUserRepository
    {
        public byte[] GetUserImage(int userId);

        public string GetUserImageSize(int userId);

        public void PostUserImage(byte[] image, int userId);

        public void DeleteUserImage(int userId);
    }
}
