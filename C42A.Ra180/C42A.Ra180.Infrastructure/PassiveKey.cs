namespace C42A.Ra180.Infrastructure
{
    public class PassiveKey
    {
        // ReSharper disable InconsistentNaming
        public string PN1 { get; set; }
        public string PN2 { get; set; }
        public string PN3 { get; set; }
        public string PN4 { get; set; }
        public string PN5 { get; set; }
        public string PN6 { get; set; }
        public string PN7 { get; set; }
        public string PN8 { get; set; }
        public string PN9 { get; set; }
        public string NYK { get; set; }
        // ReSharper restore InconsistentNaming

        public string[] ToArray()
        {
            return new[] { PN1, PN2, PN3, PN4, PN5, PN6, PN7, PN8, PN9 };
        }
    }
}