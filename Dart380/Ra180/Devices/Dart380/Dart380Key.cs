using System;

namespace Ra180
{
    public static class Dart380Key
    {
        public const string Num0 = Ra180Key.Num0;
        public const string Num1 = Ra180Key.Num1;
        public const string Num2 = Ra180Key.Num2;
        public const string Num3 = Ra180Key.Num3;
        public const string Num4 = Ra180Key.Num4;
        public const string Num5 = Ra180Key.Num5;
        public const string Num6 = Ra180Key.Num6;
        public const string Num7 = Ra180Key.Num7;
        public const string Num8 = Ra180Key.Num8;
        public const string Num9 = Ra180Key.Num9;

        public const string Asterix = Ra180Key.Asterix;
        public const string NumberSign = Ra180Key.NumberSign;

        public const string TID = Ra180Key.TID;
        public const string RDA = Ra180Key.RDA;
        public const string DTM = Ra180Key.DTM;
        public const string KDA = Ra180Key.KDA;
        public const string RAP = Ra180Key.RAP;
        public const string NYK = Ra180Key.NYK;
        public const string TJK = Ra180Key.TJK;

        public const string OPM = Ra180Key.OPM;
        public const string EFF = Ra180Key.EFF;
        public const string ÄND = Ra180Key.ÄND;
        public const string BEL = Ra180Key.BEL;
        public const string SLT = Ra180Key.SLT;
        public const string ENT = Ra180Key.ENT;

        public const string NOLLST = Ra180Key.NOLLST;

        public const string ModFRÅN = Ra180Key.ModFRÅN;
        public const string ModTE = "TE";
        public const string ModKLAR = Ra180Key.ModKLAR;
        public const string ModSKYDD = Ra180Key.ModSKYDD;
        public const string ModDRELÄ = Ra180Key.ModDRELÄ;
        public const string ModTD = "TD";
        public const string ModNG = "NG";
        public const string ModFmP = "FmP";

        public const string F1 = "F1";
        public const string F2 = "F2";
        public const string F3 = "F3";
        public const string F4 = "F4";
        public const string FMT = "FMT";
        public const string REP = "REP";
        public const string RAD = "RAD";
        public const string KVI = "KVI";
        public const string SKR = "SKR";
        public const string DDA = "DDA";
        public const string SND = "SND";
        public const string EKV = "EKV";
        public const string MOT = "MOT";
        public const string AVS = "AVS";
        public const string ISK = "ISK";

        public const string LARROW = "LARROW";
        public const string RARROW = "RARROW";
        public const string UARROW = "UARROW";
        public const string DARROW = "DARROW";
        public const string BLANKSTEG = " ";
        public const string PGUP = "PGUP";
        public const string PGDOWN = "PGDOWN";

        public static bool TryParseMod(string key, out Dart380Mod mod)
        {
            switch (key)
            {
                case ModFRÅN:
                    mod = Dart380Mod.FR;
                    return true;

                case ModTE:
                    mod = Dart380Mod.TE;
                    return true;

                case ModKLAR:
                    mod = Dart380Mod.KLAR;
                    return true;

                case ModSKYDD:
                    mod = Dart380Mod.SKYDD;
                    return true;

                case ModDRELÄ:
                    mod = Dart380Mod.DRELÄ;
                    return true;

                case ModTD:
                    mod = Dart380Mod.TD;
                    return true;

                case ModNG:
                    mod = Dart380Mod.NG;
                    return true;

                case ModFmP:
                    mod = Dart380Mod.FmP;
                    return true;

                default:
                    mod = default(Dart380Mod);
                    return false;
            }
        }

        public static string From(Dart380Mod value)
        {
            switch (value)
            {
                case Dart380Mod.FR: return ModFRÅN;
                case Dart380Mod.TE: return ModTE;
                case Dart380Mod.KLAR: return ModKLAR;
                case Dart380Mod.SKYDD: return ModSKYDD;
                case Dart380Mod.DRELÄ: return ModDRELÄ;
                case Dart380Mod.TD: return ModTD;
                case Dart380Mod.NG: return ModTD;
                case Dart380Mod.FmP: return ModFmP;
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }
    }
}