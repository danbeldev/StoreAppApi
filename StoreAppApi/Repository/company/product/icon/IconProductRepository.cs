namespace StoreAppApi.Repository.product.icon
{
    public interface IconProductRepository
    {
        public byte[] GetProductIcon(
            int productId, string productTitle,
            string companyTitle, int companyId
            );

        public string GetProductIconSize(
            int productId, string productTitle,
            string companyTitle, int companyId
            );

        public void PostProductIcon(
            byte[] imgBytes, int productId, string productTitle,
            string companyTitle, int companyId);

        public void DeleteProductIcon(
            int productId, string productTitle, string companyTitle, int companyId);
    }
}
