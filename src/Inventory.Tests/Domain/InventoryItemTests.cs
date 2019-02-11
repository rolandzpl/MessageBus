using Inventory.Domain.Events;
using NUnit.Framework;

namespace Inventory.Domain
{
    [TestFixture]
    class InventoryItemTests
    {
        [Test]
        public void GetUncommittedChanges_InventoryCreated_InventoryItemAddedEventReturned()
        {
            var item = InventoryItem.Create();
            var changes = item.GetUncommittedEvents();
            Assert.That(changes, Has.Exactly(1).InstanceOf<InventoryItemAdded>());
        }

        [Test]
        public void GetUncommittedChanges_InventoryRemoved_InventoryItemRemovedEventReturned()
        {
            var item = InventoryItem.Create();
            item.ClearUncommittedEvents();
            item.Remove();
            var changes = item.GetUncommittedEvents();
            Assert.That(changes, Has.Exactly(1).InstanceOf<InventoryItemRemoved>());
        }
    }
}
