using System.Collections;
using System.Linq;

namespace MM8000.EMU.CLI
{
    public class Intel8085
    {
        public Registers Registers { get; private set; }
        public Memory<byte> Memory { get; } = new Memory<byte>(new byte[65536]);


    }

    public static class OpCodes8085
    {
        private static int CY(this Registers current) => current[CpuFlags.CY] ? 1 : 0;
        private static Registers Step(this Registers current) => current[RegisterPair.PC, (ushort)(current[RegisterPair.PC] + 1)];

        private static (byte result, CpuFlags flags) Add(byte a, byte b, int cy)
        {
            var result = a + b + cy;
            var flags = CpuFlags.None;

            if ((result & 0x100) != 0) flags |= CpuFlags.CY;
            if ((new BitArray(new[] { result }).Cast<bool>().Count(b => b) % 2) == 0) flags |= CpuFlags.PE;
            if (result == 0) flags |= CpuFlags.Z;
            if ((result & 0x80) != 0) flags |= CpuFlags.S;

            //[Description("Signed Overflow")]
            //V = 1 << 1,
            //[Description("Aux Carry")]
            //AC = 1 << 4,
            //[Description("Sign XOR Overflow")]
            //K = 1 << 5,

            return ((byte)result, flags);
        }



        public static Registers NOP(this Memory<byte> ram, Registers current) => current.Step();

        public static Registers ACI(this Memory<byte> ram, Registers current, byte data)
        {
            var value = current[Register.A] + data + current.CY();
            //var flags = value.GetFlags8();
            return current.Step()[Register.A, (byte)(current[Register.A] + data + current.CY())];
        }


    }
}
