namespace CleanArchMvc.Infra.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        #region Database

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        });

        #endregion

        #region Repositories

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        #endregion

        #region Services

        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();

        #endregion

        #region Automapper Profile

        services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

        #endregion

        #region MediatR

        var myhandlers = AppDomain.CurrentDomain.Load("CleanArchMvc.Application");
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(myhandlers));

        #endregion

        #region Security (identity)

        services
            .AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IAuthenticate, AuthenticateService>();
        services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();

        // Existem diversas opções de configurações para este serviço
        services.ConfigureApplicationCookie(options =>
        {
            // Quando o usuário não tiver acesso a um recurso a aplicação irá redirecioná-lo para o login.
            //options.AccessDeniedPath = "/Account/Login";
            options.AccessDeniedPath = "/OperationLock/Index";
        });

        #endregion

        return services;
    }
}
