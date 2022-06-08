namespace StoreAppApi.Repository.product.image
{
    public interface ImageProductRepository
    {
        public byte[] GetProductImage(
            int productId, string productTitle, string companyTitle,
            int productImageId, int companyId);

        public string GetProductImageSize(
            int productId, string productTitle, string companyTitle,
            int productImageId, int companyId
            );

        public void PostProductImage(
            byte[] imgBytes, int productId, string productTitle,
            string companyTitle, int productImageId, int companyId);

        public void DeleteProductImage(
            int productId, string productTitle, string companyTitle,
            int productImageId, int companyId);
    }
}