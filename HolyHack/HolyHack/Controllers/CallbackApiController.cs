using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Domain.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HolyHack.Controllers
{
    [Route("api")]
    public class CallbackApiController : Controller
    {
        private readonly ILogger _logger;
        private readonly IVkCallbackApiHandler _requestHandler;

        public CallbackApiController(ILogger<CallbackApiController> logger, IVkCallbackApiHandler requestHandler)
        {
            _logger = logger;
            _requestHandler = requestHandler;
        }

        [HttpPost]
        public IActionResult Handle([FromBody] CallbackRequest callbackRequestData)
        {
            _logger.LogInformation("received",new []{JsonConvert.SerializeObject(callbackRequestData)});
            return Ok(_requestHandler.Handle(callbackRequestData));
            
        }
    }

    public class ConfirmationData
    {
        public string type { get; set; }
        public int group_id { get; set; }
    }
}