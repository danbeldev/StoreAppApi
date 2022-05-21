namespace StoreAppApi.Repository.product.icon
{
    public interface IconProductRepository
    {
        public byte[] GetProductIcon(int productId, string productTitle);

        public void PostProductIcon(byte[] imgBytes, int productId, string productTitle);

        public void DeleteProductIcon(int productId, string productTitle);
    }
}
