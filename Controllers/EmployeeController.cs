using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Models;
using ProductManagementApi.Services;

namespace ProductManagementApi.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private IUserService _userService;
        private ApplicationDbContext _context;


        public EmployeeController(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Properties requested");
            }
            else
            {
                var result = await _userService.RegisterUserAsync(model);

                
                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("User not found");
            }
            else
            {
                var result = await _userService.LoginUserAsync(model);

                if (result.IsSuccess)
                    return Ok(result);
                return BadRequest(result);
            }
        }


        [HttpPost("AddAdmin")]
        public  ActionResult RegisterAdmin([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Some parameters are not complete");
            }
            else
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();

                return Created(HttpContext.Request.Scheme + "//" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + employee.Id, employee);
            }
        }
    }
}
