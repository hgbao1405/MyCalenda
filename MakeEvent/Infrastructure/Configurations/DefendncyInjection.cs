
using MakeEvent.Core.Application.Interfaces;
using MakeEvent.Core.Application.UseCase.Event;
using MakeEvent.Core.Domain.Entities;
using MakeEvent.Infrastructure.Persistence;
using MakeEvent.Infrastructure.Service;

namespace MakeEvent.Infrastructure.Configurations
{
    public static class DefendncyInjection
    {
        internal static void AddService(WebApplicationBuilder builder)
        {
            //add repositories
            builder.Services.AddScoped<IRepository<Event>,EventRepostitory>();

            builder.Services.AddScoped<HandleExceptionService>();

            //Add UseCase
            builder.Services.AddScoped<AddEvent>();
            builder.Services.AddScoped<UpdateEvent>();
            builder.Services.AddScoped<DeleteEvent>();
            builder.Services.AddScoped<GetListEvent>();
            builder.Services.AddScoped<GetEventById>();

        }
    }
}
