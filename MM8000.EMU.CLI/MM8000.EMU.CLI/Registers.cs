using System.Runtime.InteropServices;

namespace MM8000.EMU.CLI
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Registers
    {
        [FieldOffset((int)Register.B)] public ushort BC;
        [FieldOffset((int)Register.B)] public byte B;
        [FieldOffset((int)Register.C)] public byte C;

        [FieldOffset((int)Register.D)] public ushort DE;
        [FieldOffset((int)Register.D)] public byte D;
        [FieldOffset((int)Register.E)] public byte E;

        [FieldOffset((int)Register.H)] public ushort HL;
        [FieldOffset((int)Register.H)] public byte H;
        [FieldOffset((int)Register.L)] public byte L;

        [FieldOffset((int)Register.A)] public ushort Af;
        [FieldOffset((int)Register.A)] public byte A;
        [FieldOffset((int)Register.F)] public CpuFlags Flags;

        [FieldOffset((int)Register.A + 1)] public ushort SP;
        [FieldOffset((int)Register.A + 3)] public ushort PC;

        public byte this[Register register] =>
            register switch
            {
                Register.A => A,
                Register.B => B,
                Register.C => C,
                Register.D => D,
                Register.E => E,
                Register.H => H,
                Register.L => L,
                _ => throw new NotSupportedException($"{register}")
            };
        public Registers this[Register register, byte data8] =>
                register switch
                {
                    Register.A => this with { A = data8 },
                    Register.B => this with { B = data8 },
                    Register.C => this with { C = data8 },
                    Register.D => this with { D = data8 },
                    Register.H => this with { H = data8 },
                    Register.L => this with { L = data8 },
                    Register.F => this with { Flags = (CpuFlags)data8 },
                    _ => throw new NotSupportedException($"{register}")
                };

        public ushort this[RegisterPair registerPair] =>
            registerPair switch
            {
                RegisterPair.BC => BC,
                RegisterPair.DE => DE,
                RegisterPair.HL => HL,
                RegisterPair.SP => SP,
                RegisterPair.Af => Af,
                RegisterPair.PC => PC,
                _ => throw new NotSupportedException($"{registerPair}")
            };
        public Registers this[RegisterPair registerPair, ushort data16] =>
            registerPair switch
            {
                RegisterPair.BC => this with { BC = data16 },
                RegisterPair.DE => this with { DE = data16 },
                RegisterPair.HL => this with { HL = data16 },
                RegisterPair.SP => this with { SP = data16 },
                RegisterPair.Af => this with { Af = data16 },
                RegisterPair.PC => this with { PC = data16 },
                _ => throw new NotSupportedException($"{registerPair}")
            };

        public bool this[CpuFlags flag] => Flags.HasFlag(flag);

        public Registers this[(byte result, CpuFlags flags) ac] => this[ac.result, ac.flags];
        public Registers this[byte result, CpuFlags flags] => this with { A = result, Flags = flags, };

    }
}
