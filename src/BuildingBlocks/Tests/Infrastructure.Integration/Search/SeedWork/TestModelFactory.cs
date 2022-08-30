using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork
{
    public static class TestModelFactory
    {
        public static string GetId(params int[] ids) => $"id-{string.Join("-", ids)}";
        public static string GetString(params int[] ids) => $"string-{string.Join("-", ids)}";

        public static IEnumerable<TestModel> Make(int count)
        {
            var list = new List<TestModel>();
            for (int i = 0; i < count; i++)
            {
                var items = new List<TestModelItem>();
                for (int j = 0; j < count; j++)
                {
                    items.Add(new TestModelItem
                    {
                        Number = j,
                        String = GetString(i, j),
                        DateTime = DateTime.UtcNow.AddDays(i).AddHours(j)
                    });
                    items.Add(new TestModelItemChild1
                    {
                        Number = j,
                        String = GetString(i, j),
                        Bool = j % 2 == 0,
                        DateTime = DateTime.UtcNow.AddHours(j)
                    });
                    items.Add(new TestModelItemChild2
                    {
                        Number = j,
                        String = GetString(i, j),
                        Desc = GetString(i, j),
                        DateTime = DateTime.UtcNow.AddHours(j)
                    });
                }

                list.Add(new TestModel
                {
                    Bool = i % 2 == 0,
                    Id = GetId(i),
                    StringNumber = $"{i}",
                    Number = i,
                    String = GetString(i),
                    Items = items,
                    Item = new TestModelItem
                        {String = GetString(i), Number = i, DateTime = DateTime.UtcNow.AddHours(i)},
                    DateTime = DateTime.UtcNow.AddHours(-i),
                    Enum = (TestModelEnum)(i % 2),
                    StringArray = new List<string>(i).Select((x, j) => GetString(j)),
                    IntArray = new List<int>(i).Select((x, j) => j),
                    Dictionary = new Dictionary<string, string>()
                    {
                        {GetString(i), GetString(i)}
                    }
                });
            }

            return list;
        }
    }
}
