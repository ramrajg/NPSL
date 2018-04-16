using NPSLCore.Models;
using NPSLCore.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace NPSLCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        public IUsersRepository UsersRepo { get; set; }
        public UsersController(IUsersRepository _repo)
        {
            UsersRepo = _repo;
        }
        [HttpGet]
        public IEnumerable<Users> GetAll()
        {
            return UsersRepo.GetAll();
        }

        [HttpGet("{id}", Name = "GetContacts")]
        public IActionResult GetById(string id)
        {
            var item = UsersRepo.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Users item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            UsersRepo.Add(item);
            return CreatedAtRoute("GetContacts", new { Controller = "Contacts", id = item.MobilePhone }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Users item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            var contactObj = UsersRepo.Find(id);
            if (contactObj == null)
            {
                return NotFound();
            }
            UsersRepo.Update(item);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            UsersRepo.Remove(id);
        }

    }
}