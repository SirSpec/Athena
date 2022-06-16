using Website.Options;
using Website.Policies;
using Website.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<ApiOptions>()
    .Configure<IConfiguration>((settings, configuration) => configuration
        .GetSection(nameof(ApiOptions))
        .Bind(settings));

builder.Services.AddControllersWithViews();

builder.Services
    .AddHttpClient<IPostRepository, PostRepository>(configureClient =>
    {
        configureClient.DefaultRequestHeaders.Add("User-Agent", "request");
        configureClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");

        if (builder.Environment.IsDevelopment())
        {
            var apiOptions = builder.Configuration
                .GetSection(nameof(ApiOptions))
                .Get<ApiOptions>();

            configureClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiOptions.AccessToken}");
        }
    })
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddPolicyHandler(PostServicePolicyFactory.GetRetryPolicy(builder.Services))
    .AddPolicyHandler(PostServicePolicyFactory.GetCircuitBreakerPolicy(builder.Services));

builder.Services.AddDependencies();

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
