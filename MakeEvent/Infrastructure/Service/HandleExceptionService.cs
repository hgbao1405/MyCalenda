using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MakeEvent.Infrastructure.Service
{
    public class HandleExceptionService
    {
        public void ExecuteWithExceptionHandling(Action action)
        {
            try
            {
                action();
            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra! " + ex.Message);
            }
        }
        public T ExecuteWithExceptionHandling<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra! "+ex.Message);
            }
        }
    }
}
