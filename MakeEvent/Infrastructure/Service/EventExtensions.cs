using MakeEvent.Core.Domain.Entities;

namespace MakeEvent.Infrastructure.Service
{
    public static class EventExtensions
    {
        public static IEnumerable<Event> ExpandDaily(this IEnumerable<Event> events, DateTime firstDateOfMonth, DateTime lastDateOfMonth)
        {
            var expandedEvents = new List<Event>();

            foreach (var evt in events)
            {
                // Xác định ngày bắt đầu và ngày kết thúc để mở rộng
                var startDate = evt.StartTime < firstDateOfMonth ? firstDateOfMonth : evt.StartTime;
                var endDate = evt.EndTime.HasValue && evt.EndTime.Value < lastDateOfMonth
                    ? evt.EndTime.Value
                    : lastDateOfMonth;

                // Lặp qua từng ngày trong khoảng thời gian
                for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                {
                    expandedEvents.Add(new Event
                    {
                        Id = evt.Id, // Giữ nguyên ID nếu cần nhận dạng
                        Title = evt.Title + "(Lặp lại hàng ngày)",
                        Description = evt.Description,
                        StartTime = date,
                        EndTime = null,
                        Color = evt.Color,
                        IsCompleted = evt.IsCompleted,
                        CheckDays = evt.CheckDays,
                        LoopType = evt.LoopType
                    });
                }
            }

            return expandedEvents;
        }

        public static IEnumerable<Event> ExpandMonthly(this IEnumerable<Event> events, DateTime fisrtDateOfMonth)
        {
            return events.Select(monthlyEvent => new Event
            {
                Id = monthlyEvent.Id,
                Title = monthlyEvent.Title + "(Lặp lại hàng tháng)",
                Description = monthlyEvent.Description,
                StartTime = new DateTime(fisrtDateOfMonth.Year, fisrtDateOfMonth.Month, monthlyEvent.StartTime.Day),
                EndTime = new DateTime(fisrtDateOfMonth.Year, fisrtDateOfMonth.Month, monthlyEvent.StartTime.Day)
                    .AddHours(23).AddMinutes(59).AddSeconds(59),
                Color = monthlyEvent.Color,
                IsCompleted = monthlyEvent.IsCompleted,
                CheckDays = monthlyEvent.CheckDays,
                LoopType = monthlyEvent.LoopType
            });
        }

        public static IEnumerable<Event> ExpandYearly(this IEnumerable<Event> events, DateTime fisrtDateOfMonth)
        {
            return events
                .Where(yearlyEvent => yearlyEvent.StartTime.Month == fisrtDateOfMonth.Month)
                .Select(yearlyEvent => new Event
                {
                    Id = yearlyEvent.Id,
                    Title = yearlyEvent.Title + "(Lặp lại hàng năm)",
                    Description = yearlyEvent.Description,
                    StartTime = new DateTime(fisrtDateOfMonth.Year, fisrtDateOfMonth.Month, yearlyEvent.StartTime.Day),
                    EndTime = new DateTime(fisrtDateOfMonth.Year, fisrtDateOfMonth.Month, yearlyEvent.StartTime.Day)
                        .AddHours(23).AddMinutes(59).AddSeconds(59),
                    Color = yearlyEvent.Color,
                    IsCompleted = yearlyEvent.IsCompleted,
                    CheckDays = yearlyEvent.CheckDays,
                    LoopType = yearlyEvent.LoopType
                });
        }
    }

}
