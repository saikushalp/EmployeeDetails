using Abb.EmployeeDetails.Api.DataAccess;
using Abb.EmployeeDetails.Api.DataAccess.Context;
using Abb.EmployeeDetails.Api.Mappers;
using Abb.EmployeeDetails.Api.Services;
using Abb.EmployeeDetails.Api.Validator;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Abb.EmployeeDetails.Api
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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder
                        .WithOrigins("http://localhost:5000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.Authority = Configuration["AuthToken:EndPoint"];
                opt.Audience = Configuration["AuthToken:Audience"];
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

            services.AddDbContextPool<ApplicationContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSwaggerGen();
            services.AddControllers().AddFluentValidation(opt =>
                opt.RegisterValidatorsFromAssemblyContaining<EmployeeCreateValidator>());
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeDataAccess, EmployeeDataAccess>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Details Api V1");
            });

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigin");
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
