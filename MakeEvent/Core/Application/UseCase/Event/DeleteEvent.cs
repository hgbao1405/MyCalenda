using MakeEvent.Core.Application.Interfaces;
using MakeEvent.Infrastructure.Service;

namespace MakeEvent.Core.Application.UseCase.Event
{
    public class DeleteEvent : UseCaseCRUD<Domain.Entities.Event>
    {
        public DeleteEvent(IRepository<Domain.Entities.Event> repository,
            HandleExceptionService handleExceptionService) :
            base(repository, handleExceptionService)
        {
        }

        public void Execute(int id) {
            _handleExceptionService.ExecuteWithExceptionHandling(() =>
            {
                var model = _repository.GetbyId(id);
                if (model != null)
                {
                    _repository.Delete(id);
                }
                else
                {
                    throw new NullReferenceException("Không tìm thấy lịch trình này");
                }
            });
        }
    }
}
