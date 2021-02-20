using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthDemo.Middelwares
{
    public class MyMiddlewares
    {
        private readonly RequestDelegate _next;


        public MyMiddlewares(RequestDelegate next)
        {
            this._next = next;
        }

        /// <summary>
        /// 授權失敗者，會在此取得訊息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            await _next(context);
            if(context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                var responseBody = JsonConvert.SerializeObject(new { Message = "您無權訪問該內容" });
                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(responseBody);
            }
        }
    }
}
