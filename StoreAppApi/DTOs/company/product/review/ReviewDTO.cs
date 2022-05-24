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

        public decimal TotalHasOneRating
        {
            get
            {
                int count = 0;
                foreach (Review item in Items)
                {
                    if (item.Rating == 1)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public decimal TotalHasTwoRating
        {
            get
            {
                int count = 0;
                foreach (Review item in Items)
                {
                    if (item.Rating == 2)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public decimal TotalHasThreeRating
        {
            get
            {
                int count = 0;
                foreach (Review item in Items)
                {
                    if (item.Rating == 3)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public decimal TotalHasFourRating
        {
            get
            {
                int count = 0;
                foreach (Review item in Items)
                {
                    if (item.Rating == 4)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public decimal TotalHasFiveRating
        {
            get
            {
                int count = 0;
                foreach (Review item in Items)
                {
                    if (item.Rating == 5)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public virtual List<Review> Items { get; set; } = new List<Review>();
    }
}
