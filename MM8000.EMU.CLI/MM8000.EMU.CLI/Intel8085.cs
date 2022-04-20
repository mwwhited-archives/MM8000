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
        internal static int CY(this Registers current) => current[CpuFlags.CY] ? 1 : 0;
        internal static Registers Step(this Registers current, int step = 1) => current[RegisterPair.PC, (ushort)(current[RegisterPair.PC] + step)];

        internal static byte GetValue(this Memory<byte> ram, Registers current, Register register) =>
                register switch
                {
                    Register.M => GetByHL(ram, current),
                    _ => current[register]
                };

        internal static CpuFlags GetSimpleFlags(this int result)
        {
            var flags = CpuFlags.None;

            if (result.Carry8()) flags |= CpuFlags.CY;
            if (result.EvenParity()) flags |= CpuFlags.PE;
            if (result == 0) flags |= CpuFlags.Z;
            if (result.Signed8()) flags |= CpuFlags.S;

            return flags;
        }

        internal static (byte result, CpuFlags flags) Add(byte a, byte b, int cy)
        {
            var result = a + b + cy;
            var flags = result.GetSimpleFlags();
            if (result.AuxCarryAdd(a, b)) flags |= CpuFlags.AC;

            //[Description("Signed Overflow")]
            //V = 1 << 1,
            //[Description("Sign XOR Overflow")]
            //K = 1 << 5,

            return ((byte)result, flags);
        }
        internal static (byte result, CpuFlags flags) Sub(byte a, byte b, int cy)
        {
            var result = a - b - cy;
            var flags = result.GetSimpleFlags();
            if (result.AuxCarrySubtract(a, b)) flags |= CpuFlags.AC;

            //[Description("Signed Overflow")]
            //V = 1 << 1,
            //[Description("Sign XOR Overflow")]
            //K = 1 << 5,

            return ((byte)result, flags);
        }
        internal static byte GetByHL(Memory<byte> ram, Registers current) => ram.Span[current.HL];

        public static Registers NOP(Memory<byte> ram, Registers current) => current.Step();

        public static Registers ACI(Memory<byte> ram, Registers current, byte data) =>
            current[Add(current[Register.A], data, current.CY())].Step(2);
        public static Registers ADC(Memory<byte> ram, Registers current, Register register) =>
            current[Add(current[Register.A], ram.GetValue(current, register), current.CY())].Step();

        public static Registers ADI(Memory<byte> ram, Registers current, byte data) =>
            current[Add(current[Register.A], data, 0)].Step(2);
        public static Registers ADD(Memory<byte> ram, Registers current, Register register) =>
            current[Add(current[Register.A], ram.GetValue(current, register), 0)].Step();

        public static Registers ANI(Memory<byte> ram, Registers current, byte data) =>
            current[((byte)(current[Register.A] & data), current.Flags & ~CpuFlags.CY | CpuFlags.AC)].Step(2);
        public static Registers ANA(Memory<byte> ram, Registers current, Register register) =>
            current[((byte)(current[Register.A] & ram.GetValue(current, register)), current.Flags & ~CpuFlags.CY | CpuFlags.AC)].Step();


    }
}
