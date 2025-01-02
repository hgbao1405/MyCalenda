using MakeEvent.Core.Domain.Entities;
using MakeEvent.Infrastructure.Persistence;

namespace MakeEvent.Infrastructure.Service
{
    public class EventStorage
    {
        private readonly List<Event> _events;
        private readonly object _lock = new();

        public EventStorage()
        {
            _events = new List<Event>();
        }

        public List<Event> GetEvents() => _events;

        public void AddEvent(Event ev)
        {
            lock (_lock)
            {
                _events.Add(ev);
            }
        }

        public void RemoveEvent(Event ev)
        {
            lock (_lock)
            {
                _events.Remove(ev);
            }
        }

        public void LoadEvents(IEnumerable<Event> events)
        {
            lock (_lock)
            {
                _events.Clear();
                _events.AddRange(events);
            }
        }
    }


}
