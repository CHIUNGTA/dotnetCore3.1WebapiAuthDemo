using Autofac;
using Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.SqlServerInfo;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace AuthDemo.Controllers
{
    [Route("api/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IAccount_Service _loginService;
        public ValuesController(IAccount_Service login_Object)
        {
            if (_loginService == null)
            {
                this._loginService = login_Object;
            }
        }

        [HttpGet]
        public object Get()
        {
            var data = this._loginService.GetAll();
            return data;
        }

        [HttpGet]
        [Route("byid")]
        public object GetById(int id)
        {
            return this._loginService.GetById(id);
        }

        [HttpGet]
        [Route("v1")]
        [Authorize(Roles = "Admin,Client")]
        public ActionResult<IEnumerable<string>> GetAdimnAndClient()
        {
            return new string[] { "admin", "client" };
        }

        [HttpPost]
        [Route("singup")]
        public ResponseModel Singup([FromBody] User user)
        {
            if (String.IsNullOrEmpty(user.LoginName) || String.IsNullOrEmpty(user.LoginPassword))
            {
                return new ResponseModel() { Message = "請輸入完整資料", StatsuCode = StatusCodes.Status204NoContent, Data = null };
            }
            user.LoginPassword = MyEncryption.Encryption_sha256(user.LoginPassword);

            return this.Singup(user);
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
    }
}
