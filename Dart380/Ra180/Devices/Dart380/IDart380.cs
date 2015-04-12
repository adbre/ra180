using Ra180.Programs;

namespace Ra180
{
	public interface IDart380
	{
		Ra180Display LargeDisplay { get; }
		Ra180Display SmallDisplay { get; }
		void SendKey (string key);
	}

    public class FakeDart380 : Dart380Base
    {
        public override bool IsOnline
        {
            get { throw new System.NotImplementedException(); }
        }

        public override bool IsPoweredOn
        {
            get { throw new System.NotImplementedException(); }
        }

        protected override bool OnKey(string key)
        {
            LargeDisplay.SetText(string.Format("Key={0}", key), 3, trailingUnderscore: true);
            return true;
        }

        protected override void OnKeyBEL()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnKeyReset()
        {
            throw new System.NotImplementedException();
        }

        protected override bool HandleModKey(string key)
        {
            throw new System.NotImplementedException();
        }

        protected override bool HandleSystemKey(string key)
        {
            throw new System.NotImplementedException();
        }

        protected override ProgramBase CreateProgram(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}
