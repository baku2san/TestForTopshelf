using System;

namespace TestForTopShelfAndInstaller.Services
{
    public class MemoryInfo
    {
        /// <summary>
        /// Value に対する特殊な変換を行う場合の設定：あれ？Delegateやめて、単純にこれでSwitchでよかったんじゃね？ TODO: Refactoring出来そうだね
        /// </summary>

        public enum AccessSizes
        {
            BIT = 0,
            BYTE = 1,               // Word Access Address しか無い場合、Byte指定はあり得ない。上位/下位のどちらかが取得不能なので
            WORD = 2,
            DWORD = 4,
            WORD2SmallInt = 12       // Word情報を処理してSmallIntにしたもの
        }
        public string Name { get; set; }
        public UInt32 ReadOffset { get; set; }
        public AccessSizes AccessSize { get; set; }
        public bool IsEnable { get; set; }
        /// <summary>
        /// 既定値の設定。無い場合はNullで、Read実施
        /// </summary>

        public bool HasConvertDefinition { get; set; }  // TODO: DbTables を分離すればこんなのいらんのよね・・
        public double Slope { get; set; }
        public double Intercept { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public string Unit { get; set; }

        public MemoryInfo()
        {
            HasConvertDefinition = true;
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
        public MemoryInfo(string name, UInt32 readOffset = 0, AccessSizes accessSize = AccessSizes.WORD, bool isEnable = true) : this()
        {
            // TODO : 引数確認

            // 初期化
            Name = name;
            AccessSize = accessSize;
            IsEnable = isEnable;
            ReadOffset = readOffset;

        }
        
    }
}
