namespace StoreAppApi.Repository.company.banner
{
    public interface BannerRepository
    {
        public byte[] GetCompanyBanner(string companyTitle, int companyId);

        public string GetCompanyBannerSize(string companyTitle, int companyId);

        public void PostCompanyBanner(byte[] imgBytes, string companyTitle, int companyId);

        public void DeleteCompanyBanner(string companyTitle, int companyId);

    }
}
