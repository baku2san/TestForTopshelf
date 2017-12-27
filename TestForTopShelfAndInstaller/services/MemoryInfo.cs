using System;

namespace JMLoggerApp.Producers
{
    public class MemoryInfo
    {
        public enum AccessSizes
        {
            BIT = 0,
            BYTE = 1,
            WORD = 2,
            DWORD = 4,
        }
        public string Name { get; set; }
        public UInt32 ReadOffset { get; set; }
        public AccessSizes AccessSize { get; set; }

        public MemoryInfo()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="memoryType">CIO はBitのみ</param>
        /// <param name="readOffset"></param>
        /// <param name="accessSize">CIO はBit限定</param>
        /// <param name="isEnable"></param>
        /// <param name="bitPlace"></param>
        public MemoryInfo(string name,  UInt32 readOffset = 0, AccessSizes accessSize = AccessSizes.WORD) : this()
        {
            Name = name;
            AccessSize = accessSize;
            ReadOffset = readOffset;

        }
    }
}
