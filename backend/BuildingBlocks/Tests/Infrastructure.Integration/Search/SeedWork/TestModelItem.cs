using System;

namespace HelpLine.BuildingBlocks.Infrastructure.IntegrationTests.Search.SeedWork
{
    public class TestModelItem
    {
        public string String { get; set; }
        public int Number { get; set; }

        public DateTime DateTime { get; set; }

    }

    public class TestModelItemChild1 : TestModelItem
    {
        public bool Bool { get; set; }


    }

    public class TestModelItemChild2 : TestModelItem
    {
        public string Desc { get; set; }
    }
}
