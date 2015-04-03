namespace Ra180.Programs
{
    public abstract class Ra180Program : ProgramBase
    {
        public Ra180 Ra180
        {
            get { return (Ra180) Device; }
            set { Device = value; }
        }
    }
}