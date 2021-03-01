﻿using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Park.API.Extensions
{
    public class SwaggerServiceExtensions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider;

        public SwaggerServiceExtensions(IApiVersionDescriptionProvider provider) => this.provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var desc in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    desc.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = $"Park API {desc.ApiVersion}",
                        Version = desc.ApiVersion.ToString()
                    });
            }
            var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var cmlCommentFullpath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
            options.IncludeXmlComments(cmlCommentFullpath);
        }
    }
}