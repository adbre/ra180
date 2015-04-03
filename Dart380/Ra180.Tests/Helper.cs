using Ra180.Devices.Dart380;

namespace Ra180.Tests
{
    public sealed class Helper
    {
        public static Ra180DataKey EnterNewPny(Ra180 ra180)
        {
            var key = Ra180DataKey.Generate();
            EnterNewPny(ra180, key);
            return key;
        }

        public static Ra180DataKey EnterNewPny(Dart380 dart380)
        {
            var key = Ra180DataKey.Generate();
            EnterNewPny(dart380, key);
            return key;
        }

        public static void EnterNewPny(Ra180 ra180, Ra180DataKey key)
        {
            EnterNewPny(ra180, ra180.Display, key);
        }

        public static void EnterNewPny(Dart380 ra180, Ra180DataKey key)
        {
            EnterNewPny(ra180, ra180.SmallDisplay, key);
        }

        public static void EnterNewPny(Ra180Device ra180, Ra180Display display, Ra180DataKey key)
        {
            ra180.SendKey(Ra180Key.KDA); // FR
            ra180.SendKey(Ra180Key.ENT); // BD1
            ra180.SendKey(Ra180Key.ENT); // BD2/SYNK
            if (display.ToString().StartsWith("BD2"))
                ra180.SendKey(Ra180Key.ENT); // SYNK
            ra180.SendKey(Ra180Key.ENT); // PNY
            ra180.SendKey(Ra180Key.ÄND);

            foreach (var group in key.Data)
            {
                ra180.SendKeys(group);
                ra180.SendKey(Ra180Key.ENT);
            }

            ra180.SendKey(Ra180Key.SLT);
        }
    }
}