using System;

namespace Ra180
{
    public class Dart380
    {
        private readonly LedDisplay _largeDisplay= new LedDisplay(16);
        private readonly LedDisplay _smallDisplay = new LedDisplay(8);

        public LedDisplay LargeDisplay { get { return _largeDisplay; } }
        public LedDisplay SmallDisplay { get { return _smallDisplay; } }

        public Ra180Channel Channel
        {
            get { throw new NotImplementedException(); }
            set { SendKey(Dart380Converter.ToKeyCode(value)); }
        }

        public Dart380Mod Mod
        {
            get { throw new NotImplementedException(); }
            set { SendKey(Dart380Converter.ToKeyCode(value)); }
        }

        public Ra180Volume Volume
        {
            get { throw new NotImplementedException(); }
            set { SendKey(Dart380Converter.ToKeyCode(value)); }
        }

        public void SendKeys(params Dart380KeyCode[] keys)
        {
            foreach (var key in keys)
                SendKey(key);
        }

        public void SendKey(Dart380KeyCode key)
        {
            SendKey(new Dart380KeyEventArgs {KeyCode = key});
        }

        public void SendKey(Dart380KeyEventArgs args)
        {
            
        }
    }

    internal static class Dart380Converter
    {
        public static Dart380KeyCode ToKeyCode(Dart380Mod mod)
        {
            switch (mod)
            {
                case Dart380Mod.FR: return Dart380KeyCode.ModFR;
                case Dart380Mod.TE: return Dart380KeyCode.ModTE;
                case Dart380Mod.KLAR: return Dart380KeyCode.ModKLAR;
                case Dart380Mod.SKYDD: return Dart380KeyCode.ModSKYDD;
                case Dart380Mod.DRELÄ: return Dart380KeyCode.ModDRELÄ;
                case Dart380Mod.TD: return Dart380KeyCode.ModTD;
                case Dart380Mod.NG: return Dart380KeyCode.ModNG;
                case Dart380Mod.FmP: return Dart380KeyCode.ModFmP;
                default:
                    throw new ArgumentOutOfRangeException("mod");
            }
        }

        public static Dart380KeyCode ToKeyCode(Ra180Channel channel)
        {
            switch (channel)
            {
                case Ra180Channel.Channel1: return Dart380KeyCode.Channel1;
                case Ra180Channel.Channel2: return Dart380KeyCode.Channel2;
                case Ra180Channel.Channel3: return Dart380KeyCode.Channel3;
                case Ra180Channel.Channel4: return Dart380KeyCode.Channel4;
                case Ra180Channel.Channel5: return Dart380KeyCode.Channel5;
                case Ra180Channel.Channel6: return Dart380KeyCode.Channel6;
                case Ra180Channel.Channel7: return Dart380KeyCode.Channel7;
                case Ra180Channel.Channel8: return Dart380KeyCode.Channel8;
                default:
                    throw new ArgumentOutOfRangeException("channel");
            }
        }

        public static Dart380KeyCode ToKeyCode(Ra180Volume volume)
        {
            switch (volume)
            {
                case Ra180Volume.Volume1: return Dart380KeyCode.Volume1;
                case Ra180Volume.Volume2: return Dart380KeyCode.Volume2;
                case Ra180Volume.Volume3: return Dart380KeyCode.Volume3;
                case Ra180Volume.Volume4: return Dart380KeyCode.Volume4;
                case Ra180Volume.Volume5: return Dart380KeyCode.Volume5;
                case Ra180Volume.Volume6: return Dart380KeyCode.Volume6;
                case Ra180Volume.Volume7: return Dart380KeyCode.Volume7;
                case Ra180Volume.Volume8: return Dart380KeyCode.Volume8;
                default:
                    throw new ArgumentOutOfRangeException("volume");
            }
        }
    }

    public enum Dart380Mod
    {
        FR = 1,
        TE = 2,
        KLAR = 3,
        SKYDD = 4,
        DRELÄ = 5,
        TD = 6,
        NG = 7,
        FmP = 8
    }

    public class Dart380KeyEventArgs
    {
        public Dart380Modifier Modifier { get; set; }
        public Dart380KeyCode KeyCode { get; set; }
    }

    public enum Dart380Modifier
    {
        None = 0,
        Shift = 1,
    }

    public enum Dart380KeyCode
    {
        None,

        F1,
        F2,
        F3,
        F4,

        NumberSign,
        Asterix,

        FMT,
        REP,
        RAD,
        KVI,
        SKR,
        DDA,
        SND,
        EKV,
        MOT,
        AVS,
        ISK,
        OPM,
        EFF,
        ÄND,
        BEL,
        SLT,
        ENT,

        DEL,

        PageDown,
        PageUp,

        Left,
        Right,
        Up,
        Down,

        Num0,
        Num1,
        Num2,
        Num3,
        Num4,
        Num5,
        Num6,
        Num7,
        Num8,
        Num9,

        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z,
        Å,
        Ä,
        Ö,

        Channel1,
        Channel2,
        Channel3,
        Channel4,
        Channel5,
        Channel6,
        Channel7,
        Channel8,

        ModFR,
        ModTE,
        ModKLAR,
        ModSKYDD,
        ModDRELÄ,
        ModTD,
        ModNG,
        ModFmP,

        Volume1,
        Volume2,
        Volume3,
        Volume4,
        Volume5,
        Volume6,
        Volume7,
        Volume8
    }
}
