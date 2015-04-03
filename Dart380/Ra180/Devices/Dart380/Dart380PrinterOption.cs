using System;

namespace Ra180.Devices.Dart380
{
    [Flags]
    public enum Dart380PrinterOption
    {
        Manual = 0,
        Received = 1,
        Transmitted = 2,
        All = Received|Transmitted
    }
}