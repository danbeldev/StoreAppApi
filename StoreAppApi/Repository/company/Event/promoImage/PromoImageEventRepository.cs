namespace StoreAppApi.Repository.company.Event.promo
{
    public interface PromoImageEventRepository
    {
        public byte[] GetCompanyPromo(
            string companyTitle, int companyId,int productId, string productTitle,
            int eventId, string eventTitle);

        public string GetCompanyPromoSize(
            string companyTitle, int companyId, int productId, string productTitle,
            int eventId, string eventTitle
            );

        public void PostCompanyPromo(
            byte[] imgBytes, string companyTitle, int companyId,
            int productId, string productTitle,
            int eventId, string eventTitle);

    }
}
