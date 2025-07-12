public static class TestDataGenerator
{
    private static readonly Random _random = new Random();
    
    public static int RandomQuantity(int min = 1, int max = 100)
    {
        return _random.Next(min, max + 1);
    }

    public static string RandomEnergyType()
    {
        var energyTypes = new List<string> { "electric", "gas", "oil", "nuclear", "coal" };
        return energyTypes[_random.Next(energyTypes.Count)];
    }

    public static DateTime RandomOrderDate(DateTime? startDate = null, DateTime? endDate = null)
    {
        startDate ??= DateTime.Now.AddYears(-1);
        endDate ??= DateTime.Now;
        
        TimeSpan timeSpan = endDate.Value - startDate.Value;
        TimeSpan newSpan = new TimeSpan(0, _random.Next(0, (int)timeSpan.TotalMinutes), 0);
        return startDate.Value + newSpan;
    }

    public static string RandomCustomerId()
    {
        return Guid.NewGuid().ToString().Substring(0, 8);
    }

    public static Dictionary<string, int> GenerateEnergyPurchaseCombinations()
    {
        return new Dictionary<string, int>
        {
            { "electric", RandomQuantity() },
            { "gas", RandomQuantity() },
            { "oil", RandomQuantity() }
        };
    }

    public static Dictionary<string, object> GenerateInvalidPurchaseData()
    {
        var invalidTypes = new List<object>
        {
            new { energyType = "invalid", quantity = 1 },
            new { energyType = "electric", quantity = 0 },
            new { energyType = "gas", quantity = -1 },
            new { energyType = "oil", quantity = 999999 }
        };

        return new Dictionary<string, object>
        {
            { "InvalidEnergyType", invalidTypes[0] },
            { "ZeroQuantity", invalidTypes[1] },
            { "NegativeQuantity", invalidTypes[2] },
            { "ExcessiveQuantity", invalidTypes[3] }
        };
    }

    public static List<DateTime> GenerateHistoricalOrderDates(int count)
    {
        var dates = new List<DateTime>();
        for (int i = 0; i < count; i++)
        {
            dates.Add(RandomOrderDate(endDate: DateTime.Now.AddDays(-1)));
        }
        return dates;
    }
}