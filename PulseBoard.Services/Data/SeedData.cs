using PulseBoard.Application.Common.Enums;
using PulseBoard.Services.Entities;

namespace PulseBoard.Services.Data;

public static class SeedData
{
    public static async Task InitialiseAsync(AppDbContext context)
    {
        if (context.Customers.Any())
            return;

        var random = new Random();

        // -----------------------
        // Customers
        // -----------------------
        var customers = new List<Customer>
        {
            new() { Name = "Acme Ltd", Region = Region.UK, Segment = Segment.Enterprise, ChurnRiskScore = 20 },
            new() { Name = "Bright Retail", Region = Region.UK, Segment = Segment.SMB, ChurnRiskScore = 65 },
            new() { Name = "Nordic Tech", Region = Region.EU, Segment = Segment.Mid, ChurnRiskScore = 35 },
            new() { Name = "Velocity Labs", Region = Region.EU, Segment = Segment.Enterprise, ChurnRiskScore = 15 },
            new() { Name = "Delta Supplies", Region = Region.UK, Segment = Segment.SMB, ChurnRiskScore = 50 }
        };

        await context.Customers.AddRangeAsync(customers);
        await context.SaveChangesAsync(); // ensure IDs generated

        // -----------------------
        // Orders
        // -----------------------
        var startDate = DateTime.UtcNow.AddMonths(-6);

        var orders = new List<Order>();

        foreach (var customer in customers)
        {
            for (int i = 0; i < 40; i++)
            {
                orders.Add(new Order
                {
                    Customer = customer, // ✅ use navigation
                    Amount = random.Next(500, 5000),
                    OrderDate = startDate.AddDays(random.Next(0, 180)),
                    ProductLine = random.Next(0, 2) == 0 ? "Core" : "Premium",
                    Region = customer.Region,
                    Currency = "Sterling",
                    IsRecurring = random.Next(0, 2) == 0
                });
            }
        }

        await context.Orders.AddRangeAsync(orders);
        await context.SaveChangesAsync(); // save orders separately

        // -----------------------
        // Deals
        // -----------------------
        var deals = new List<Deal>();

        foreach (var customer in customers)
        {
            deals.Add(new Deal
            {
                Customer = customer, // ✅ use navigation
                Amount = random.Next(10000, 100000),
                Stage = Stage.Proposal,
                ExpectedCloseDate = DateTime.UtcNow.AddDays(random.Next(10, 90)),
                WinProbability = 0.6m,
                RiskScore = random.Next(10, 80),
                Owner = "Sales Team"
            });
        }

        await context.Deals.AddRangeAsync(deals);
        await context.SaveChangesAsync();

        // -----------------------
        // Targets
        // -----------------------
        var currentMonthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var currentMonthEnd = currentMonthStart.AddMonths(1).AddDays(-1);

        var targets = new List<Target>
        {
            new()
            {
                PeriodStart = currentMonthStart,
                PeriodEnd = currentMonthEnd,
                Region = Region.UK,
                TargetRevenue = 250000m
            },
            new()
            {
                PeriodStart = currentMonthStart,
                PeriodEnd = currentMonthEnd,
                Region = Region.EU,
                TargetRevenue = 200000m
            }
        };

        await context.Targets.AddRangeAsync(targets);
        await context.SaveChangesAsync();
    }
}
