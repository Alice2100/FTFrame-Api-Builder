using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using System.Threading.Tasks;
using FTFrame.Tool;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FTFrame.Project.Core.Utils;
using Microsoft.AspNetCore.Http;
using FTFrame.Base.Core;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.DataProtection;
using System.IO;

namespace FTFrame.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            if (System.IO.File.Exists(SysConst.RootPath + Path.DirectorySeparatorChar + "FTFrame.Dynamic.Core.dll"))
            {
                using (var stream = System.IO.File.OpenRead(SysConst.RootPath + Path.DirectorySeparatorChar + "FTFrame.Dynamic.Core.dll"))
                {
                    AssemblyLoadContext.Default.LoadFromStream(stream);
                }
            }

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Jwt add:
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,//是否验证Issuer
            //            ValidateAudience = true,//是否验证Audience
            //            ValidateLifetime = true,//是否验证失效时间
            //            ClockSkew = TimeSpan.FromMinutes(Jwt.ExpiresMinutes),
            //            ValidateIssuerSigningKey = true,//是否验证SecurityKey
            //            ValidAudience = Jwt.Domin,//Audience
            //            ValidIssuer = Jwt.Domin,//Issuer，这两项和前面签发jwt的设置一致
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt.SecretKey))//拿到SecurityKey
            //        };
            //        options.Events = new JwtBearerEvents
            //        {
            //            OnChallenge = context =>
            //            {
            //                context.HandleResponse();
            //                var payload = JsonConvert.SerializeObject(new { code = 401, message = context.Error, status = false });
            //                context.Response.ContentType = "application/json";
            //                context.Response.StatusCode = StatusCodes.Status200OK;
            //                context.Response.WriteAsync(payload);
            //                return Task.FromResult(0);
            //            }
            //        };
            //    });


            services.AddRazorPages(options =>
            {
                Project.Core.ProjectFilter.Create(options, Configuration);
            }).AddMvcOptions(options =>
            {
                options.Filters.Add(new Project.Core.ProjectPageFilter(Configuration));
            })
                .AddRazorRuntimeCompilation()
            ;
            var mvcBuilder = services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddMvc(o => o.Filters.Add(typeof(Project.Core.ProjectAsyncActionFilter))).AddRazorPagesOptions(o =>
              {
                  o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
              });
            services.AddSingleton<DBClient.DB>();
            services.AddSingleton<IHostedService, Project.Core.Service.JobService>();
            //services.AddDbContext<Project.Model.Demo.DemoContext>(options =>
            //    options.UseSqlServer(SysConst.ConnString));
            //services.AddMvc()
            //    .AddRazorPagesOptions(options =>
            //{
            //    //options.RootDirectory = "/Pages";
            //    Server.Core.Api.MapPageRoute(options);
            //});
            services.AddControllers(o => o.Filters.Add(typeof(Project.Core.ProjectAsyncActionFilter)));
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10); //session活期时间
                options.Cookie.HttpOnly = true;//设为httponly
            });
            //services.AddDistributedMemoryCache();
            //services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "DataProtection"));
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddScoped<FTFrame.Base.Core.UserBase>();

            //配置EF的服务注册 若需要用到EF的话，Model通过EF Tool自动生成
            //services.AddDbContext<Project.Model.Demo.DemoContext>(options =>
            //    options.UseSqlServer(FTFrame.SysConst.ConnString)
            //    );
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.AllowAnyMethod()
                    //.WithOrigins(new string[] { "http://127.0.0.1:5000" })
                    .SetIsOriginAllowed((x) => {
                        if (SysConst.OriginAllowed.Contains("*")) return true;
                        string ori = x == null ? "null" : x;
                        return SysConst.OriginAllowed.Contains(ori);
                    })
                    .AllowAnyHeader()
                    .AllowCredentials();

                });

            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var fileExtProvider = new FileExtensionContentTypeProvider();
            fileExtProvider.Mappings[".vue"] = "text/plain";
            fileExtProvider.Mappings[".dmg"] = "application/octet-stream";

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = fileExtProvider
            });
            app.UseSession();//需增加Redis作为Session存储
            app.UseRouting();
            app.UseCors("any");
            app.UseAuthentication();
            app.UseAuthorization();
            FTHttpContext.Configure(app.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var exception = context.Features.Get<IExceptionHandlerFeature>();
                    if (exception != null)
                    {
                        var error = new
                        {
                            Stacktrace = exception.Error.StackTrace,
                            Message = exception.Error.Message
                        };
                        var errObj = JsonConvert.SerializeObject(error);

                        await context.Response.WriteAsync(errObj).ConfigureAwait(false);
                    }
                });
            }
         );
        }
    }
}
