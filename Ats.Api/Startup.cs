using System;
using System.IO;
using System.Reflection;
using System.Text;
using Ats.Data;
using Ats.Data.EntityFramework;
using Ats.Domain.Identity.Models;
using Ats.Security.Identity;
using Ats.Services;
using AutoMapper;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Ats.Services.DI;

namespace Ats.Api
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
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(20);//You can set Time  
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
          
            var connectionString = Configuration.GetConnectionString("DataAccessSqlProvider");

            services.AddControllers()
              .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddMemoryCache();
            services.AddCors();

            services.AddDbContext<SCDataContext>(options =>
            {
                options.UseLazyLoadingProxies().UseMySql(connectionString);
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddRepositoryCollection();
            services.AddServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseMySql(connectionString));

            //services.ConfigureAuth(connectionString);
            services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(token =>
           {
               
               var SecretKey = Encoding.ASCII.GetBytes(Configuration.GetSection("SecretKey").Value);
               var key = new SymmetricSecurityKey(SecretKey);
               var domain = Configuration.GetSection("Domain").Value;
               token.RequireHttpsMetadata = false;
               token.SaveToken = true;
               token.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   ValidAudience = domain,
                   IssuerSigningKey = key,
                   ValidateIssuer = true,
                   RequireExpirationTime = true,
                   ValidateLifetime = true, 
                   ClockSkew = TimeSpan.Zero,
                   ValidIssuer = domain
               };
           });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TigerNow",
                    Version = "v1",
                    Description = "TigerNow Web API",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                       {
                         new OpenApiSecurityScheme
                         {
                           Reference = new OpenApiReference
                           {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                           }
                          },
                          new string[] { }
                        }
                      });
            });

            var autoMapperConfig = new MapperConfiguration(mc =>
            {
               // mc.AddProfile(new AutoMapperConfig());
            });

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                .AllowAnyMethod()
                                                                 .AllowAnyHeader()));
            services.AddMvc(config =>
                                   {
                                       config.RespectBrowserAcceptHeader = true;
        });
       IMapper mapper = autoMapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials()
            );
            app.UseCors("AllowAll");
            //app.ConfigureCustomExceptionMiddleware();

//#if DEBUG
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PTI 4T Web API V1");
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });
//#endif
            app.UseRouting();

            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //});

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
    
}
