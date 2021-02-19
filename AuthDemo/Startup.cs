using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AuthDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //services.AddControllers();

            #region 驗證
            //讀取配置文件
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            //驗證
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(option=> 
            {
                
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    AudienceValidator = (m, n, z) =>
                    {
                        string acc = m.FirstOrDefault().Split('|').FirstOrDefault();
                        bool status = m != null && m.FirstOrDefault().Equals(Const.ValidAudienceList.Where(x => x.Key == acc).FirstOrDefault().Value);
                        return m != null && m.FirstOrDefault().Equals(Const.ValidAudienceList.Where(x=>x.Key == acc).FirstOrDefault().Value);
                    },
                    ValidateIssuer = true,
                    ValidIssuer = audienceConfig["Issuer"],//發行人
                    //ValidAudience = audienceConfig["Audience"],//訂閱人
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                    IssuerSigningKey = signingKey,
                };
                option.Events = new JwtBearerEvents
                {
                    //OnAuthenticationFailed = context =>
                    //{
                    //    var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                    //    logger.LogError("Authentication failed.", context.Exception);
                    //    return Task.CompletedTask;
                    //},

                    //OnMessageReceived = context =>
                    //{
                    //    return Task.CompletedTask;
                    //},

                    OnChallenge = context =>
                    {
                        //自定義授權失敗
                        context.HandleResponse();
                        var payload = JsonConvert.SerializeObject(new { Code = "401", Message = "您無權訪問" });
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        context.Response.WriteAsync(payload);
                        return Task.FromResult(0);
                    }
                };

            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            //app.UseRouting();

            ////app.UseAuthorization();
            //app.UseAuthentication();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            app.UseRouting();
            app.UseAuthentication();
            //app.UseAuthorization();

          
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
