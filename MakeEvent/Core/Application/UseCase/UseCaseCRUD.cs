using MakeEvent.Core.Application.Interfaces;
using MakeEvent.Infrastructure.Service;

namespace MakeEvent.Core.Application.UseCase
{
    public class UseCaseCRUD<T>
    {
        protected IRepository<T> _repository;
        protected HandleExceptionService _handleExceptionService;

        public UseCaseCRUD(IRepository<T> repository,
            HandleExceptionService handleExceptionService)
        {
            _repository = repository;
            _handleExceptionService = handleExceptionService;
        }
    }
}