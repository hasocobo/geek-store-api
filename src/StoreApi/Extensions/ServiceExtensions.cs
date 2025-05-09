using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StoreApi.Entities;
using StoreApi.Features;
using StoreApi.Features.Authentication;
using StoreApi.Features.Carts;
using StoreApi.Features.Categories;
using StoreApi.Features.Customers;
using StoreApi.Features.Orders;
using StoreApi.Features.Products;
using StoreApi.Features.Wishlists;
using StoreApi.Infrastructure;

namespace StoreApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );
            });
        }

        public static void AddEntityRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IWishlistRepository, WishlistRepository>();
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        public static void AddEntityServices(this IServiceCollection services)
        {
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IServiceManager, ServiceManager>();
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(o =>
                {
                    o.User.RequireUniqueEmail = true;
                    o.Password.RequiredLength = 8;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<StoreContext>()
                .AddDefaultTokenProviders();
        }

        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = Environment.GetEnvironmentVariable("STORE_API_JWT_SECRET") ?? "jwt key";

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings["ValidIssuer"],
                    ValidAudience = jwtSettings["ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }
    }
}