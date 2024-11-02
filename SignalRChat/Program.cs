global using Microsoft.EntityFrameworkCore.ChangeTracking;
global using System.ComponentModel.DataAnnotations;
global using Microsoft.AspNetCore.Http.Features;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.SignalR;
global using SignalRChat.Other.Interfaces;
global using SignalRChat.Other.Sessions;
global using Microsoft.AspNetCore.Mvc;
global using System.Linq.Expressions;
global using SignalRChat.Other.Enums;
global using SignalRChat.Controllers;
global using SignalRChat.Other.Hubs;
global using SignalRChat.Models;
global using SignalRChat.Other;
global using System.Reflection;
global using System.Net.Mail;
global using System.Net;


/*
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
}); 
*/
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddSession();
builder.Services.AddControllersWithViews();
builder.Services.AddRouting(x=>x.LowercaseUrls=true);//urldeki tum harfler kucuk harf
builder.Services.AddScoped<IAccountService,AccountService>();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue;
});
var app = builder.Build();
app.UseSession();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapHub<ChatHub>("/chatHub");
app.UseStatusCodePagesWithReExecute("/Home/Index");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}",
    defaults: new {controller="Home",action="Index"}
    );
app.Run();
