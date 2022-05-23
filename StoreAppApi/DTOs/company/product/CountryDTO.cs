using StoreAppApi.models.сompany.product;
using System.Collections.Generic;

namespace StoreAppApi.DTOs.company.product
{
    public class CountryDTO
    {
        public decimal Total
        {
            get
            {
                return Items.Count;
            }
        }

        public virtual List<Country> Items { get; set; } = new List<Country>();
    }
}
