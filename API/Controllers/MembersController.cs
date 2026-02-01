using API.Data;
using API.Entitties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]// localhost:5001/api/Members
    [ApiController]
    public class MembersController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IReadOnlyList<AppUser>> GetMembers()
        {
            var members = context.Users.ToList();
            return members;
        }
        [HttpGet("{id}")]
        public ActionResult <AppUser?>Getmember(string id)
        {
            var member = context.Users.Find(id);
            return member;
        }
    }
}
