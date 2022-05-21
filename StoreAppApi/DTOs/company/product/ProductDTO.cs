using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreAppApi.DTOs.product
{
    public class ProductDTO
    {
        public decimal Total
        {
            get
            {
                return Items.Count;
            }
        }

        [Required] public virtual List<ProductItemDTO> Items { get; set; } = new List<ProductItemDTO>();
    }
}
