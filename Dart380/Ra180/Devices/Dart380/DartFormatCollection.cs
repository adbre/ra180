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
            AddFormat(103, "LEDNING ", "LEDNSBTJ", "F�RBINDELSEPROV*", "F�RBPROV");
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
            AddFormat(203, "LV      ", "LV-LEDN ", "ORDLUFTLANDS�TTN", "ORDER*LL");
            AddFormat(204, "LV      ", "LV-LEDN ", "ORD*UNDERST�DJER", "ORD*USTD");
            AddFormat(205, "LV      ", "LV-LEDN ", "ORDER*SKYDDAR*  ", "ORD*SKYD");
            AddFormat(206, "LV      ", "LV-LEDN ", "LUFOR/LV-ORDER* ", "LUFOR*LV");
            AddFormat(207, "LV      ", "LV-LEDN ", "RPK*ORIGO*ALT*  ", "ORIGO");
            AddFormat(208, "LV      ", "LV-LEDN ", "FLYGBASSAMVERKAN", "FLBASSV1");
            AddFormat(209, "LV      ", "LV-LEDN ", "FLYGR�NNA       ", "FLBASSV2");

            AddFormat(210, "LV      ", "LV-RAPP", "GRUPPERINGSRAPP*", "GRPRAPP ");
            AddFormat(211, "LV      ", "LV-RAPP", "H�RBARHETLUF/LVO", "H�RBLUF ");
            AddFormat(212, "LV      ", "LV-RAPP", "AFLYGV�DERPROGN*", "AF*V�DEP");

            AddFormat(300, "SJ�M�L  ", "ETI     ", "ETI*SVAR*       ", "ETI*SVAR");
            AddFormat(310, "SJ�M�L  ", "ELDORDER", "ELDORDER/ETI*   ", "EO*ETI  ");
            AddFormat(311, "SJ�M�L  ", "ELDORDER", "ELDORDER*KNS*   ", "EO*KNS  ");
            AddFormat(312, "SJ�M�L  ", "ELDORDER", "ELDORDER*SJ�M�L1", "EO*SJM*1");

            AddFormat(320, "SJ�M�L  ", "ELDSIGN ", "ERO*NORMALMETOD1", "ERO*N1  ");
            AddFormat(321, "SJ�M�L  ", "ELDSIGN ", "ERO*NORMALMETOD2", "ERO*N2  ");
            AddFormat(322, "SJ�M�L  ", "ELDSIGN ", "VERKANSRAPPORT* ", "VERKRAPP");
            AddFormat(323, "SJ�M�L  ", "ELDSIGN ", "ERO*MFU*ESU*    ", "ERO*ESU");
            AddFormat(324, "SJ�M�L  ", "ELDSIGN ", "PJ�SCHEFSRAPPORT", "PJCHRAPP");
            AddFormat(325, "SJ�M�L  ", "ELDSIGN ", "ERO*NYTT*ELDK*  ", "ERO*NYTT");
            AddFormat(326, "SJ�M�L  ", "ELDSIGN ", "ERO*NYA-BERELEM ", "ERO*NYBE");
            AddFormat(327, "SJ�M�L  ", "ELDSIGN ", "ERO*KNS*        ", "ERO*KNS");
            AddFormat(328, "SJ�M�L  ", "ELDSIGN ", "ERO*KORREKTIONER", "ERO*KORR");
            AddFormat(329, "SJ�M�L  ", "ELDSIGN ", "ERO*SJ�M�L      ", "ERO*SJM ");
            AddFormat(330, "SJ�M�L  ", "ELDSIGN ", "ME*SJ�M�L*      ", "ME*SJ�  ");
            AddFormat(331, "SJ�M�L  ", "ELDSIGN ", "ME*MARKM�L*     ", "ME*MARK ");

            AddFormat(340, "SJ�M�L  ", "REK     ", "REKORDER*       ", "REKORDER");
            AddFormat(340, "SJ�M�L  ", "REK     ", "REKRAPPORT*     ", "REKRAPP ");

            AddFormat(350, "SJ�M�L  ", "GRPNG   ", "GRPORDER*MST*   ", "GRP*MST ");
            AddFormat(351, "SJ�M�L  ", "GRPNG   ", "GRUPPERINGSO*01*", "GRP*01  ");
            AddFormat(352, "SJ�M�L  ", "GRPNG   ", "GRUPPERINGSO*02*", "GRP*02  ");

            AddFormat(360, "SJ�M�L  ", "F�LLRAPP", "F�LLNINGSRAPPORT", "F�LLRAPP");

            AddFormat(370, "SJ�M�L  ", "SPANRAPP", "SPANINGSORDER*KA", "S-ORD*KA");
            AddFormat(371, "SJ�M�L  ", "SPANRAPP", "IK*TABL�*       ", "IK*TABL�");

            // TODO: L�gg till resten
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
                return "ROT*NIV�";

            foreach (var fmt in _formats)
            {
                var currentLevel = fmt.Nr.ToString();
                if (currentLevel.StartsWith(level))
                {
                    if (level.Length == 1)
                        return fmt.Niv�1;
                    if (level.Length == 2)
                        return fmt.Niv�2;

                    return fmt.Namn8Tecken;
                }
            }

            return "ROT*NIV�";
        }

        private void AddFormat(int nr, string niv�1, string niv�2, string namn16tkn, string namn8tkn, params string[] body)
        {
            var fmt = new DartFormat(nr, niv�1, niv�2, namn16tkn, namn8tkn, body);
            _formats.Add(fmt);
        }
    }
}