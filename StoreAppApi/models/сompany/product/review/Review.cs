using StoreAppApi.models.user;
using System;
using System.ComponentModel.DataAnnotations;

namespace StoreAppApi.models.product.review
{
    public class Review
    {
        [Key] public int Id { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
        [Required] public float Rating { get; set; }
        [Required] public DateTime DatePublication { get; set; }
        [Required] public BaseUser User { get; set; }
        [Required] public Product Product { get; set; }
    }
}
