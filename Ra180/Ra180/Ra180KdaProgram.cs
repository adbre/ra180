using System;
using System.Linq;

namespace Ra180
{
    internal class Ra180KdaProgram : Ra180MenuProgram
    {
        private Ra180EditMenuItem _bd1;
        private Ra180EditMenuItem _bd2;
        private Ra180EditMenuItem _pny;

        private int _pnyIndex;
        private string[] _pnyInput;

        public Ra180KdaProgram(Ra180 ra180)
            : base(ra180)
        {
            Title = "KDA";

            AddChild(new Ra180EditMenuItem
            {
                Prefix = () =>
                {
                    if (Ra180.Data.CurrentChannelData.IsKLARDisabled)
                        return "**";

                    return "FR";
                },
                MaxInputTextLength = () => 5,
                CanEdit = () => true,
                AcceptInput = (text, key) =>
                {
                    if (key == "*" && (text == "" || text == "*"))
                        return true;

                    return key.All(Char.IsDigit);
                },
                SaveInput = text =>
                {
                    if (string.IsNullOrEmpty(text)) return false;
                    
                    if (text == "**")
                    {
                        var value = Ra180.Data.CurrentChannelData.IsKLARDisabled;
                        Ra180.Data.CurrentChannelData.IsKLARDisabled = !value;
                        return true;
                    }

                    if (text.Length != 5) return false;
                    if (!text.All(Char.IsDigit)) return false;
                    var fr = int.Parse(text);

                    if ((fr%25) != 0) return false;

                    Ra180.Data.CurrentChannelData.FR = fr;
                    return true;
                },
                GetValue = () =>
                {
                    if (Ra180.Data.CurrentChannelData.IsKLARDisabled && Ra180.Mod == Ra180Mod.KLAR)
                        return "00000";

                    return string.Format("{0:00000}", Ra180.Data.CurrentChannelData.FR);
                }
            });

            if (Ra180.Mod == Ra180Mod.KLAR)
                return;

            _bd1 = new Ra180EditMenuItem
            {
                Prefix = () => "BD1",
                CanEdit = () => true,
                AcceptInput = (text, key) => key.All(Char.IsDigit),
                SaveInput = text =>
                {
                    if (string.IsNullOrEmpty(text)) return false;
                    if (text.Length != 4) return false;
                    if (!text.All(Char.IsDigit)) return false;

                    var from = short.Parse(text.Substring(0, 2));
                    var end = short.Parse(text.Substring(2, 2));

                    Ra180.Data.CurrentChannelData.BD1.Start = @from;
                    Ra180.Data.CurrentChannelData.BD1.End = end;

                    if (from == 90)
                        return true;

                    CurrentChild = _bd2;
                    _bd1.IsEditing = false;
                    _bd1.EditValue = null;
                    _bd2.IsEditing = true;
                    _bd2.EditValue = null;
                    return false;
                },
                GetValue = () => string.Format("{0:00}{1:00}", Ra180.Data.CurrentChannelData.BD1.Start, Ra180.Data.CurrentChannelData.BD1.End)
            };

            _bd2 = new Ra180EditMenuItem
            {
                Prefix = () => "BD2",
                CanEdit = () => true,
                AcceptInput = (text, key) => key.All(Char.IsDigit),
                IsDisabled = () =>
                {
                    return Ra180.Data.CurrentChannelData.BD1.Start == 90;
                },
                SaveInput = text =>
                {
                    if (string.IsNullOrEmpty(text)) return false;
                    if (text.Length != 4) return false;
                    if (!text.All(Char.IsDigit)) return false;

                    var from = short.Parse(text.Substring(0, 2));
                    var end = short.Parse(text.Substring(2, 2));

                    Ra180.Data.CurrentChannelData.BD2.Start = @from;
                    Ra180.Data.CurrentChannelData.BD2.End = end;
                    CurrentChild = _bd1;
                    return true;
                },
                GetValue = () => string.Format("{0:00}{1:00}", Ra180.Data.CurrentChannelData.BD2.Start, Ra180.Data.CurrentChannelData.BD2.End),
                OnKey = key =>
                {
                    if (key == Ra180Key.ÄND && !_bd2.IsEditing)
                    {
                        _bd1.IsEditing = true;
                        _bd1.EditValue = null;
                        CurrentChild = _bd1;
                        return true;
                    }

                    return false;
                }
            };

            AddChild(_bd1);
            AddChild(_bd2);

            AddChild(new Ra180EditMenuItem
            {
                Prefix = () => "SYNK",
                CanEdit = () => Ra180.Data.CurrentChannelData.Synk,
                GetValue = () => Ra180.Data.CurrentChannelData.Synk ? "JA" : "NEJ",
                OnKey = key =>
                {
                    if (key == Ra180Key.ÄND)
                    {
                        Ra180.Data.CurrentChannelData.Synk = !Ra180.Data.CurrentChannelData.Synk;
                        return true;
                    }

                    return false;
                }
            });

            _pny = new Ra180EditMenuItem
            {
                Prefix = () =>
                {
                    if (_pny.IsEditing)
                        return string.Format("PN{0}", _pnyIndex + 1);

                    return "PNY";
                },
                CanEdit = () => true,
                GetValue = () =>
                {
                    var pny = Ra180.Data.CurrentChannelData.PNY;
                    if (pny != null)
                        return pny.Checksum;

                    return "###";
                },
                OnKey = key =>
                {
                    if (!_pny.IsEditing && key == Ra180Key.ÄND)
                    {
                        _pnyIndex = 0;
                        _pnyInput = new string[8];
                    }

                    return false;
                },
                SaveInput = text =>
                {
                    if (string.IsNullOrEmpty(text)) return false;
                    if (text.Length != 4) return false;
                    if (!text.All(Char.IsDigit)) return false;

                    _pnyInput[_pnyIndex++] = text;
                    if (_pnyIndex < _pnyInput.Length)
                        return false;

                    Ra180.Data.CurrentChannelData.PNY = new Ra180DataKey(_pnyInput);
                    return true;
                }
            };

            AddChild(_pny);
        }

        public override bool SendKey(string key)
        {
            if (key == Ra180Key.ENT)
            {
                
            }

            return base.SendKey(key);
        }
    }
}