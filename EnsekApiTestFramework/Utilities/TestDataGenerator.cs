using System;
using System.Collections.Generic;

public static class TestDataGenerator
{
    private static readonly Random _random = new Random();
    
    public static int RandomQuantity(int min = 1, int max = 100) => 
        _random.Next(min, max + 1);

    public static int RandomEnergyId(IList<int> validIds) => 
        validIds.Count > 0 ? validIds[_random.Next(validIds.Count)] : 0;

    public static Dictionary<int, int> GenerateEnergyPurchaseCombinations(
        IList<int> validIds, int minQty = 1, int maxQty = 100)
    {
        var purchases = new Dictionary<int, int>();
        foreach (var id in validIds)
        {
            purchases[id] = RandomQuantity(minQty, maxQty);
        }
        return purchases;
    }

    public static List<Dictionary<string, object>> GenerateInvalidPurchaseData()
    {
        return new List<Dictionary<string, object>>
        {
            new Dictionary<string, object> { ["id"] = -1, ["quantity"] = 1 },
            new Dictionary<string, object> { ["id"] = 1, ["quantity"] = 0 },
            new Dictionary<string, object> { ["id"] = 2, ["quantity"] = -5 },
            new Dictionary<string, object> { ["id"] = 3, ["quantity"] = 999999 }
        };
    }

    public static string RandomOrderId() => 
        Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

    public static DateTime RandomOrderDate(DateTime? start = null, DateTime? end = null)
    {
        start ??= DateTime.Now.AddYears(-1);
        end ??= DateTime.Now;
        
        int range = (end.Value - start.Value).Days;
        return start.Value.AddDays(_random.Next(range));
    }
}