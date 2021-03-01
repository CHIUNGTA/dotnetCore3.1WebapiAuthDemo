using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AuthDemo.Middelwares;
using Autofac;
using System.Data.SqlClient;
using System.Reflection;
using Model.ViewModel;
using System.Collections.Generic;
using NSwag.Generation.Processors.Security;

namespace AuthDemo
{
    public class Startup
    {


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            if (Const.ValidAudienceList == null)
            {
                Const.ValidAudienceList = new Dictionary<string, string>();
            }
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                //options.Filters.Add<AuthorizeFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            #region 
            //services.AddOpenApiDocument();
            services.AddSwaggerDocument(settings =>
            {
                settings.AddSecurity("輸入身份認證Token", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme()
                {
                    Description = "JWT認證 請輸入Bearer {token}",
                    Name = "Authorization",
                    In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                    Type = NSwag.OpenApiSecuritySchemeType.ApiKey
                });

                settings.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT Token"));
            });

            #endregion

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
            .AddJwtBearer(option =>
            {

                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    AudienceValidator = (m, n, z) =>
                    {
                        string acc = m.FirstOrDefault().Split('|').FirstOrDefault();
                        string value = string.Empty;
                        bool tokenKeyIsexist = Const.ValidAudienceList.TryGetValue(acc, out value);
                        if (tokenKeyIsexist)
                        {
                            bool status = m != null && m.FirstOrDefault().Equals(value);
                            return m != null && m.FirstOrDefault().Equals(value);
                        }
                        else
                        {
                            return false;
                        }
                    },
                    ValidateIssuer = true,
                    ValidIssuer = audienceConfig["Issuer"],//發行人
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                    IssuerSigningKey = signingKey,
                };
                option.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        //自定義驗證失敗回傳內容
                        context.HandleResponse();
                        string reponseBody = JsonConvert.SerializeObject(new ResponseModel<string> {StatsuCode=StatusCodes.Status401Unauthorized,  Data=string.Empty,  Message = "登入失敗，請登入後再進行嘗試" });
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        context.Response.WriteAsync(reponseBody);
                        return Task.FromResult(0);
                    }
                };

            });
            #endregion
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //builder.Register(x => new SqlConnection(Configuration.GetSection("SqlServer")["SqlServerConnection"]))
            //    .As<SqlConnection>()
            //    .InstancePerLifetimeScope();
            // 通過Autofac自動完成依賴注入
            var assemblies = new Assembly[]
            {
                Assembly.Load("AuthDemo"),
                Assembly.Load("Service") ,
                Assembly.Load("Repository")
            };
            //// 註冊Controller
            builder.RegisterAssemblyTypes(assemblies)
                .Where(x=> {
                    Console.WriteLine(x.Name);
                    return (x.Name.EndsWith("Service") || x.Name.EndsWith("Repository")) && !x.IsInterface;
                })
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
            builder.Register(x => new SqlConnection(Configuration.GetSection("SqlServer")["SqlServerConnection"]))
                .As<SqlConnection>()
                .InstancePerLifetimeScope();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseMiddleware<MyMiddlewares>();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();
           
        }


    }
}
