using System.ComponentModel;

namespace MM8000.EMU.CLI
{
    [Flags]
    public enum CpuFlags : byte
    {
        None = 0,

        [Description("Carry")]
        CY = 1 << 0,
        [Description("Signed Overflow")]
        V = 1 << 1,
        [Description("Parity Even")]
        PE = 1 << 2,
        [Description("Always Zero")]
        X = 1 << 3,
        [Description("Aux Carry")]
        AC = 1 << 4,
        [Description("Sign XOR Overflow")]
        K = 1 << 5,
        [Description("Zero")]
        Z = 1 << 6,
        [Description("Signed")]
        S = 1 << 7,
    }
}
