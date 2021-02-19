using AuthDemo.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthDemo.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public TestController()
        {
            if (Const.ValidAudienceList == null)
            {
                Const.ValidAudienceList = new Dictionary<string, string>();
            }
        }
        public string Get()
        {
            return JsonConvert.SerializeObject(new { Code = "401", Message = "很抱歉，您无权访问该接口；Jason.Song（成长的小猪）写了一个JWT权限验证失败后自定义返回Json数据对象，来源：https://blog.csdn.net/jasonsong2008" });
        }


        [HttpGet]
        [Route("login")]
        public async Task<object> GetJWTToken(string acc, string pws)
        {
            var _user = HttpContext.User;
            string jwtStr = string.Empty;
            bool suc = false;

            string tempToken = $"{acc}|{pws}|{DateTime.Now}";
            this.TokenReflash(tempToken);



            //TODO 後面串聯DB取得帳號驗證資料與使用者相關資料

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


            return Ok(new
            {
                success = suc,
                token = jwtStr
            });
        }


        //[HttpPost]
        //public async void Logout()
        //{
        //    _logger.LogInformation("User {Name} logged out at {Time}.",
        //        User.Identity.Name, DateTime.UtcNow);

        //    #region snippet1
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    #endregion

        //}

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
