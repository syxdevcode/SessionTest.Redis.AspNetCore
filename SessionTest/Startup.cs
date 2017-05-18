using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace SessionTest
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            //抽取key-xxxxx.xml 
            //services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"D:\www"));

            services.AddDistributedRedisCache(option =>
            {
                //redis 数据库连接字符串
                option.Configuration = Configuration.GetConnectionString("RedisConnection");
                //redis 实例名
                option.InstanceName = Configuration.GetConnectionString("InstanceName");
            });


            // 1.自定义用户机密
            services.AddSingleton<IXmlRepository, CustomXmlRepository>();

            
            services.AddDataProtection(configure =>
            {
                configure.ApplicationDiscriminator = "SessionTest.Web";
            });
            // 2.存储到Redis服务器
            //.PersistKeysToDistributedStore();


            //Session 过期时长分钟
            var sessionOutTime = Configuration.GetValue<int>("SessionTimeOut", 30);

            //添加Session并设置过期时长
            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(sessionOutTime); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
