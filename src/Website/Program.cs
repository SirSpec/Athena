using Website.Options;

var builder = WebApplication.CreateBuilder(args);

if(builder.Environment.IsProduction())
{
    builder.Configuration.AddAppConfiguration();
    builder.Services.AddAzureAppConfiguration();
}

builder.Services
    .AddOptions<ApiOptions>()
    .Configure<IConfiguration>((settings, configuration) => configuration
        .GetSection(nameof(ApiOptions))
        .Bind(settings));

builder.Services
    .AddOptions<CacheOptions>()
    .Configure<IConfiguration>((settings, configuration) => configuration
        .GetSection(nameof(CacheOptions))
        .Bind(settings));

builder.Services
    .AddDependencies()
    .AddHttpClient(builder.Configuration);

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
    app.UseAzureAppConfiguration();
}

app.UseStatusCodePagesWithRedirects("/error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(policy =>
{
    var apiOptions = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
    policy.WithOrigins(apiOptions);
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}");

app.Run();