using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MakeEvent.Infrastructure.Persistence;
using MakeEvent.Core.Domain.Entities;
using MakeEvent.Core.Application.UseCase.Event;
using MakeEvent.Infrastructure.Service;

namespace MakeEvent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : BaseController
    {
        private readonly AddEvent _add;
        private readonly DeleteEvent _delete;
        private readonly UpdateEvent _update;
        private readonly GetListEvent _list;
        private readonly GetEventById _getById;

        public EventsController(AddEvent add,
            DeleteEvent delete,
            UpdateEvent update,
            GetListEvent list,
            GetEventById getById)
        {
            _add = add;
            _delete = delete;
            _update = update;
            _list = list;
            _getById = getById;
        }

        // GET: api/Events
        [HttpGet]
        public IActionResult GetEvent(int? year, int? month)
        {
            return Execute(() =>
            {
                return Ok(_list.Execute(year,month));
            });
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public IActionResult GetEvent(int id)
        {
            return Execute(() =>
            {
                return Ok(_getById.Execute(id));
            });
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutEvent(int id, Event @event)
        {
            return Execute(() =>
            {
                _update.Execute(id, @event);
                return Ok();
            });
        }

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult PostEvent(Event @event)
        {
            return Execute(() =>
            {
                _add.Execute(@event);
                return Ok();
            });
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            return Execute(() =>
            {
                _delete.Execute(id);
                return Ok();
            });
        }
    }
}
