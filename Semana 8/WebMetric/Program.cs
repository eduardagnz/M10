using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenTelemetry()
    .WithMetrics(builder =>
    {
        builder.AddPrometheusExporter();
        builder.AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel");
        builder.AddView("http.server.request.duration",
            new ExplicitBucketHistogramConfiguration
            {
                Boundaries = new double[] { 0, 0.005, 0.01, 0.025, 0.05, 0.1, 0.25, 0.5, 1 }
            });
    });

var app = builder.Build();

app.MapPrometheusScrapingEndpoint();
app.MapGet("/", () => "Hello OpenTelemetry! Ticks:" + DateTime.Now.Ticks.ToString()[^3..]);

app.Run();