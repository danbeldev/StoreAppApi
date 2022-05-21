using StoreAppApi.models.product.review;
using System.Collections.Generic;

namespace StoreAppApi.DTOs.product.review
{
    public class ReviewDTO
    {
        public decimal Total
        {
            get
            {
                return Items.Count;
            }
        }

        public virtual List<Review> Items { get; set; } = new List<Review>();
    }
}
