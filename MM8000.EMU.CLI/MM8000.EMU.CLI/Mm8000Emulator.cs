namespace MM8000.EMU.CLI
{
    public struct Mm8000Emulator
    {
        public Intel8085 Cpu { get; }
        public Memory<byte> Rom { get; }
        public Memory<byte> Ram { get; }
        public Memory<byte> Io { get; }

        /*
         * Memory Map
         * 
         * 0000-07FF ROM
         * 0800-7FFF Unused
         * 8000-80FF RAM
         * 8100-FFFF Unused
         * 
         * IO - Map
         */

        /*
         * Monitor 
         * RAM: 80DC-80FF: 
         * 
         * Function Map
         *  X2  = 0000: (Restart Monitor)
         *  R   = 0102: Display value at Address AH:AL and incerment address
         *  X1  = 0000: (Restart Monitor)
         *  DA  = 80E7: Display value at AH:AL
         *  GO  = 0100: 
         *  AL  = 80E8:
         *  ST  = 80DC:
         *  AH  = 80E9:
         *  
         */

        /* ROM
         *  Start           0000-0002
         *  Unused          0003-003F
         *  Pattern Table   0040-004F
         *  Function Table  0050-005F: X2, R, X1, DA, GO, AL, ST, AH
         *  Init            0060-0065
         *  Copy            0006-0073
         *  Link            0074-0076
         *  Display         0077-0093
         *  RDIG            0094-00A1
         *  Scan            00A2-00A7
         *  New Drv         00A8-00B6
         *  Test Key        00B7-00D3
         *  K FND           00D4-00E1
         *  D FND           00E2-00F0
         *  F FND           00F1-00FF
         *  FGO             0100-0101
         *  FR              0102-012D
         */

        /*
         Write a loop to display pattern
        Rgedcbaf Lgedcbaf

        Port B = 128, 32, 16, 4, 2, 64
        Port C[0] = 1:L, 0:R

        g=128
        f=64
        e=32
        d=16
        c=8
        b=4
        a=2
        */
    }

}
