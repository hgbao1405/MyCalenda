using MakeEvent.Core.Application.Interfaces;
using MakeEvent.Core.Domain.Entities;
using MakeEvent.Infrastructure.Service;

namespace MakeEvent.Core.Application.UseCase.Event
{
    public class AddEvent : UseCaseCRUD<Domain.Entities.Event>
    {
        public AddEvent(IRepository<Domain.Entities.Event> repository,
            HandleExceptionService handleExceptionService) : 
            base(repository,handleExceptionService)
        {
        }

        public void Execute(Domain.Entities.Event entity) {
            _handleExceptionService.ExecuteWithExceptionHandling(() =>
            {
                _repository.Create(entity);
            });
        }
    }
}
