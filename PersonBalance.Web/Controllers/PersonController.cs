using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Web.Http;
using PersonBalance.Web.Models;
using PersonBalance.Web.Services;

namespace PersonBalance.Web.Controllers
{
    [RoutePrefix("api/Person")]
    public class PersonController : ApiController
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Register([FromBody] PersonRegisterModel person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Json(new {id = await _personService.CreateNewPerson(person.ToDto())});
        }

        [HttpPost]
        public async Task<IHttpActionResult> Balance(Guid id)
        {
            try
            {
                return Json(new {balance = await _personService.CheckBalance(id)});
            }
            catch (KeyNotFoundException e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("{id:guid}/changeBalance")]

        public async Task<IHttpActionResult> ChangeBalance(Guid id, decimal balance)
        {
            try
            {
                await _personService.ChangeBalance(id, balance);
                return Ok();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest();
            }
        }
    }
}
