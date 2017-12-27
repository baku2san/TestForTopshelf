using System;

namespace JMLoggerApp.Producers
{
    public class MemoryValue
    {
        public Object Value { get; set; }

        public MemoryInfo MemoryInfo { get; set; }

        public MemoryValue(MemoryInfo memoryInfo, Object value)
        {
            MemoryInfo = memoryInfo;
            Value = value;
        }
    }
}
