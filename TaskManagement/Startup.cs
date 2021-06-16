using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using TaskManagement.Data;
using TaskManagement.Models.DAO;
using TaskManagement.Models.Services;
using TaskManagement.Profile;


namespace TaskManagement
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
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews().AddViewLocalization();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("TaskManagement"));
                // options.EnableSensitiveDataLogging();
            });
            services
                .AddScoped<IDbRepository, DbService>(provider =>
                    new DbService(provider.GetRequiredService<AppDbContext>()));
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddTransient<ITaskDao, TaskService>();
            services.AddTransient<ITreeItemDao, TreeItemService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ASP.NETCoreApi", Version = "v1"});
            });
            
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.NETCoreApi v1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>()?.Value;
            app.UseRequestLocalization(localizationOptions);

            using var scope = app.ApplicationServices.CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            DbContentInit.Initial(context);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
            });
        }
    }
}