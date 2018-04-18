//using NPSLCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using NPSLCore.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using NPSLCore.Repository;

namespace NPSLCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {

        private IDataRepository<Users, long> _iRepo;
        public UsersController(IDataRepository<Users, long> repo)
        {
            _iRepo = repo;
        }
        [HttpGet]
        public IEnumerable<Users> GetAll()
        {
            return _iRepo.GetAll();
        }

        [HttpGet("{id}")]
        public Users GetById(int id)
        {
            return _iRepo.GetById(id);
        }

        [HttpPost]
        public void Post([FromBody] Users item)
        {
             _iRepo.Add(item);
        }

        [HttpPut]
        public void  Update([FromBody] Users item)
        {
            _iRepo.Update( item);
        }

        [HttpDelete("{id}")]
        public long Delete(long id)
        {
            return _iRepo.Delete(id);
        }

    }
}