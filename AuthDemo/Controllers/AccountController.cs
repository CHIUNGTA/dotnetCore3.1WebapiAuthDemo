using AuthDemo.Helper;
using Interface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.SqlServerInfo;
using Model.ViewModel;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Utility;

namespace AuthDemo.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccount_Service _accountService;
        public AccountController(IAccount_Service accountService)
        {
            if(this._accountService  == null)
            {
                this._accountService = accountService;
            }
           
        }

        

        [HttpPost]
        [Route("singup")]
        public ActionResult<ResponseModel<string>> Singup([FromBody]User user)
        {
            if (String.IsNullOrEmpty(user.LoginName) || String.IsNullOrEmpty(user.LoginPassword))
            {
                return new ResponseModel<string>() { Message = "請輸入完整資料", StatsuCode = StatusCodes.Status204NoContent, Data = null }; 
            }
            user.LoginPassword = MyEncryption.Encryption_sha256(user.LoginPassword);
            var result = this._accountService.Singup(user);
            return result; 
        }

        [HttpGet]
        [Route("login")]
        //[SwaggerResponse(HttpStatusCode.OK, Description = "OKOK", Type = typeof(ResponseModel<string>))]
        [ProducesResponseType(typeof(ResponseModel<LoginModel>),200)]
        [ProducesResponseType(typeof(ResponseModel<string>), 500)]
        public ActionResult<ResponseModel<LoginModel>> GetJWTToken(string acc, string pws)
        {

            try
            {
                var _user = HttpContext.User;
                string jwtStr = string.Empty;
                bool suc = false;

                string tempToken = $"{acc}|{pws}|{DateTime.Now}";
                this.TokenReflash(tempToken);



                //TODO 後面串聯DB取得帳號驗證資料與使用者相關資料

                #region 範例(模擬資料庫)
                if (string.IsNullOrEmpty(acc) || string.IsNullOrEmpty(pws))
                {
                    return new JsonResult(new
                    {
                        Status = false,
                        message = "密碼不可為空值"
                    });
                }

                TokenModelJWT tokenModel = new TokenModelJWT();
                tokenModel.Uid = 1;
                if (acc.ToUpper() != "TEST")
                {
                    tokenModel.Role = "Client";
                }
                else
                {
                    tokenModel.Role = "Admin";
                }
                tokenModel.Temp = tempToken;

                jwtStr = JwtHelper.IssueJWT(tokenModel);
                suc = true;
                #endregion
                return new ResponseModel<LoginModel>() { Message = "", StatsuCode = 200, Data = new LoginModel() { success = suc, token = jwtStr } };
            }
            catch (Exception ex)
            {
                return new ResponseModel<LoginModel>() { Message = "發生不可知錯誤", StatsuCode = 200};
            }

        }

        /// <summary>
        /// Token集合更新
        /// </summary>
        /// <param name="_validAudience"></param>
        private void TokenReflash(string _validAudience)
        {
            //取得使用者帳號
            string acc = _validAudience.Split('|')[0];
            //查看暫存空間內是否有該帳號的token
            if (Const.ValidAudienceList.Where(x => x.Key == acc).Any())
            {
                //更新 _validAudience
                //Const.ValidAudienceList = Const.ValidAudienceList.Where(x => x.Key == user).Select(x => { x.Value = _validAudience; return x; }).ToList();
                Const.ValidAudienceList[acc] = _validAudience;
            }
            else
            {
                //集合中沒有的加入LIST中
                //Const.ValidAudienceList.Add(new ValidAudienceData() { UserName = user, ValidAudience = _validAudience });
                Const.ValidAudienceList.Add(acc, _validAudience);
            }

        }
    }
}
