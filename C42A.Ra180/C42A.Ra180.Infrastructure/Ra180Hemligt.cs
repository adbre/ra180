namespace C42A.Ra180.Infrastructure
{
    internal class Ra180Hemligt
    {
        public Ra180Hemligt()
        {
            Kanal1 = new Ra180Kanaldata();
            Kanal2 = new Ra180Kanaldata();
            Kanal3 = new Ra180Kanaldata();
            Kanal4 = new Ra180Kanaldata();
            Kanal5 = new Ra180Kanaldata();
            Kanal6 = new Ra180Kanaldata();
            Kanal7 = new Ra180Kanaldata();
            Kanal8 = new Ra180Kanaldata();
        }

        public Ra180Kanaldata Kanal1 { get; set; }
        public Ra180Kanaldata Kanal2 { get; set; }
        public Ra180Kanaldata Kanal3 { get; set; }
        public Ra180Kanaldata Kanal4 { get; set; }
        public Ra180Kanaldata Kanal5 { get; set; }
        public Ra180Kanaldata Kanal6 { get; set; }
        public Ra180Kanaldata Kanal7 { get; set; }
        public Ra180Kanaldata Kanal8 { get; set; }

        public static Ra180Hemligt GetDefault()
        {
            var result = new Ra180Hemligt();
            result.Kanal1.Frekvens = "30060";
            result.Kanal1.Bandbredd1 = "1234";
            result.Kanal1.Bandbredd2 = "5678";

            return result;
        }
    }
}