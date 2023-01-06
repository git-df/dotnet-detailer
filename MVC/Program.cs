using Data;
using MVC;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddData();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(op => {
        op.Cookie.Name = "CleanAndCareAuth";
        op.LoginPath = "/Access/SignIn";
        op.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        op.AccessDeniedPath = "/Home/Index";
    });

builder.Services.AddAuthorization(op => {
    //[Authorize(Policy = "MustBeEmployee")]
    op.AddPolicy("MustBeEmployee",
        policy => policy.RequireClaim("EmployeeId"));

    //[Authorize(Policy = "MustBeAdmin")]
    op.AddPolicy("MustBeAdmin",
        policy => policy.RequireClaim("IsAdmin", "True"));
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

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
