namespace StoreAppApi.Repository.product.image
{
    public interface ImageProductRepository
    {
        public byte[] GetProductImage(
            int productId, string productTitle, string companyTitle, int productImageId);

        public void PostProductImage(
            byte[] imgBytes, int productId, string productTitle, string companyTitle, int productImageId);

        public void DeleteProductImage(
            int productId, string productTitle, string companyTitle, int productImageId);
    }
}