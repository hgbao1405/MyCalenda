using MakeEvent.Core.Domain.Entities;

namespace MakeEvent.Core.Application.Interfaces
{
    public interface IRepository<T>
    {
        void Create(T model);
        void Update(T model);
        void Delete(int id);
        List<T> GetAll(DateTime? searchDate);
        T GetbyId(int id);
    }
}