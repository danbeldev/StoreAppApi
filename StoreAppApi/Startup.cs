using FastestDeliveryApi.database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using StoreAppApi.Auth;
using StoreAppApi.Mappings;
using StoreAppApi.Repository.company.logo;
using StoreAppApi.Repository.image;
using StoreAppApi.Repository.product.file;
using StoreAppApi.Repository.product.icon;
using StoreAppApi.Repository.product.image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StoreAppApi
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
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "cfif31.ru";
            builder.Port = 3306;
            builder.UserID = "ISPr24-39_BeluakovDS";
            builder.Password = "ISPr24-39_BeluakovDS";
            builder.Database = "ISPr24-39_BeluakovDS_StoreAppDatabase";
            builder.CharacterSet = "utf8";

            services.AddDbContext<EfModel>(o => o.UseMySql(builder.ConnectionString, ServerVersion.AutoDetect(builder.ConnectionString)));

            services.AddSingleton<ImageUserRepositoryImpl>();

            services.AddSingleton<IconProductRepositoryImpl>();

            services.AddTransient<FileProductRepository, FileProductRepositoryImpl>();

            services.AddTransient<ImageProductRepository, ImageProductRepositoryImpl>();

            services.AddTransient<LogoCompanyRepository, LogoCompanyRepositoryImpl>();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddControllers();

            services.AddControllers()
                .AddJsonOptions(
                    opt => opt.JsonSerializerOptions
                    .Converters.Add(new JsonStringEnumConverter()));


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudience = TokenBaseOptions.AUDIENCE,
                        ValidateIssuer = true,
                        ValidIssuer = TokenBaseOptions.ISSUER,
                        ValidateLifetime = true,
                        IssuerSigningKey = TokenBaseOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = false
                    };
                });



            services.AddSwaggerGen(c =>
            {
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme{
                        Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "bearerAuth"
                            }
                        }, new string []{}
                    }
                });

                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StoreAppApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StoreAppApi v1"));
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
