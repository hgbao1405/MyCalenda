using MakeEvent.Core.Application.Interfaces;
using MakeEvent.Infrastructure.Service;

namespace MakeEvent.Core.Application.UseCase.Event
{
    public class GetListEvent : UseCaseCRUD<Domain.Entities.Event>
    {
        public GetListEvent(IRepository<Domain.Entities.Event> repository,
            HandleExceptionService handleExceptionService) :
            base(repository, handleExceptionService)
        {
        }

        public List<Domain.Entities.Event> Execute(int? year, int? month) {
            return _handleExceptionService.ExecuteWithExceptionHandling(() =>
            {
                if (year == null)
                {
                    year = DateTime.Now.Year;
                }
                if (month == null)
                {
                    month = DateTime.Now.Month;
                }
                var date = new DateTime(year.Value, month.Value, 1);
                return _repository.GetAll(date);
            });
        }
    }
}
