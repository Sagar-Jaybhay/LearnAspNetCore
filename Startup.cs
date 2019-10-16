using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnAspCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using  Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
namespace LearnAspCore
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                option.Filters.Add(new AuthorizeFilter(policy));

            });
            services.AddTransient<IStudentRepository,StudentRepo>();
            services.AddDbContextPool<OurDbContext>(options => options.UseSqlServer(_config.GetConnectionString("StudentDBString")));


            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 12;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 5;

            });
            services.AddIdentity<ExtendedIdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<OurDbContext>();

            services.AddScoped<IStudentRepository, SQLStudentRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            DeveloperExceptionPageOptions pageOptions = new DeveloperExceptionPageOptions {SourceCodeLineCount = 10};
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage(pageOptions);
            //}
            //else
            {

                app.UseExceptionHandler("/Error");
                //app.UseStatusCodePagesWithReExecute("/Error/{0}");
                //  app.UseStatusCodePagesWithRedirects("/Error/{0}");
            }
            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseHsts();
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
                //routes.MapRoute(name: "default", template: "sagar/{controller=Home}/{action=Index}/{id?}");
            });
            
        }
    }
}


// app.UseMvc();

//app.Run(async (context) => 
//{
////    throw new Exception("Error occured");
//    await context.Response.WriteAsync("Hello Sagar Jaybhay3");
//});








//DefaultFilesOptions defaultFiles = new DefaultFilesOptions();
//defaultFiles.DefaultFileNames.Clear();
//defaultFiles.DefaultFileNames.Add("demo.html");
//app.UseDefaultFiles(defaultFiles);
//app.UseStaticFiles();

//FileServerOptions fileServer = new FileServerOptions();
//fileServer.DefaultFilesOptions.DefaultFileNames.Clear();
//            fileServer.DefaultFilesOptions.DefaultFileNames.Add("demo.html");

//            app.UseFileServer(fileServer);

//app.Use(async (context, next) =>
//            {
//                await context.Response.WriteAsync("Hello Sagar Jaybhay");
//await next();
//            });