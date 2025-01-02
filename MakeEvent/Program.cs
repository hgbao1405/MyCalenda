using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MakeEvent.Infrastructure.Persistence;
using MakeEvent.Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using MakeEvent.Infrastructure.Service;

namespace MakeEvent
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MakeEventContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MakeEventContext") ?? throw new InvalidOperationException("Connection string 'MakeEventContext' not found.")));

            builder.Services.AddControllers();

            DefendncyInjection.AddService(builder);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<EventStorage>();
            builder.Services.AddSignalR();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapControllers(); 

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await InitializeEventStorageAsync(app.Services);
            app.Run();
        }

        static async Task InitializeEventStorageAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MakeEventContext>();
            var eventStorage = scope.ServiceProvider.GetRequiredService<EventStorage>();

            // Nạp dữ liệu từ cơ sở dữ liệu vào `EventStorage`
            var today = DateTime.UtcNow.Date;
            var startOfDay = today; // Ngày bắt đầu
            var endOfDay = today.AddDays(1).AddTicks(-1); // Kết thúc ngày hôm nay

            var monthlyEvents = await dbContext.Event
                .Where(e => e.LoopType == "Monthly" &&
                            e.StartTime <= endOfDay &&
                            (e.EndTime == null || e.EndTime >= startOfDay) &&
                            e.StartTime.Day == today.Day).ToListAsync();

            var yearlyEvents = await dbContext.Event
                .Where(e => e.LoopType == "Yearly" &&
                            e.StartTime <= endOfDay &&
                            (e.EndTime == null || e.EndTime >= startOfDay) &&
                            e.StartTime.Day == today.Day &&
                            e.StartTime.Month == today.Month).ToListAsync();

            var events = await dbContext.Event
                .Where(e=>(e.LoopType=="None" && e.StartTime.Date==today)
                || (e.LoopType=="Daily" && e.StartTime<=today &&(e.EndTime==null|| e.EndTime >= startOfDay)
                //|| (e.LoopType=="Monthly" && e.StartTime <= today && (e.EndTime == null || e.EndTime.Value.Date >= today) && e.StartTime.Day==today.Day)
                //|| (e.LoopType=="Yearly" && e.StartTime <= today && (e.EndTime == null || e.EndTime.Value.Date >= today) && (e.StartTime.Day == today.Day && e.StartTime.Month==today.Month))
                )).Concat(monthlyEvents).Concat(yearlyEvents)
                .ToListAsync();
            
            foreach (var ev in events)
            {
                eventStorage.AddEvent(ev);
            }
        }
    }
}
