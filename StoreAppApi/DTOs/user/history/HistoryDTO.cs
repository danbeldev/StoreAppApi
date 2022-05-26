using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreAppApi.DTOs.user.history
{
    public class HistoryDTO
    {
       public decimal Total
        {
            get
            {
                return Items.Count;
            }
        }

        public virtual List<HistoryItemDTO> Items { get; set; } = new List<HistoryItemDTO>();
    }
}
