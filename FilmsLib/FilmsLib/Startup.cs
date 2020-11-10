using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FilmsLib.Data;
using FilmsLib.Models;
using FilmsLib.Services.Interfaces;
using FilmsLib.Services.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace FilmsLib
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
            //services.AddDbContext<FilmsLibContext>(options =>
            // options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<FilmsLibContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MsSqlConnection")));

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
              .AddEntityFrameworkStores<FilmsLibContext>()
              .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireEmail", policy => policy.RequireClaim(ClaimTypes.Email));
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHttpContextAccessor();

            services.AddControllersWithViews();

            services.AddRazorPages().AddMvcOptions(options =>
             {
                 options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                        _ => "Це поле обов'язкове");
                 options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(
                        _ => "Це поле обов'язкове");
             });

            services.AddScoped<IReviewerRepository, ReviewerRepository>();
            services.AddScoped<IDetailsRepository, DetailsRepository>();
            services.AddScoped<IFilmRepository, FilmRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, FilmsLibContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            InitializeIdentity(serviceProvider);
        }

        private void InitializeIdentity(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            Task<IdentityResult> roleResultAdmin;
            Task<IdentityResult> roleResultUser;

            string email = "admin@admin.com";
            string password = "Qwe123!";

            Task<bool> hasAdminRole = roleManager.RoleExistsAsync("Admin");
            hasAdminRole.Wait();

            if (!hasAdminRole.Result)
            {
                roleResultAdmin = roleManager.CreateAsync(new IdentityRole("Admin"));
                roleResultAdmin.Wait();
            }

            Task<bool> hasUserRole = roleManager.RoleExistsAsync("User");
            hasUserRole.Wait();

            if (!hasUserRole.Result)
            {
                roleResultUser = roleManager.CreateAsync(new IdentityRole("User"));
                roleResultUser.Wait();
            }

            Task<User> testUser = userManager.FindByEmailAsync(email);
            testUser.Wait();

            if (testUser.Result == null)
            {
                User administrator = new User
                {
                    Email = email,
                    UserName = email
                };

                Task<IdentityResult> newUser = userManager.CreateAsync(administrator, password);
                newUser.Wait();

                if (newUser.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(administrator, "Admin");
                    newUserRole.Wait();
                }
            }
        }
    }
}
