using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoCommon;
using DemoCommon.Models;
using Microsoft.EntityFrameworkCore;
using DemoService;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DemoService.Helpers;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace DemoAPI2
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
            services.AddCors();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ApplicationDbContextConnection")));
            var jwtSetting = Configuration.GetSection("JWTSetting");
           
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IFileService, FileService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async (context) =>
                    {
                        //var accessToken = context.SecurityToken as JwtSecurityToken;
                        //var userId = accessToken.Claims.FirstOrDefault(c => c.Type == AuthConstants.USER_ID_CLAIM_TYPE);

                        //var validateTokenService = context.HttpContext.RequestServices.GetRequiredService<IValidateTokenService>();

                        //var isRevoked = await validateTokenService.IsUserLoginRevokedAsync(int.Parse(userId.Value), accessToken.RawData);
                        //if (isRevoked)
                        //{
                        //    throw new CustomException("1000", "Unauthorized", HttpStatusCode.Unauthorized);
                        //}

                        //var userStatus = await validateTokenService.GetUserStatusAsync(int.Parse(userId.Value));

                        //if (userStatus == null || userStatus.Value == UserStatus.Closed)
                        //{
                        //    throw new CustomException(Errors.USER_CLOSED, Errors.USER_CLOSED_MSG, HttpStatusCode.Unauthorized);
                        //}
                    }
                };

                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    // The signing key must match!
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("d0dd4162d3ab48b7adae097b755e671e7411")),

                    // Validate the JWT Issuer (iss) claim
                    ValidateIssuer = true,
                    ValidIssuer = "jwtIssuer",

                    // Validate the JWT Audience (aud) claim
                    ValidateAudience = true,
                    ValidAudience = "jwtAudience",

                    // Validate the token expiry
                    ValidateLifetime = true,

                    // If you want to allow a certain amount of clock drift, set that here:
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });


            services.AddControllers();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Demo api",
                    Description = "Demo api",
                });
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Here enter JWT Token with bearer format like bearer [space] token"
                });
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                            Scheme="oauth2",
                            Name="Bearer",
                            In=ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Showing API V1");
                });
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //Path.Combine(env.WebRootPath, "images")),
            //    RequestPath = "/Resources"
            //});
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(options => options
             .WithOrigins(new[] { "http://localhost:3000", "http://localhost:8080", "http://localhost:4200","http://localhost:3001" })
                .AllowAnyHeader()
                .AllowAnyMethod()
               
            );
           
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
