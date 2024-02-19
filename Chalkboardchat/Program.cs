using Chalkboardchat.Data.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//Connection String
var connectionString = builder.Configuration.GetConnectionString("AuthConnection");
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connectionString));


//Connection String för AuthDbContext
var authConnectionString = builder.Configuration.GetConnectionString("AuthConnection");
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(authConnectionString));

//Connection String för AppDbContext
var appConnectionString = builder.Configuration.GetConnectionString("AppConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(appConnectionString));

//Lägger till Identity
//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();



//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
//{
//    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
//})
//    .AddEntityFrameworkStores<AuthDbContext>()
//    .AddDefaultTokenProviders(); ;


builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Configure password requirements if needed
    // options.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//Kollar vilken roll som är inloggad
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
