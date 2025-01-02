using Microsoft.AspNetCore.Mvc;

namespace MakeEvent.Controllers
{
    public class BaseController : Controller
    {
        protected IActionResult Execute(Func<IActionResult> action)
        {
            try
            {
                return action();
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
