using StoreAppApi.models.product.enums;
using StoreAppApi.models.product.review;
using StoreAppApi.models.user;
using StoreAppApi.models.сompany;
using StoreAppApi.models.сompany.product;
using StoreAppApi.models.сompany.product.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StoreAppApi.models.product
{
    public class Product
    {
        [Key] public int Id { get; set; }
        public string Icon { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string ShortDescription { get; set; }
        [Required] public string FullDescription { get; set; }
        [Required] public Boolean Advertising { get; set; }
        [Required] public string Version { get; set; }
        [Required] public Country Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public virtual List<SocialNetwork> SocialNetwork { get; set; } = new List<SocialNetwork>();
        [Required] public AgeRating AgeRating { get; set; }
        public int? Price { get; set; }
        public string PrivacyPolicyWebUrl { get; set; }
        public string FileUrl { get; set; }
        public ProductExtension FileExtension { get; set; }
        [Required] public DateTime DatePublication { get; set; }
        [Required] public ProductType ProductType { get; set; }
        [Required] public ProductStatus ProductStatus { get; set; }
        [Required] public Genre Genre { get; set; }
        public virtual List<Image> Images { get; set; } = new List<Image>();
        public Video Video { get; set; }

        public decimal UserDownloadTotal
        {
            get
            {
                return UserDownload.Count;
            }
        }

        public decimal ReviewsTotal
        {
            get
            {
                return Reviews.Count;
            }
        }

        public float? Rating
        {
            get
            {
                if (Reviews == null || Reviews.Count == 0)
                    return null;

                float sum = 0f;
                for (int i = 0; i < Reviews.Count; i++)
                {
                    sum += Reviews[i].Rating;
                }
                return sum / Reviews.Count;
            }
        }

        [JsonIgnore, Required] public Сompany Company { get; set; }
        [JsonIgnore] public virtual List<Review> Reviews { get; set; } = new List<Review>();
        [JsonIgnore] public virtual List<BaseUser> UserDownload { get; set; } = new List<BaseUser>();
    }
}
