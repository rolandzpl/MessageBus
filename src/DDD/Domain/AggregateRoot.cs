using System.Collections.Generic;
using System.Linq;

namespace DDD.Domain
{
    public class AggregateRoot
    {
        private List<Event> changes = new List<Event>();

        public IEnumerable<Event> GetUncommittedEvents()
        {
            return changes.ToList();
        }

        public void ClearUncommittedEvents()
        {
            changes.Clear();
        }

        protected void AddEvent(Event e)
        {
            changes.Add(e);
        }
    }
}
