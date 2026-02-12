using API.Data;
using API.Entitties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
   
    public class MembersController(AppDbContext context) : BaseAPIController
    {
        [HttpGet]
        public ActionResult<IReadOnlyList<AppUser>> GetMembers()
        {
            var members = context.Users.ToList();
            return members;
        }
         [Authorize]
        [HttpGet("{id}")]
        public ActionResult <AppUser?>Getmember(string id)
        {
            var member = context.Users.Find(id);
            return member;
        }
    }
}
