using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfflineMessaging.Api.Services.Block;
using OfflineMessaging.Api.Services.Message;
using OfflineMessaging.Api.Services.Token;
using OfflineMessaging.Api.Services.User;
using OfflineMessaging.Infrastructure.Context;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;

namespace OfflineMessaging.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddScoped<ITokenServices, JwtTokenServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ICrudUserServices, CrudUserServices>();
            services.AddScoped<ICheckUserServices, CheckUserServices>();
            services.AddScoped<IMessageServices, MessageServices>();
            services.AddScoped<ICrudMessageServices, CrudMessageServices>();
            services.AddScoped<IBlockServices, BlockServices>();
            services.AddScoped<ICrudBlockServices, CrudBlockServices>();

            var connectionString = Configuration["Db:ConnectionString"];
            services.AddDbContext<OfflineMessagingContext>(options => options.UseSqlServer(connectionString));

            var secretKey = Configuration.GetValue<string>("App:JwtSecretKey");
            var symmetricKey = Convert.FromBase64String(secretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Offline Messaging Api",
                    Version = "v1",
                    Description = "This api provides Offline Messaging Api services",
                    Contact = new OpenApiContact
                    {
                        Name = "Ezgi Peker",
                        Email = "ezgi.peker.6@gmail.com",
                        Url = new Uri("https://github.com/ezgipeker")
                    }
                });
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "OfflineMessaging.Api.xml");
                c.IncludeXmlComments(xmlPath, true);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog(Log.Logger);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                x.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            DatabaseHelper.PrepareDatabase(app);
        }
    }
}
