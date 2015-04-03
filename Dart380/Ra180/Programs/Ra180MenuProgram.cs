namespace Ra180.Programs
{
    public abstract class Ra180MenuProgram : MenuProgram<Ra180>
    {
        protected Ra180MenuProgram(Ra180 device, Ra180Display display) : base(device, display)
        {
        }

        public Ra180 Ra180
        {
            get { return Device; }
            set { Device = value; }
        }
    }
}