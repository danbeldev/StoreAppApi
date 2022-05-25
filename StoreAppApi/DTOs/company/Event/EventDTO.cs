using System.Collections.Generic;

namespace StoreAppApi.DTOs.company.Event
{
    public class EventDTO
    {
        public decimal Total
        {
            get
            {
                return Items.Count;
            }
        }
        public virtual List<EventItemDTO> Items { get; set; } = new List<EventItemDTO>();
    }
}
