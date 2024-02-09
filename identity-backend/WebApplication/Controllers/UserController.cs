using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public sealed class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        
        public UserController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<ApplicationUser>> FindByName(string name)
        {
            return this.Ok(await this.userManager.FindByNameAsync(name));
        }
    }
}