namespace StoreAppApi.Repository.company.logo
{
    public interface LogoCompanyRepository
    {
        public byte[] GetCompanyLogo(string companyTitle, int companyId);

        public void PostCompanyLogo(byte[] imgBytes, string companyTitle, int companyId);

        public void DeleteCompanyLogo(string companyTitle, int companyId);

    }
}
