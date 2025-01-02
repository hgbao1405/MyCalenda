using MakeEvent.Core.Application.Interfaces;
using MakeEvent.Core.Domain.Entities;
using MakeEvent.Infrastructure.Service;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace MakeEvent.Infrastructure.Persistence
{
    internal class EventRepostitory : IRepository<Event>
    {
        private readonly MakeEventContext _context;
        private readonly HandleExceptionService _handleExceptionService;
        public EventRepostitory(MakeEventContext context,
            HandleExceptionService handleExceptionService)
        {
            _context = context;
            _handleExceptionService = handleExceptionService;
        }

        public void Create(Event model)
        {
            _handleExceptionService.ExecuteWithExceptionHandling(() =>
            {
                _context.Add(model);
                _context.SaveChanges();
            });
        }

        public void Delete(int id)
        {
            _handleExceptionService.ExecuteWithExceptionHandling(() =>
            {
                var model = _context.Event.Find(id);
                _context.Remove(model);
                _context.SaveChanges();
            });
        }

        public List<Event> GetAll(DateTime? searchDate)
        {
            return _handleExceptionService.ExecuteWithExceptionHandling(() =>
            {
                if (searchDate == null)
                {
                    searchDate = DateTime.Now.Date;
                }
                var fisrtDateOfMonth = searchDate.Value;
                fisrtDateOfMonth.AddDays(-fisrtDateOfMonth.Day + 1);
                var lastDateOfMonth = fisrtDateOfMonth.AddMonths(1).AddDays(-1);

                var searchNoLoop = _context.Event
                .Where(x=>x.LoopType=="None" 
                && x.StartTime >= fisrtDateOfMonth && x.StartTime <= lastDateOfMonth).AsEnumerable();

                //Cái này xuất hiện mỗi ngày nên điều kiện là:
                var searchLoopDaily = _context.Event
                .Where(x => x.LoopType == "Daily"
                && (
                    (x.StartTime <= lastDateOfMonth && (x.EndTime == null || x.EndTime >= fisrtDateOfMonth))
                    || (x.StartTime >= fisrtDateOfMonth && x.StartTime <= lastDateOfMonth)
                    || (x.EndTime==null || (x.EndTime.Value.Date >= x.StartTime.Date 
                        && (x.EndTime.Value >= fisrtDateOfMonth && x.EndTime.Value <= lastDateOfMonth)))
                )).AsEnumerable();

                //cái này chắc chắn xuất hiện đúng 1 lần mỗi tháng
                var searchhMonthly = _context.Event
                .Where(x => x.LoopType == "Monthly"
                && (
                    //Tháng này nằm giữa ngày bắt đầu và kết thúc.
                    (x.StartTime <= lastDateOfMonth && (x.EndTime == null || x.EndTime >= fisrtDateOfMonth))
                    //Ngày bắt đầu nằm giữa tháng.
                    || (x.StartTime >= fisrtDateOfMonth && x.StartTime <= lastDateOfMonth)
                    //Ngày kết thúc lớn hơn hoặc bằng ngày bắt đầu
                    || (x.EndTime==null || (x.EndTime.Value.Date >= x.StartTime.Date
                    //và ngày kết thúc nằm giữa tháng này.
                    && ((x.EndTime.Value >= fisrtDateOfMonth && x.EndTime.Value <= lastDateOfMonth))))
                )).AsEnumerable();

                //Cái này chỉ xuất hiện mỗi năm 1 lần
                //nên kiểm tra tháng của ngày bắt đầu có bằng tháng này không
                var searchLoopYearly = _context.Event
                .Where(x => x.LoopType.Equals("Yearly") 
                && x.StartTime.Month == fisrtDateOfMonth.Month 
                && (x.EndTime!=null || (x.EndTime >= fisrtDateOfMonth && x.EndTime <= lastDateOfMonth)))
                .AsEnumerable();

                // Tách và mở rộng từng loại sự kiện
                var expandedDailyEvents = searchLoopDaily.ExpandDaily(fisrtDateOfMonth, lastDateOfMonth);
                var expandedMonthlyEvents = searchhMonthly.ExpandMonthly(fisrtDateOfMonth);
                var expandedYearlyEvents = searchLoopYearly.ExpandYearly(fisrtDateOfMonth);

                // Gộp tất cả sự kiện lại thành danh sách cuối cùng
                return searchNoLoop
                    .Concat(expandedDailyEvents)
                    .Concat(expandedMonthlyEvents)
                    .Concat(expandedYearlyEvents)
                    .OrderBy(e => e.StartTime)
                    .ToList();

            });
        }

        public Event GetbyId(int id)
        {
            return _handleExceptionService.ExecuteWithExceptionHandling(() =>
            {
                return _context.Event.Find(id);
            });
        }

        public void Update(Event model)
        {
            _handleExceptionService.ExecuteWithExceptionHandling(() =>
            {
                _context.Event.Update(model);
                _context.SaveChanges();
            });
        }

    }
}
