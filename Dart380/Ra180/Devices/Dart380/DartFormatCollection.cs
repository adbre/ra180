using System.Collections.Generic;
using System.Linq;
using Ra180.Programs;

namespace Ra180.Devices.Dart380
{
    public class DartFormatCollection
    {
        private readonly List<DartFormat> _formats = new List<DartFormat>();

        public DartFormatCollection()
        {
            AddFormat(000, "RADIOFMT", "RADIOFMT", "RADIOFORMAT*", "RADIOFMT");

            AddFormat(100, "LEDNING ", "LEDNSBTJ", "FRI*TEXT*       ", "FRI*TEXT", "TEXT:");
            AddFormat(101, "LEDNING ", "LEDNSBTJ", "PASSNINGSALT*   ", "PASS*ALT");
            AddFormat(102, "LEDNING ", "LEDNSBTJ", "RADIONYCKEL*KRY*", "PASS*ALT");
            AddFormat(103, "LEDNING ", "LEDNSBTJ", "FÖRBINDELSEPROV*", "FÖRBPROV");
            AddFormat(104, "LEDNING ", "LEDNSBTJ", "KVITTENS        ", "KVITTENS");
            AddFormat(105, "LEDNING ", "LEDNSBTJ", "REPETERA*       ", "REPETERA");

            AddFormat(110, "LEDNING ", "LEDNRAPP", "STABSPLATS*RAPP*", "STABRAPP");
            AddFormat(111, "LEDNING ", "LEDNRAPP", "TROSS/UH-PLATSER", "TROSSRAP");
            AddFormat(112, "LEDNING ", "LEDNRAPP", "MINERINGSRAPPORT", "MINRAPP ");
            AddFormat(113, "LEDNING ", "LEDNRAPP", "STRIVRAPP:10-64 ", "STRVRAPP");
            AddFormat(114, "LEDNING ", "LEDNRAPP", "STRIVRAPP:70-120", "STRVRAPP");

            AddFormat(200, "LV      ", "LV-LEDN ", "ORDER*INFLYGNING", "ORDINFLY");
            AddFormat(201, "LV      ", "LV-LEDN ", "ORDLUFTFARKOST*1", "ORDLUFT1");
            AddFormat(202, "LV      ", "LV-LEDN ", "ORDLUFTFARKOST*2", "ORDLUFT2");
            AddFormat(203, "LV      ", "LV-LEDN ", "ORDLUFTLANDSÄTTN", "ORDER*LL");
            AddFormat(204, "LV      ", "LV-LEDN ", "ORD*UNDERSTÖDJER", "ORD*USTD");
            AddFormat(205, "LV      ", "LV-LEDN ", "ORDER*SKYDDAR*  ", "ORD*SKYD");
            AddFormat(206, "LV      ", "LV-LEDN ", "LUFOR/LV-ORDER* ", "LUFOR*LV");
            AddFormat(207, "LV      ", "LV-LEDN ", "RPK*ORIGO*ALT*  ", "ORIGO");
            AddFormat(208, "LV      ", "LV-LEDN ", "FLYGBASSAMVERKAN", "FLBASSV1");
            AddFormat(209, "LV      ", "LV-LEDN ", "FLYGRÄNNA       ", "FLBASSV2");

            AddFormat(210, "LV      ", "LV-RAPP", "GRUPPERINGSRAPP*", "GRPRAPP ");
            AddFormat(211, "LV      ", "LV-RAPP", "HÖRBARHETLUF/LVO", "HÖRBLUF ");
            AddFormat(212, "LV      ", "LV-RAPP", "AFLYGVÄDERPROGN*", "AF*VÄDEP");

            AddFormat(300, "SJÖMÅL  ", "ETI     ", "ETI*SVAR*       ", "ETI*SVAR");
            AddFormat(310, "SJÖMÅL  ", "ELDORDER", "ELDORDER/ETI*   ", "EO*ETI  ");
            AddFormat(311, "SJÖMÅL  ", "ELDORDER", "ELDORDER*KNS*   ", "EO*KNS  ");
            AddFormat(312, "SJÖMÅL  ", "ELDORDER", "ELDORDER*SJÖMÅL1", "EO*SJM*1");

            AddFormat(320, "SJÖMÅL  ", "ELDSIGN ", "ERO*NORMALMETOD1", "ERO*N1  ");
            AddFormat(321, "SJÖMÅL  ", "ELDSIGN ", "ERO*NORMALMETOD2", "ERO*N2  ");
            AddFormat(322, "SJÖMÅL  ", "ELDSIGN ", "VERKANSRAPPORT* ", "VERKRAPP");
            AddFormat(323, "SJÖMÅL  ", "ELDSIGN ", "ERO*MFU*ESU*    ", "ERO*ESU");
            AddFormat(324, "SJÖMÅL  ", "ELDSIGN ", "PJÄSCHEFSRAPPORT", "PJCHRAPP");
            AddFormat(325, "SJÖMÅL  ", "ELDSIGN ", "ERO*NYTT*ELDK*  ", "ERO*NYTT");
            AddFormat(326, "SJÖMÅL  ", "ELDSIGN ", "ERO*NYA-BERELEM ", "ERO*NYBE");
            AddFormat(327, "SJÖMÅL  ", "ELDSIGN ", "ERO*KNS*        ", "ERO*KNS");
            AddFormat(328, "SJÖMÅL  ", "ELDSIGN ", "ERO*KORREKTIONER", "ERO*KORR");
            AddFormat(329, "SJÖMÅL  ", "ELDSIGN ", "ERO*SJÖMÅL      ", "ERO*SJM ");
            AddFormat(330, "SJÖMÅL  ", "ELDSIGN ", "ME*SJÖMÅL*      ", "ME*SJÖ  ");
            AddFormat(331, "SJÖMÅL  ", "ELDSIGN ", "ME*MARKMÅL*     ", "ME*MARK ");

            AddFormat(340, "SJÖMÅL  ", "REK     ", "REKORDER*       ", "REKORDER");
            AddFormat(340, "SJÖMÅL  ", "REK     ", "REKRAPPORT*     ", "REKRAPP ");

            AddFormat(350, "SJÖMÅL  ", "GRPNG   ", "GRPORDER*MST*   ", "GRP*MST ");
            AddFormat(351, "SJÖMÅL  ", "GRPNG   ", "GRUPPERINGSO*01*", "GRP*01  ");
            AddFormat(352, "SJÖMÅL  ", "GRPNG   ", "GRUPPERINGSO*02*", "GRP*02  ");

            AddFormat(360, "SJÖMÅL  ", "FÄLLRAPP", "FÄLLNINGSRAPPORT", "FÄLLRAPP");

            AddFormat(370, "SJÖMÅL  ", "SPANRAPP", "SPANINGSORDER*KA", "S-ORD*KA");
            AddFormat(371, "SJÖMÅL  ", "SPANRAPP", "IK*TABLÅ*       ", "IK*TABLÅ");

            // TODO: Lägg till resten
        }

        public List<DartFormat> Formats { get { return _formats; } }

        public DartFormat GetFormat(int nr)
        {
            return _formats.FirstOrDefault(fmt => fmt.Nr == nr);
        }

        public DartFormat GetFormat(string numberString)
        {
            int nr;
            if (!int.TryParse(numberString, out nr))
                return null;

            return GetFormat(nr);
        }

        public string GetShortName(string level)
        {
            if (string.IsNullOrEmpty(level))
                return "ROT*NIVÅ";

            foreach (var fmt in _formats)
            {
                var currentLevel = fmt.Nr.ToString();
                if (currentLevel.StartsWith(level))
                {
                    if (level.Length == 1)
                        return fmt.Nivå1;
                    if (level.Length == 2)
                        return fmt.Nivå2;

                    return fmt.Namn8Tecken;
                }
            }

            return "ROT*NIVÅ";
        }

        private void AddFormat(int nr, string nivå1, string nivå2, string namn16tkn, string namn8tkn, params string[] body)
        {
            var fmt = new DartFormat(nr, nivå1, nivå2, namn16tkn, namn8tkn, body);
            _formats.Add(fmt);
        }
    }
}