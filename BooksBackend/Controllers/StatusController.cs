using BooksBackend.Models.Employees;
using BooksBackend.Models.Status;
using BooksBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BooksBackend.Controllers
{
    public class StatusController : ControllerBase
    {

        private ISystemTime _systemTime;

        public StatusController(ISystemTime systemTime)
        {
            _systemTime = systemTime;
        }

        // GET /status
        [HttpGet("status")]
        public ActionResult GetStatus()
        {
            var response = new GetStatusResponse
            {
                CheckedBy = "Joe",
                Message = "Looks Good",
                LastChecked = _systemTime.GetCurrent()
            };
            return Ok(response);
        }

        // Url - Route Params
        [HttpGet("products/{category}/{productid:int}")]
        public ActionResult GetProduct(string category, int productid)
        {
            return Ok($"That is in the category of {category} and product id {productid}.");
        }

        [HttpGet("employees")]
        public ActionResult GetEmployeesInDepartment([FromQuery] string department = "all")
        {
            return Ok($"Giving you all employees in {department}.");
        }

        // useful for logging, but not much else
        [HttpGet("whoami")]
        public ActionResult WhoAmI([FromHeader(Name ="User-Agent")] string userAgent)
        {
            return Ok($"User Agent: {userAgent}.");
        }

        [HttpPost("employees")]
        public ActionResult Hire([FromBody] PostEmployeeCreate request)
        {
            return Ok(request);
        }
    }
}
