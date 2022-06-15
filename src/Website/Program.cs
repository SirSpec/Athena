using Website.Options;
using Website.Policies;
using Website.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<ApiOptions>()
    .Configure<IConfiguration>((settings, configuration) => configuration
        .GetSection(nameof(ApiOptions))
        .Bind(settings));

builder.Services.AddControllersWithViews();

builder.Services
    .AddHttpClient<IPostService, PostService>(configureClient =>
    {
        var apiOptions = builder.Configuration
            .GetSection(nameof(ApiOptions))
            .Get<ApiOptions>();

        configureClient.BaseAddress = new Uri(apiOptions.BaseAddress ?? throw new ArgumentNullException());
        configureClient.DefaultRequestHeaders.Add("User-Agent", "request");
        configureClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");

        if (builder.Environment.IsDevelopment())
            configureClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiOptions.AccessToken}");
    })
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddPolicyHandler(PostServicePolicyFactory.GetRetryPolicy(builder.Services));

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
