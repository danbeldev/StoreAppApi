using StoreAppApi.models.product;
using StoreAppApi.models.сompany.Event.enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StoreAppApi.models.сompany.Event
{
    public class Event
    {
        [Key] public int Id { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string ShortDescription { get; set; }
        [Required] public string FullDescription { get; set; }
        public string PromoVideoUrl { get; set; }
        public string WebUrl { get; set; }
        public string PromoImageUrl { get; set; }
        [Required] public DateTime DatePublication { get; set; }
        [Required] public EventStatus EventStatus { get; set; }

        [JsonIgnore] public Сompany Company { get; set; }
        [JsonIgnore] public Product Product { get; set; }
    }
}
