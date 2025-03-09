using contact_management;
using contact_management.Repositories.Classes;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//serviceProvider is IServiceProvider object who fetch the registered service from DI Container.
builder.Services.AddSingleton<NpgsqlConnection>((serviceProvider) =>
{
    var connection = serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("pgconn");
    return new NpgsqlConnection(connection);
});
builder.Services.AddSingleton<IUserInterface, UserRepository>();
builder.Services.AddSession((option) =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(30);
    option.Cookie.IsEssential = true;
    option.Cookie.HttpOnly=true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();//Session Middleware

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
