using System.Collections.Generic;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class PagingViewModel<TItem>
    {
        public PagingViewModel(int total, int skipped, int taken, IReadOnlyCollection<TItem> items)
        {
            Total = total;
            Skipped = skipped;
            Taken = taken;
            Items = items;
        }
        
        public int Total { get; }
        
        public int Skipped { get; }
        
        public int Taken { get; }
        
        public IReadOnlyCollection<TItem> Items { get; } 
    }
}