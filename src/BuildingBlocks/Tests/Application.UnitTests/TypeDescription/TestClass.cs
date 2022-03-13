using System;
using System.Collections.Generic;

namespace HelpLine.BuildingBlocks.Application.UnitTests.TypeDescription
{
    public enum TestEnum
    {
        A,
        B,
        C
    }

    public class TestSubClass
    {
        public string Property { get; set; }
        public IEnumerable<string> List { get; set; }
    }

    public class TestClass
    {
        public string String { get; set; }
        public int Number { get; set; }
        public bool Boolean { get; set; }
        public DateTime Date { get; set; }
        public TestEnum Enum { get; set; }
        public IEnumerable<string> StringList { get; set; }
        public IEnumerable<TestEnum> EnumList { get; set; }
        public TestSubClass Deep1 { get; set; }
        public TestSubClass Deep2 { get; set; }
    }
}
