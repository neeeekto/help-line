using System;
using System.Collections.Generic;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork
{
    public class TestModel
    {

        public string Id { get; set; }
        public string StringNumber { get; set; }
        public string String { get; set; }
        public bool Bool { get; set; }
        public int Number { get; set; }
        public TestModelEnum Enum { get; set; }
        public DateTime DateTime { get; set; }
        public TestModelItem Item { get; set; }
        public Dictionary<string, string> Dictionary { get; set; }
        public IEnumerable<string> StringArray { get; set; }
        public IEnumerable<int> IntArray { get; set; }

        public IEnumerable<TestModelItem> Items { get; set; }
    }

    public enum TestModelEnum
    {
        Enum1 = 0,
        Enum2 = 1
    }

    public class TestModelExtend : TestModel
    {
        public long Long { get; set; }
    }
}
