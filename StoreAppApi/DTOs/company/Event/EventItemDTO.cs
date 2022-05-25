using StoreAppApi.models.сompany.Event.enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace StoreAppApi.DTOs.company.Event
{
    public class EventItemDTO
    {
        [Key] public int Id { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string ShortDescription { get; set; }
        [Required] public string FullDescription { get; set; }
        public string PromoVideoUrl { get; set; }
        public string PromoImageUrl { get; set; }
        [Required] public DateTime DatePublication { get; set; }
        [Required] public EventStatus EventStatus { get; set; }
        public string WebUrl { get; set; }
    }
}
