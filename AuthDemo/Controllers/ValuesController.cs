using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthDemo.Controllers
{
    [Route("api/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "non role";
        }

        [HttpGet]
        [Route("v1")]
        [Authorize(Roles = "Admin,Client")]
        public ActionResult<IEnumerable<string>> GetAdimnAndClient()
        {
            return new string[] { "admin", "client" };
        }


        [HttpGet]
        [Route("v2")]
        [Authorize(Roles = "Client")]
        public ActionResult<IEnumerable<string>> GetClient()
        {
            return new string[] { "Client" };
        }


        [HttpGet]
        [Route("v3")]
        [Authorize(Roles ="Admin")]
        public ActionResult<IEnumerable<string>> GetAdmin()
        {
            return new string[] { "Admin"}; 

        }

        //        [HttpGet]
        //        [Route("Test2")]
        //        [Authorize]
        //        public string Get2()
        //        {
        //bool status=             HttpContext.User.Identity.IsAuthenticated;
        //            return "Test2";
        //        }

        //        [HttpGet]
        //        [Route("UU")]
        //        [Authorize]
        //        public string UU()
        //        {
        //            return "admin";
        //        }


    }
}
