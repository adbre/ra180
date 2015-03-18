using System;

namespace Ra180
{
    [Flags]
    public enum Ra180KeyCode : long
    {
        OPM = 1,
        EFF = 2,
        ÄND = 4,
        
        BEL = 8,
        SLT = 16,
        ENT = 32,

        Num0 = 64,
        Num1 = 128,
        Num2 = 256,
        Num3 = 512,
        Num4 = 1024,
        Num5 = 2048,
        Num6 = 4096,
        Num7 = 8192,
        Num8 = 16384,
        Num9 = 32768,

        Asterix = 65536,
        NumberSign = 131072,

        Channel1 = 262144,
        Channel2 = 524288,
        Channel3 = 1048576,
        Channel4 = 2097152,
        Channel5 = 4194304,
        Channel6 = 8388608,
        Channel7 = 16777216,
        Channel8 = 33554432,

        Volume1 = 67108864,
        Volume2 = 134217728,
        Volume3 = 268435456,
        Volume4 = 536870912,
        Volume5 = 1073741824,
        Volume6 = 2147483648,
        Volume7 = 4294967296,
        Volume8 = 8589934592,

        ModFR = 17179869184,
        ModKLAR = 34359738368,
        ModSKYDD = 68719476736,
        ModDRELÄ = 137438953472,

        TID = Num1,
        RDA = Num2,
        DTM = Num3,
        KDA = Num4,
        NIV = Num5,
        RAP = Num6,
        NYK = Num7,
        TJK = Num9,

        RESET = Asterix|NumberSign
    }
}