using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FoodOrderSystem.Core.Application.DTOs
{
    public class PagingDTO<TItem>
    {
        public PagingDTO(int total, int skipped, int taken, IEnumerable<TItem> items)
        {
            Total = total;
            Skipped = skipped;
            Taken = taken;
            Items = new ReadOnlyCollection<TItem>(items.ToList());
        }
        
        public int Total { get; }
        
        public int Skipped { get; }
        
        public int Taken { get; }
        
        public IReadOnlyCollection<TItem> Items { get; } 
    }
}