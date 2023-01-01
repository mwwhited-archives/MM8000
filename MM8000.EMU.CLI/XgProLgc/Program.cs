using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace XgProLgc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var testFile = @"C:\Wincupl\Projects\IO-Selector.lgc";
            var data = File.ReadAllBytes(testFile);
            Span<byte> seq = data;
            var header = MemoryMarshal.Cast<byte, LgcFileHeader>(seq)[0];

            var pointers = MemoryMarshal.Cast<byte, int>(seq[16..])[0..512];
            var entryPointers = pointers.Slice(0, (int)header.ItemCount);


            var entryPointer = seq[entryPointers[0]..];
            var item = new LgcFileItem()
            {
                VectorCount = MemoryMarshal.Cast<byte, int>(entryPointer)[0],
                ItemName = ASCIIEncoding.ASCII.GetString(entryPointer[4..36]).TrimEnd('\0'),
                VoltageLevel = MemoryMarshal.Cast<byte, LgcVoltages>(entryPointer[36..])[0],
                PinCount = entryPointer[37],
                Res0 = entryPointer[38],
                Res1 = entryPointer[39],
                UiRes = MemoryMarshal.Cast<byte, uint>(entryPointer[40..])[0],
            };

            var pinSpan = entryPointer[44..];

            var vectors = new LgcVectors[item.VectorCount];
            for (var v = 0; v < vectors.Length; v++)
            {
                var vectorOffset = v * 24;
                //MemoryMarshal.
                var vectorSpan = pinSpan.Slice(vectorOffset, 24).ToArray();

                var q = vectorSpan.SelectMany(b => new[] { (LgcPinType)(b & 0xf), (LgcPinType)((b >> 4) & 0xf) }).Take(item.PinCount).ToArray();
                vectors[v] = new LgcVectors { Pins = q, };
            }
            var entry = new LgcFileEntry
            {
                FileItem = item,
                LogicVectors = vectors,
            };
        }
    }

    public static class LgcGlobals
    {
        public static readonly byte[] FileFlag = new byte[] { 0xab, 0xab, 0xab, 0xee, };
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct LgcFileHeader
    {
        [FieldOffset(0)]
        public uint AllCrc32;
        [FieldOffset(4)]
        public uint UiFlag;
        [FieldOffset(8)]
        public uint ItemCount;
        [FieldOffset(12)]
        public uint Res;
        //[FieldOffset(16)]
        //public uint[] ItemStart;
    }

    public struct LgcVectors
    {
        public LgcPinType[] Pins;
    }

    public struct LgcFileEntry
    {
        public LgcFileItem FileItem;
        public LgcVectors[] LogicVectors;
    }

    public struct LgcFile
    {
        public LgcFileHeader Header;
        public LgcFileEntry[] Entries;
    }
}
