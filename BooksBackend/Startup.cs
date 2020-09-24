using AutoMapper;
using BooksBackend.Domain;
using BooksBackend.Profiles;
using BooksBackend.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BooksBackend
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
                options.AddPolicy("CorsPolicy", builder =>
                    builder.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                );
            });

            services.AddControllers();

            // add singleton - all requests will share a single instance of this thing
            // add transient - for any object i create, create a new instance of it
            // add scoped - share this instance for the entire request
            services.AddTransient<ISystemTime, SystemTime>();
            services.AddDbContext<BooksDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("books")));
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new BooksProfile());
            }
            );
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton<IMapper>(mapper); // read only and slow to create, so singleton works here
            services.AddSingleton<MapperConfiguration>(mappingConfig);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
