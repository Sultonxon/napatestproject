

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("TestDB");
    //options.UseSqlServer(connectionString, b => b.MigrationsAssembly("NapaProjects.OnlineMarket"));
});
builder.Services.AddIdentity<AppUser, IdentityRole<int>>(options =>
{
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>()
  .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/Login/SignIn");
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddTransient<UserValidator<AppUser>>();
builder.Services.AddTransient<PasswordValidator<AppUser>>();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);


var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseStatusCodePages();
app.UseAuthentication();
app.UseMvc(routes =>
{
    routes.MapRoute("home", "/", new { controller = "Home", Action = "Index" });
    routes.MapRoute("",template: "{controller}/{action}/");
});

//AppDbContext.InitializeOnMigration(app.Services);
app.Run();