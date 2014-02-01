using System;

namespace C42A.Ra180.Infrastructure
{
    [Flags]
    public enum Ra180Knapp : ulong
    {
// ReSharper disable InconsistentNaming
        OPM = 1,
        EFF = 2,
        ÄND = 4,
        BEL = 8,
        SLT = 16,
        ENT = 32,

        Knapp1 = 64,
        Knapp2 = 128,
        Knapp3 = 256,
        Knapp4 = 512,
        Knapp5 = 1024,
        Knapp6 = 2048,
        Knapp7 = 4096,
        Knapp8 = 8192,
        Knapp9 = 16384,
        KnappAsterix = 32768,
        Knapp0 = 65536,
        KnappHashtag = 131072,

        Volym1 = 262144,
        Volym2 = 524288,
        Volym3 = 1048576,
        Volym4 = 2097152,
        Volym5 = 4194304,
        Volym6 = 8388608,
        Volym7 = 16777216,
        Volym8 = 33554432,

        Kanal1 = 67108864,
        Kanal2 = 134217728,
        Kanal3 = 268435456,
        Kanal4 = 536870912,
        Kanal5 = 1073741824,
        Kanal6 = 2147483648,
        Kanal7 = 4294967296,
        Kanal8 = 8589934592,

        ModFrån = 17179869184,
        ModKlar = 34359738368,
        ModSkydd = 68719476736,
        ModDRelä = 137438953472,
    }
}