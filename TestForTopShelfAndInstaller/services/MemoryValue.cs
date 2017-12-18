using System;

namespace TestForTopShelfAndInstaller.Services
{
    public class MemoryValue
    {
        /// <summary>
        /// Object として、各型でそのまま入れ、DBへもそのまま流すようにする
        /// </summary>
        public Object Value {get; set;}

        public MemoryInfo MemoryInfo { get; set; }

        public MemoryValue(MemoryInfo memoryInfo, Object value)
        {
            MemoryInfo = memoryInfo;
            Value = value;
        }
    }
}
