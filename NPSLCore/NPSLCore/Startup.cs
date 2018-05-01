using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NPSLCore.Models.DB;


using NPSL.Repository.Core.User;
using NPSL.Models.Models.DB;
using NPSL.Extensions;
using NPSL.Extentions.CustomException;

namespace NPSLCore
{
    public class Startup
    {
        public static string ConnectionString { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
            });
            services.AddDbContext<NPSLContext>(options => options.UseSqlServer(Configuration["ConnectionString:DBConnection"]));
            services.AddSingleton<IConfiguration>(Configuration);

            // services.AddDbContext<BaseDataAccess>(options => options.UseSqlServer(Configuration["ConnectionString:DBConnection"]));

            
            services.AddScoped<DatabaseContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            ////services.AddDbContext<TrainingDatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
               app.UseDeveloperExceptionPage();

            }
            else { app.UseExceptionHandler(); }
            app.UseCors("AllowAllHeaders");
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMvc();
           
        }
    }
}
