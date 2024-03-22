using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using TelemtryUsingJaeger.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetryTracing(options =>
                                        options.AddConsoleExporter().AddSource(TestTelemetryController.TelemtrySource)
                                        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(TestTelemetryController.TelemtrySource).AddTelemetrySdk())
                                        .AddHttpClientInstrumentation()
                                        .AddAspNetCoreInstrumentation()
                                        .AddJaegerExporter()

                                        );

builder.Services.AddOpenTelemetryMetrics(options =>
                                            options.AddHttpClientInstrumentation()
                                            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(TestTelemetryController.TelemtrySource).AddTelemetrySdk())
                                            .AddMeter("Meter1")
                                            .AddOtlpExporter()
                                        );
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
