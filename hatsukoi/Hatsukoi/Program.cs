using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository;
using Hatsukoi.Repository.DataContext;
using Hatsukoi.Service;
using Hatsukoi.Service.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Repository.DBRepository.Interface;
using Hatsukoi.Repository.DBRepository;
using Hatsukoi.Hubs;
using Hatsukoi.Attributes;
using Microsoft.AspNetCore.SignalR;
using Coravel;
using Hatsukoi.Scheduler;
using Microsoft.Extensions.Options;
using System.Configuration;
using StackExchange.Redis;
using Hatsukoi.Service.CacheServices;
using Hatsukoi.Configuration;

namespace Hatsukoi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddRazorPages();

            // Add services to the container.
            //資料庫連線設定
            var conn = builder.Configuration.GetConnectionString("HatsukoiContext");

            //分散式快取，Redis
            var redisConn = builder.Configuration.GetConnectionString("RedisConnectionString");
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConn;
                options.InstanceName = "MyRedisCache";
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<HatsukoiContext>(options => options.UseSqlServer(conn));
            builder.Services.AddDistributedMemoryCache();
            //builder.Services.AddHostedService<TimedHostedService>();
            builder.Services.AddScheduler();
            builder.Services.AddScoped<SendNotiScheduler>();
            builder.Services.AddScoped<ClearShippedOrders>();
            builder.Services.AddScoped<CancelUnpaidOrders>();
            builder.Services.AddSignalR();
            builder.Services.AddSignalR().AddHubOptions<ChatHub>(options =>
            {
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
                options.HandshakeTimeout = TimeSpan.FromSeconds(30);
            });


                       
            builder.Services.AddScoped<IHomepageService, RedisHomepageCacheService>();

            //builder.Services.AddScoped<IHubContext<ChatHub>, IHubContext<ChatHub>>();
            //builder.Services.AddSignalR().AddHubOptions<MyOtherHub>(options =>
            //{
            //    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
            //    options.HandshakeTimeout = TimeSpan.FromSeconds(60);
            //});
            //Service註冊
            //builder.Services.AddTransient<EFRepository>();
            //builder.Services.AddTransient<IRepository>();
            #region ===Servive===
            //builder.Services.AddTransient<AccountService>();
            //builder.Services.AddTransient<OrderService>();
            //builder.Services.AddTransient<NotifyService>();
            //builder.Services.AddTransient<IAccountService, AccountService>();
            //builder.Services.AddTransient<ProductService>();
            //builder.Services.AddTransient<ShopService>();
            //builder.Services.AddTransient<SellerService>();
            //builder.Services.AddTransient<HomepageService>();
            //builder.Services.AddTransient<HomeProductFilterService>();
            //builder.Services.AddTransient<IHomeProductFilterService, HomeProductFilterService>();
            //builder.Services.AddTransient<IEmailService, EmailService>();
            //builder.Services.AddTransient<FavoriteListService>();
            //builder.Services.AddTransient<IImageService, ImageService>();
            //builder.Services.AddTransient<UserService>();
            //builder.Services.AddTransient<CartService>();
            //builder.Services.AddTransient<CenterNotificationService>();
            //builder.Services.AddTransient<ChatRoomService>();
            builder.Services.AddScoped<IRepository, EFRepository>();
            builder.Services.AddScoped<IFavRepository, FavRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<IHomeProductFilterRepository, HomeProductFilterRepository>();
            builder.Services.AddScoped<IHomepageRepository, HomepageRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            
            //builder.Services.AddTransient<ReportService>();
            //builder.Services.AddTransient<PaymentService>();
            builder.Services.AddScoped<PaymentRepository>();

            #endregion


            builder.Services.AddWebService();


            //cookie設定
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                //沒權限
                options.AccessDeniedPath = "/Home/Index/";
                //沒驗證，要跳登入
                options.LoginPath = "/Login/Index/";
            });
            //設定 Google 驗證
            var services = builder.Services;
            var configuration = builder.Configuration;

            //services.AddAuthentication()
            //.AddGoogle(googleOptions =>
            //{
            //    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
            //    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            //});
            services.AddAuthentication()
               .AddGoogle(options =>
               {
                   options.ClientId = configuration["Authentication:Google:ClientId"];
                   options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
               })
               .AddFacebook(options =>
               {
                   options.ClientId = configuration["Authentication:Facebook:ClientId"];
                   options.ClientSecret = configuration["Authentication:Facebook:ClientSecret"];
               })
               ;
            //為Swagger套件註冊服務
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddControllers(options =>
            //{
            //    // ↓↓↓ 設定全域Filters ↓↓↓
            //    options.Filters.Add(typeof(ExceptionAttribute));
            //});

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
                app.UseDeveloperExceptionPage();
                //如果是開發環境的話裝上這兩個Swagger套件
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //授權的中介軟體
            app.UseAuthentication();
            app.UseAuthorization();


            app.Services.UseScheduler(scheduler =>
            {
                scheduler.Schedule<SendNotiScheduler>().EveryFiveMinutes();
                scheduler.Schedule<ClearShippedOrders>().EveryMinute();
                scheduler.Schedule<CancelUnpaidOrders>().EveryMinute();
            });
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //Cart
            app.MapControllerRoute(
                name: "Cart",
                pattern: "ShoppingCart",
                defaults: new { controller = "Cart", action = "Index" });

            //CartToCheck
            app.MapControllerRoute(
                name: "Cart",
                pattern: "Buyme",
                defaults: new { controller = "Cart", action = "CartToCheck" });

            //HomePage
            app.MapControllerRoute(
                name: "HomePage",
                pattern: "Home",
                defaults: new { controller = "Home", action = "Index" });

            //Product
            app.MapControllerRoute(
                name: "Product",
                pattern: "Product/Id/{id?}",
                defaults: new { controller = "Product", action = "Index" });

            //Shop
            app.MapControllerRoute(
                name: "Shop",
                pattern: "Shop",
                defaults: new { controller = "Product", action = "Shop" });


            //Fav
            app.MapControllerRoute(
                name: "Favlist",
                pattern: "MyFavorite",
                defaults: new { controller = "Favlist", action = "Index" });

            //ProductFilter
            app.MapControllerRoute(
                name: "ProductFilter",
                pattern: "ProductFilter",
                defaults: new { controller = "Home", action = "ProductFilter" });

            //ProductFilterByCat
            app.MapControllerRoute(
                name: "ProductFilterByCat",
                pattern: "ProductFilter/Category",
                defaults: new { controller = "Home", action = "ProductFilterByCat" });

            //ProductFilterByKeyword
            app.MapControllerRoute(
                name: "ProductFilterByKeyword",
                pattern: "Search",
                defaults: new { controller = "Home", action = "ProductFilterByKeyword" });

            //OrderDetails
            app.MapControllerRoute(
                name: "OrderDetails",
                pattern: "OrderDetails",
                defaults: new { controller = "Order", action = "OrderDetails" });

            //OrderSearch
            app.MapControllerRoute(
                name: "OrderSearch",
                pattern: "OrderSearch",
                defaults: new { controller = "Seller", action = "Search" });


            //設定Hub的中介軟體
            app.MapHub<ChatHub>("/Chathub");
            app.Use(async (context, next) =>
            {
                var hubContext = context.RequestServices
                                        .GetRequiredService<IHubContext<ChatHub>>();
                //...

                if (next != null)
                {
                    await next.Invoke();
                }
            });
            app.Run();
        }
    }
}