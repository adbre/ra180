using System;

namespace Ra180
{
    internal static class Ra180Converter
    {
        public static Ra180KeyCode ToKeyCode(Ra180Mod mod)
        {
            switch (mod)
            {
                case Ra180Mod.FR: return Ra180KeyCode.ModFR;
                case Ra180Mod.KLAR: return Ra180KeyCode.ModKLAR;
                case Ra180Mod.SKYDD: return Ra180KeyCode.ModSKYDD;
                case Ra180Mod.DRELÄ: return Ra180KeyCode.ModDRELÄ;
                default:
                    throw new ArgumentOutOfRangeException("mod");
            }
        }

        public static Ra180KeyCode ToKeyCode(Ra180Channel value)
        {
            switch (value)
            {
                case Ra180Channel.Channel1: return Ra180KeyCode.Channel1;
                case Ra180Channel.Channel2: return Ra180KeyCode.Channel2;
                case Ra180Channel.Channel3: return Ra180KeyCode.Channel3;
                case Ra180Channel.Channel4: return Ra180KeyCode.Channel4;
                case Ra180Channel.Channel5: return Ra180KeyCode.Channel5;
                case Ra180Channel.Channel6: return Ra180KeyCode.Channel6;
                case Ra180Channel.Channel7: return Ra180KeyCode.Channel7;
                case Ra180Channel.Channel8: return Ra180KeyCode.Channel8;
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }

        public static Ra180KeyCode ToKeyCode(Ra180Volume value)
        {
            switch (value)
            {
                case Ra180Volume.Volume1: return Ra180KeyCode.Volume1;
                case Ra180Volume.Volume2: return Ra180KeyCode.Volume2;
                case Ra180Volume.Volume3: return Ra180KeyCode.Volume3;
                case Ra180Volume.Volume4: return Ra180KeyCode.Volume4;
                case Ra180Volume.Volume5: return Ra180KeyCode.Volume5;
                case Ra180Volume.Volume6: return Ra180KeyCode.Volume6;
                case Ra180Volume.Volume7: return Ra180KeyCode.Volume7;
                case Ra180Volume.Volume8: return Ra180KeyCode.Volume8;
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }

        public static Ra180Mod ToMod(Ra180KeyCode keyCode)
        {
            switch (keyCode)
            {
                case Ra180KeyCode.ModFR: return Ra180Mod.FR;
                case Ra180KeyCode.ModKLAR: return Ra180Mod.KLAR;
                case Ra180KeyCode.ModSKYDD: return Ra180Mod.SKYDD;
                case Ra180KeyCode.ModDRELÄ: return Ra180Mod.DRELÄ;
                default:
                    throw new ArgumentOutOfRangeException("keyCode");
            }
        }
    }
}