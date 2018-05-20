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
        private readonly IAppAuthorizer _appAuthorizer;

        public AuthorizationController(IAppAuthorizer appAuthorizer)
        {
            _appAuthorizer = appAuthorizer;
        }

        [HttpGet]
        public IActionResult GetCodeForAccessToken([FromBody] string code)
        {
            _appAuthorizer.SendRequestToGetAccessToken();
            return Ok();
        }
    }
}