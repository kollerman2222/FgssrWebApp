using fgssr.CustomMethodsServices;
using fgssr.Data;
using fgssr.Hubs;
using fgssr.Models;
using fgssr.UnitOFWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace fgssr
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.Configure<TwilioSetting>(builder.Configuration.GetSection("Twilio"));

            builder.Services.AddSignalR();


            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddTransient<IEmailSystem , EmailSystem>();
            builder.Services.AddTransient<ISMSSystem, SMSSystem>();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultUI().AddDefaultTokenProviders();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
            });

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            });

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();


            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {

                    ctx.Context.Response.Headers.Append("Cache-Control", "public, max-age=345600"); /* 4 days */
                }
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Homepage}/{id?}");
            app.MapRazorPages();
            app.MapHub<ChatHub>("/chatHub");
            app.MapHub<AlertHub>("/alertHub");


            app.Run();
        }
    }
}
