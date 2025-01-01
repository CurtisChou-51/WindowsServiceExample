using Microsoft.AspNetCore.Mvc;

namespace WindowsServiceExample
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Ok(new { alive = true });
        }
    }
}
