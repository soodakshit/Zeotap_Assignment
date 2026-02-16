using Microsoft.EntityFrameworkCore;
using IncidentTracker.Api.Models;

namespace IncidentTracker.Api.Data;

public static class DatabaseSeeder
{
    private static readonly Random _random = new();
    
    private static readonly string[] Services = new[]
    {
        "Auth Service", "Payment Gateway", "User Service", "API Gateway",
        "Database Cluster", "Cache Layer", "Search Service", "Email Service",
        "Notification Service", "Analytics Engine", "CDN", "Load Balancer",
        "Message Queue", "Storage Service", "Logging Service"
    };

    private static readonly string[] TitleTemplates = new[]
    {
        "High latency detected in {0}",
        "{0} experiencing connection timeouts",
        "Database deadlock in {0}",
        "Memory leak detected in {0}",
        "{0} returning 5xx errors",
        "Service degradation in {0}",
        "API rate limit exceeded for {0}",
        "Disk space critical on {0}",
        "Network connectivity issues with {0}",
        "Configuration error in {0}",
        "Security vulnerability found in {0}",
        "Data corruption detected in {0}",
        "Performance degradation on {0}",
        "{0} failed health check",
        "Deployment rollback needed for {0}"
    };

    private static readonly string?[] Owners = new[]
    {
        "John Doe", "Jane Smith", "Bob Johnson", "Alice Williams",
        "Charlie Brown", "Diana Prince", "Ethan Hunt", "Fiona Gallagher",
        null, null // Some incidents without owners
    };

    public static async Task SeedDataAsync(ApplicationDbContext context)
    {
        if (await context.Incidents.AnyAsync())
        {
            return; // Already seeded
        }

        var incidents = new List<Incident>();
        var startDate = DateTime.UtcNow.AddDays(-90); // Past 90 days

        for (int i = 0; i < 200; i++)
        {
            var service = Services[_random.Next(Services.Length)];
            var titleTemplate = TitleTemplates[_random.Next(TitleTemplates.Length)];
            var createdAt = startDate.AddDays(_random.NextDouble() * 90);
            var severity = (Severity)_random.Next(4);
            
            // Status distribution: more resolved than open
            var statusRoll = _random.NextDouble();
            var status = statusRoll < 0.6 ? Status.RESOLVED :
                        statusRoll < 0.8 ? Status.MITIGATED :
                        Status.OPEN;

            var incident = new Incident
            {
                Id = Guid.NewGuid(),
                Title = string.Format(titleTemplate, service),
                Service = service,
                Severity = severity,
                Status = status,
                Owner = Owners[_random.Next(Owners.Length)],
                Summary = GenerateSummary(service, severity),
                CreatedAt = createdAt,
                UpdatedAt = status == Status.OPEN ? createdAt : 
                           createdAt.AddHours(_random.NextDouble() * 48)
            };

            incidents.Add(incident);
        }

        await context.Incidents.AddRangeAsync(incidents);
        await context.SaveChangesAsync();
    }

    private static string? GenerateSummary(string service, Severity severity)
    {
        var summaries = new string?[]
        {
            $"Investigating {severity} incident affecting {service}. Initial diagnostics show elevated error rates.",
            $"Multiple users reporting issues with {service}. Team is actively investigating root cause.",
            $"Automated alerts triggered for {service}. Monitoring metrics indicate abnormal behavior.",
            $"Production incident detected in {service}. Engineering team has been paged.",
            $"Service degradation observed in {service}. Impact assessment in progress.",
            null
        };
        
        return summaries[_random.Next(summaries.Length)];
    }
}