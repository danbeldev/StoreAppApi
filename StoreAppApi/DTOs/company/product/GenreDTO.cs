using StoreAppApi.models.product;
using System.Collections.Generic;

namespace StoreAppApi.DTOs.product
{
    public class GenreDTO
    {
        public decimal Total
        {
            get
            {
                return Items.Count;
            }
        }
        public virtual List<Genre> Items { get; set; } = new List<Genre>();
    }
}
