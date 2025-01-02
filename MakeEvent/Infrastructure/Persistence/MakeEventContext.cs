using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MakeEvent.Core.Domain.Entities;

namespace MakeEvent.Infrastructure.Persistence
{
    public class MakeEventContext : DbContext
    {
        public MakeEventContext(DbContextOptions<MakeEventContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Event { get; set; } = default!;
    }
}
