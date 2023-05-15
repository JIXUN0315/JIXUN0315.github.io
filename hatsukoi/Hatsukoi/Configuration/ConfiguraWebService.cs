using Hatsukoi.Service;
using Hatsukoi.Service.Interface;

namespace Hatsukoi.Configuration
{
    public static class ConfiguraWebService
    {
        public static IServiceCollection AddWebService(this IServiceCollection services)
        {
            services.AddTransient<AccountService>();
            services.AddTransient<OrderService>();
            services.AddTransient<NotifyService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ProductService>();
            services.AddTransient<ShopService>();
            services.AddTransient<SellerService>();
            services.AddTransient<HomepageService>();
            services.AddTransient<HomeProductFilterService>();
            services.AddTransient<IHomeProductFilterService, HomeProductFilterService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<FavoriteListService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<UserService>();
            services.AddTransient<CartService>();
            services.AddTransient<CenterNotificationService>();
            services.AddTransient<ChatRoomService>();
            services.AddTransient<ReportService>();
            services.AddTransient<PaymentService>();
            return services;
        }
    }
}
