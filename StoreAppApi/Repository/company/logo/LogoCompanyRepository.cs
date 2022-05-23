using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreAppApi.Repository.company.logo
{
    interface LogoCompanyRepository
    {
        public byte[] GetProductLogo(string companyTitle, int companyId);

        public void PostProductLogo(byte[] imgBytes, string companyTitle, int companyId);

        public void DeleteProductLogo(int productId, string companyTitle, int companyId);

    }
}
