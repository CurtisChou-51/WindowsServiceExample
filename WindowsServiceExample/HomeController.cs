using Microsoft.AspNetCore.Mvc;
using WindowsServiceExample.MainSchedule;

namespace WindowsServiceExample
{
    public class HomeController : Controller
    {
        private readonly MainBackgroundService _mainBackgroundService;

        public HomeController(MainBackgroundService mainBackgroundService)
        {
            _mainBackgroundService = mainBackgroundService;
        }

        public IActionResult Index()
        {
            return Ok(new { alive = true });
        }

        public async Task<IActionResult> JobsStatus()
        {
            var result = await _mainBackgroundService.QueryJobsStatus();
            return Ok(result);
        }

        public IActionResult TriggerJob(string jobIdentity)
        {
            _mainBackgroundService.TriggerJob(jobIdentity);
            return Ok();
        }
    }
}
