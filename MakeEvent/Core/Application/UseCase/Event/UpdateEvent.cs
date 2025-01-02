using MakeEvent.Core.Application.Interfaces;
using MakeEvent.Infrastructure.Service;

namespace MakeEvent.Core.Application.UseCase.Event
{
    public class UpdateEvent : UseCaseCRUD<Domain.Entities.Event>
    {
        public UpdateEvent(IRepository<Domain.Entities.Event> repository,
            HandleExceptionService handleExceptionService) :
            base(repository, handleExceptionService)
        {
        }

        public void Execute(int id, Domain.Entities.Event entity) {
            _handleExceptionService.ExecuteWithExceptionHandling(() =>
            { 
                var model = _repository.GetbyId(id);
                if (model != null)
                {
                    if (model.StartTime != entity.StartTime)
                    {
                        throw new InvalidOperationException("Không thể thay đổi thời gian bắt đầu");
                    }
                    if (model.EndTime != entity.EndTime) 
                    {
                        throw new InvalidOperationException("Không thể thay đổi thời gian kết thúc");
                    }
                    model.LoopType = entity.LoopType;
                    model.Title = entity.Title;
                    model.Description = entity.Description;
                    model.Color = entity.Color;
                    model.IsCompleted = entity.IsCompleted;
                }
                else
                {
                    throw new NullReferenceException("Không tìm thấy lịch trình này");
                }
                _repository.Update(model);
            });
        }
    }
}
