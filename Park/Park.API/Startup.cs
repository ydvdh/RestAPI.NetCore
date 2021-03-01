using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Park.API.Extensions;
using Park.API.Helpers;
using Park.Core.Interfaces;
using Park.Infra.Data;
using Park.Infra.Repository;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace Park.API
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
            services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrialRepository, TrialRepository>();
            services.AddAutoMapper(typeof(AutoMapperPark));
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerServiceExtensions>();
            services.AddSwaggerGen();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo 
            //    {
            //        Version = "v1",                   
            //        Title = "Park.API",
            //        Description = "Park App ASP.NET Core Web API",
            //        Contact = new OpenApiContact()
            //        {
            //            Email = "ydv.dh@gmail.com",
            //            Name = "ydv dh",
            //            Url = new Uri("https://wwww.ydv.com")
            //        },
            //        License = new OpenApiLicense()
            //        {
            //            Name = "IT License",
            //            Url = new Uri("https://en.wikipedia.org/wiki/IT_License")
            //        }
            //    });
            //    //c.SwaggerDoc("v1", new OpenApiInfo
            //    //{
            //    //    Version = "v1",
            //    //    Title = "Trail.API",
            //    //    Description = "Trail App ASP.NET Core Web API",
            //    //    Contact = new OpenApiContact()
            //    //    {
            //    //        Email = "ydv.dh@gmail.com",
            //    //        Name = "ydv dh",
            //    //        Url = new Uri("https://wwww.ydv.com")
            //    //    },
            //    //    License = new OpenApiLicense()
            //    //    {
            //    //        Name = "IT License",
            //    //        Url = new Uri("https://en.wikipedia.org/wiki/IT_License")
            //    //    }
            //    //});

            //    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    var cmlCommentFullpath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
            //    c.IncludeXmlComments(cmlCommentFullpath);
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Park.API v1"));
            //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trail.API v1"));
            app.UseSwaggerUI(options => {
                foreach (var desc in provider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                        desc.GroupName.ToUpperInvariant());
                options.RoutePrefix = "";
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
