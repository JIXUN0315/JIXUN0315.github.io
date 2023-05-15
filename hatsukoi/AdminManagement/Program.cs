using AdminManagement.Configuration;
using AdminManagement.Scheduler;
using AdminManagement.Services;
using Coravel;
using Hatsukoi.Repository.DataContext;
using Hatsukoi.Repository.DBRepository;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Scheduler;
using Microsoft.EntityFrameworkCore;

namespace AdminManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var conn = builder.Configuration.GetConnectionString("HatsukoiContext");
            builder.Services.AddDbContext<HatsukoiContext>(options => options.UseSqlServer(conn));
            builder.Services.AddDistributedMemoryCache();
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<SellerService>();
            builder.Services.AddTransient<SalesDataService>();
            builder.Services.AddTransient<BannerService>();
            builder.Services.AddTransient<NotificationService>();
            builder.Services.AddTransient<RevenueService>();
            builder.Services.AddTransient<EmailService>();
            builder.Services.AddScoped<IRepository, EFRepository>();
            builder.Services.AddTransient<OrderService>();
            builder.Services.AddTransient<LineService>();
            builder.Services.AddTransient<LineBotService>();
            builder.Services.AddTransient<LoginService>();

            builder.Services.AddScheduler();
            builder.Services.AddScoped<SendLineMessage>();
            builder.Services.AddTransient<MemberService>();




            builder.Services
                .AddJwtServices(builder.Configuration)
                .AddSwaggerService();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Services.UseScheduler(scheduler =>
            {
                scheduler.Schedule<SendLineMessage>().EveryMinute();
                
            });

            app.MapControllerRoute(
                name: "Login",
                pattern: "Login",
                new { Controller = "Auth", Action = "Login" }
                );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=DashBoard}/{action=Index}/{id?}");

            app.Run();
        }
    }
}