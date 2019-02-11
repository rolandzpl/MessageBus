using DDD.Domain;
using Inventory.Domain.Events;

namespace Inventory.Domain
{
    public class InventoryItem : AggregateRoot
    {
        public static InventoryItem Create()
        {
            return new InventoryItem();
        }

        private InventoryItem()
        {
            AddEvent(new InventoryItemAdded());
        }

        public void Remove()
        {
            AddEvent(new InventoryItemRemoved());
        }
    }
}
