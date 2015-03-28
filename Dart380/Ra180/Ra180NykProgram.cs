namespace Ra180
{
    internal class Ra180NykProgram : Ra180MenuProgram
    {
        public Ra180NykProgram(Ra180 ra180, Ra180Display display) : base(ra180, display)
        {
            Title = "NYK";

            AddChild(new Ra180EditMenuItem
            {
                Prefix = () => "NYK",
                CanEdit = () =>
                {
                    var any = Ra180.Data.CurrentChannelData.NYK;
                    var pny = Ra180.Data.CurrentChannelData.PNY;

                    return any != null || pny != null;
                },
                OnKey = key =>
                {
                    if (key == Ra180Key.ÄND)
                    {
                        var currentChannelData = Ra180.Data.CurrentChannelData;
                        var any = currentChannelData.NYK;
                        var pny = currentChannelData.PNY;
                        currentChannelData.NYK = pny;
                        currentChannelData.PNY = any;
                        return true;
                    }

                    return false;
                },
                GetValue = () =>
                {
                    var any = Ra180.Data.CurrentChannelData.NYK;
                    if (any == null)
                        return "###";

                    return any.Checksum;
                }
            });
        }
    }
}