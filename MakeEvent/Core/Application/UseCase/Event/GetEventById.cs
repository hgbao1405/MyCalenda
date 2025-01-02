using MakeEvent.Core.Application.Interfaces;
using MakeEvent.Core.Domain.Entities;
using MakeEvent.Infrastructure.Service;

namespace MakeEvent.Core.Application.UseCase.Event
{
    public class GetEventById : UseCaseCRUD<Domain.Entities.Event>
    {
        public GetEventById(IRepository<Domain.Entities.Event> repository, HandleExceptionService handleExceptionService) : base(repository, handleExceptionService)
        {
        }

        public Domain.Entities.Event Execute(int id)
        {
            return _handleExceptionService.ExecuteWithExceptionHandling(() =>
            {
                var model=_repository.GetbyId(id);
                if (model == null) {
                    throw new NullReferenceException("Không tìm thấy lịch trình này");
                }
                return model;
            });
        }
    }
}
