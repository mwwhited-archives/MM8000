using System.Collections;

namespace MM8000.EMU.CLI
{
    public static class UtilityFunctions
    {
        public static bool EvenParity(this int value) =>
            new BitArray(new[] { value }).Cast<bool>().Aggregate(false, (a, i) => a ^ i);

        public static bool Signed8(this int value) => (value & 0x80) != 0;
        public static bool Signed16(this int value) => (value & 0x8000) != 0;

        public static bool Carry8(this int value) => (value & 0x100) != 0;
        public static bool Carry16(this int value) => (value & 0x10000) != 0;

        public static bool AuxCarryAdd(this int r, int a, int b)
        {
            var a4 = (a & 0b1000) != 0;
            var b4 = (b & 0b1000) != 0;
            var r4 = (r & 0b1000) != 0;

            // assign auxa = (~iA[4] & ~iB[4] & oR[4]) | (~iA[4] & iB[4] & ~oR[4]) | (iA[4] & ~iB[4] & ~oR[4]) | (iA[4] & iB[4] & oR[4]);//adc and aci

            var auxc =
                (!a4 && !b4 && r4) |
                (!a4 && b4 && !r4) |
                (a4 && !b4 && !r4) |
                (a4 && b4 && r4)
                ;

            return auxc;
        }

        public static bool AuxCarrySubtract(this int r, int a, int b)
        {
            var a3 = (a & 0b100) != 0;
            var b3 = (b & 0b100) != 0;
            var r3 = (r & 0b100) != 0;

            // assign auxb = (~iA[3] & iB[3] & ~oR[3]) | (~iA[3] & iB[3] & oR[3]) | (iA[3] & iB[3] & oR[3]);//sbb and cmp

            var auxc =
                (!a3 && b3 && !r3) |
                (!a3 && b3 && r3) |
                (a3 && b3 && r3)
                ;

            return auxc;
        }
    }
}
