using System;

namespace ServiceTests
{
    internal class TestPriorityAttribute : Attribute
    {
        public int Priority { get; private set; }

        public TestPriorityAttribute(int priority) => Priority = priority;
    }
}