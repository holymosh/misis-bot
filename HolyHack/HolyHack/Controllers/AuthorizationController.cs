using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HolyHack.Controllers
{
    [Produces("application/json")]
    [Route("oauth")]
    public class AuthorizationController : Controller
    {

        public AuthorizationController()
        {
        }

        [HttpGet]
        public IActionResult GetCodeForAccessToken([FromBody] string code)
        {
            return Ok();
        }
    }
}