using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using OpenTelemetry.Trace;

namespace TrailmarksApi.Tests.OpenTelemetry
{
    public class OpenTelemetryConfigurationTests
    {
        [Fact]
        public void OpenTelemetry_Services_Are_Registered()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            
            // Simulate the OpenTelemetry configuration from Program.cs
            builder.Services.AddOpenTelemetry()
                .WithTracing(tracing => tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation());

            var app = builder.Build();

            // Act
            var tracerProvider = app.Services.GetService<TracerProvider>();

            // Assert
            Assert.NotNull(tracerProvider);
        }

        [Fact]
        public void OpenTelemetry_TracerProvider_Is_Available()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            
            // Simulate the OpenTelemetry configuration from Program.cs
            builder.Services.AddOpenTelemetry()
                .WithTracing(tracing => tracing
                    .AddAspNetCoreInstrumentation());

            var serviceProvider = builder.Services.BuildServiceProvider();

            // Act
            var tracerProvider = serviceProvider.GetService<TracerProvider>();

            // Assert
            Assert.NotNull(tracerProvider);
        }
    }
}
