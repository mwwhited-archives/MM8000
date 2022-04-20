namespace MM8000.EMU.CLI
{
    public enum RegisterPair
    {
        BC = 0b00,
        DE = 0b01,
        HL = 0b10,
        SP = 0b11,

        Af = 0x80 | SP,
        PC = 0x80 | 0b00,
    }
}
