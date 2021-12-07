using MiniUri.Common;
using MiniUri.Common.Plugins;
using MiniUri.UriService.Contracts;
using MiniUri.UriService.Implementation;
using MiniUri.UriService.Implementation.Plugins;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IIdGenerator<long>, Int64IdGenerator>();
builder.Services.AddSingleton<IKeyEncoder<long>, Int64ToBase64UrlEncoder>();
builder.Services.AddSingleton<IStorage<long>, Storage<long>>();
builder.Services.AddSingleton<IValidation, Validation>();
builder.Services.AddSingleton<IUriService, UrlService<long>>();

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();