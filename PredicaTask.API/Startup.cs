using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PredicaTask.API.Helpers;
using PredicaTask.API.Middleware;
using PredicaTask.Application;
using PredicaTask.Application.RepositoryInterfaces;
using PredicaTask.Application.Services;
using PredicaTask.Data;
using PredicaTask.DataManipulation;


namespace PredicaTask.API
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
            services.ConfigureApplicationServices();
            services.AddDbContext<PredicaTaskDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MssqlConnection"));
                
            });
            services.AddCors();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<JwtService>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IWeathersService, WeathersService>();
            
            services.AddControllers();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PredicaTask.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PredicaTask.API v1"));
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors(options => options
                .WithOrigins(new[] { "http://localhost:3000", "http://localhost:8080", "http://localhost:7222" })
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
            );

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}