using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ra180
{
    public static class Dart380Key
    {
        public const string ModFRÅN = Ra180Key.ModFRÅN;
        public const string ModTE = "TE";
        public const string ModKLAR = Ra180Key.ModKLAR;
        public const string ModSKYDD = Ra180Key.ModSKYDD;
        public const string ModDRELÄ = Ra180Key.ModDRELÄ;
        public const string ModTD = "TD";
        public const string ModNG = "NG";
        public const string ModFmP = "FmP";

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

                case ModFmP:
                    mod = Dart380Mod.FmP;
                    return true;

                default:
                    mod = default(Dart380Mod);
                    return false;
            }
        }
    }

    public static class Ra180Key
    {
        public const string Num0 = "0";
        public const string Num1 = "1";
        public const string Num2 = "2";
        public const string Num3 = "3";
        public const string Num4 = "4";
        public const string Num5 = "5";
        public const string Num6 = "6";
        public const string Num7 = "7";
        public const string Num8 = "8";
        public const string Num9 = "9";

        public const string Asterix = "*";
        public const string NumberSign = "#";

        public const string TID = Num1;
        public const string RDA = Num2;
        public const string DTM = Num3;
        public const string KDA = Num4;
        public const string NIV = Num5;
        public const string RAP = Num6;
        public const string NYK = Num7;
        public const string TJK = Num9;

        public const string OPM = "OPM";
        public const string EFF = "EFF";
        public const string ÄND = "ÄND";
        public const string BEL = "BEL";
        public const string SLT = "SLT";
        public const string ENT = "ENT";

        public const string NOLLST = Asterix + NumberSign;
        public const string RESET = NOLLST;

        public const string ModFRÅN = "FRÅN";
        public const string ModKLAR = "KLAR";
        public const string ModSKYDD = "SKYDD";
        public const string ModDRELÄ = "DRELÄ";

        public const string Channel1 = "KANAL-1";
        public const string Channel2 = "KANAL-2";
        public const string Channel3 = "KANAL-3";
        public const string Channel4 = "KANAL-4";
        public const string Channel5 = "KANAL-5";
        public const string Channel6 = "KANAL-6";
        public const string Channel7 = "KANAL-7";
        public const string Channel8 = "KANAL-8";

        public const string Volume1 = "VOLYM-1";
        public const string Volume2 = "VOLYM-2";
        public const string Volume3 = "VOLYM-3";
        public const string Volume4 = "VOLYM-4";
        public const string Volume5 = "VOLYM-5";
        public const string Volume6 = "VOLYM-6";
        public const string Volume7 = "VOLYM-7";
        public const string Volume8 = "VOLYM-8";

        public static bool IsModKey(string key)
        {
            var modKeys = new[]
            {
                ModFRÅN,
                ModKLAR,
                ModSKYDD,
                ModDRELÄ
            };

            return modKeys.Contains(key);
        }

        public static string From(Ra180Mod value)
        {
            switch (value)
            {
                case Ra180Mod.FR:
                    return ModFRÅN;
                case Ra180Mod.KLAR:
                    return ModKLAR;
                case Ra180Mod.SKYDD:
                    return ModSKYDD;
                case Ra180Mod.DRELÄ:
                    return ModDRELÄ;
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }

        public static string From(Ra180Channel value)
        {
            switch (value)
            {
                case Ra180Channel.Channel1: return Channel1;
                case Ra180Channel.Channel2: return Channel2;
                case Ra180Channel.Channel3: return Channel3;
                case Ra180Channel.Channel4: return Channel4;
                case Ra180Channel.Channel5: return Channel5;
                case Ra180Channel.Channel6: return Channel6;
                case Ra180Channel.Channel7: return Channel7;
                case Ra180Channel.Channel8: return Channel8;
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }

        public static string From(Ra180Volume value)
        {
            switch (value)
            {
                case Ra180Volume.Volume1: return Volume1;
                case Ra180Volume.Volume2: return Volume2;
                case Ra180Volume.Volume3: return Volume3;
                case Ra180Volume.Volume4: return Volume4;
                case Ra180Volume.Volume5: return Volume5;
                case Ra180Volume.Volume6: return Volume6;
                case Ra180Volume.Volume7: return Volume7;
                case Ra180Volume.Volume8: return Volume8;
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }

        public static bool TryParseMod(string key, out Ra180Mod mod)
        {
            var map = new Dictionary<string, Ra180Mod>
            {
                {ModFRÅN, Ra180Mod.FR},
                {ModKLAR, Ra180Mod.KLAR},
                {ModSKYDD, Ra180Mod.SKYDD},
                {ModDRELÄ, Ra180Mod.DRELÄ},
            };

            return map.TryGetValue(key, out mod);
        }

        public static bool TryParseChannel(string key, out Ra180Channel channel)
        {
            var map = new Dictionary<string, Ra180Channel>
            {
                {Channel1, Ra180Channel.Channel1},
                {Channel2, Ra180Channel.Channel2},
                {Channel3, Ra180Channel.Channel3},
                {Channel4, Ra180Channel.Channel4},
                {Channel5, Ra180Channel.Channel5},
                {Channel6, Ra180Channel.Channel6},
                {Channel7, Ra180Channel.Channel7},
                {Channel8, Ra180Channel.Channel8},
            };

            return map.TryGetValue(key, out channel);
        }

        public static bool TryParseVolume(string key, out Ra180Volume volume)
        {
            var map = new Dictionary<string, Ra180Volume>
            {
                {Volume1, Ra180Volume.Volume1},
                {Volume2, Ra180Volume.Volume2},
                {Volume3, Ra180Volume.Volume3},
                {Volume4, Ra180Volume.Volume4},
                {Volume5, Ra180Volume.Volume5},
                {Volume6, Ra180Volume.Volume6},
                {Volume7, Ra180Volume.Volume7},
                {Volume8, Ra180Volume.Volume8},
            };

            return map.TryGetValue(key, out volume);
        }
    }
}