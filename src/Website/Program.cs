using Website.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<ApiOptions>()
    .Configure<IConfiguration>((settings, configuration) => configuration
        .GetSection(nameof(ApiOptions))
        .Bind(settings));

builder.Services.AddControllersWithViews();

builder.Services
    .AddHttpClient(builder.Configuration)
    .AddDependencies();

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
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