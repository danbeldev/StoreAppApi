using StoreAppApi.models.product;
using StoreAppApi.models.product.enums;
using StoreAppApi.models.user;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StoreAppApi.DTOs.product
{
    public class ProductItemDTO
    {
        [Required] public int Id { get; set; }
        [Required] public string Icon { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
        public int? Price { get; set; }
        [Required] public DateTime DatePublication { get; set; }
        [Required] public ProductType ProductType { get; set; }
        [Required] public Genre Genre { get; set; }
        public decimal ImagesTotal
        {
            get
            {
                return Images.Count;
            }
        }
        public virtual List<Image> Images { get; set; } = new List<Image>();
        public Video Video { get; set; }

        public decimal UserDownloadTotal
        {
            get
            {
                return UserDownload.Count;
            }
        }

        [JsonIgnore] public virtual List<BaseUser> UserDownload { get; set; } = new List<BaseUser>();

    }
}
