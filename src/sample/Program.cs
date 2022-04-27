using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services
        .AddOpenTelemetryTracing((otelBuilder) => otelBuilder
        .AddAspNetCoreInstrumentation()
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Otel-code-path"))
        .AddSource("MySource")
        .AddOtlpExporter(otlpOptions => {
            otlpOptions.Endpoint = new Uri("https://api.honeycomb.io:443");
            var key = builder.Configuration.GetValue<string>("Honeycomb:ApiKey");
            Console.WriteLine(key);
            otlpOptions.Headers = $"x-honeycomb-team={key}";
        }));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
