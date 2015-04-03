using System.Collections.Generic;
using Ra180.Programs;

namespace Ra180
{
    public abstract class Ra180Device
    {
        private readonly Stack<ProgramBase> _programStack = new Stack<ProgramBase>();

        public abstract bool IsOnline { get; }
        public abstract bool IsPoweredOn { get; }

        public void SendKeys(string keys)
        {
            if (keys.Contains(Ra180Key.NOLLST))
            {
                SendKey(Ra180Key.NOLLST);
                return;
            }

            foreach (var character in keys)
            {
                SendKey(character.ToString());
            }
        }

        public void SendKeys(params string[] keys)
        {
            if (string.Join("", keys).Contains(Ra180Key.NOLLST))
            {
                SendKey(Ra180Key.NOLLST);
                return;
            }

            foreach (var key in keys)
                SendKey(key);
        }

        public void SendKey(string key)
        {
            OnKey(key);
        }

        protected abstract void OnKeyBEL();

        protected virtual bool OnKey(string key)
        {
            if (HandleModKey(key))
            {
                RefreshDisplay();
                return true;
            }

            if (!IsPoweredOn)
            {
                RefreshDisplay();
                return true;
            }

            if (key == Ra180Key.NOLLST)
            {
                OnKeyReset();
                return true;
            }

            if (key == Ra180Key.BEL)
            {
                OnKeyBEL();
                return true;
            }

            if (HandleSystemKey(key))
            {
                RefreshDisplay();
                return true;
            }

            if (!IsOnline)
            {
                RefreshDisplay();
                return true;
            }

            return SendKeyToPrograms(key);
        }

        protected abstract void OnKeyReset();

        protected abstract bool HandleModKey(string key);

        protected abstract bool HandleSystemKey(string key);

        protected virtual bool SendKeyToPrograms(string key)
        {
            if (SendKeyToCurrentProgram(key))
                return true;

            return StartNewProgram(key);
        }

        private bool SendKeyToCurrentProgram(string key)
        {
            var program = GetCurrentProgram();
            if (program == null)
                return false;

            program.SendKey(key);
            program.UpdateDisplay();
            return true;
        }

        protected ProgramBase GetCurrentProgram()
        {
            if (_programStack.Count == 0)
                return null;

            return _programStack.Peek();
        }

        protected virtual bool StartNewProgram(string key)
        {
            var program = CreateProgram(key);
            if (program == null)
                return false;

            _programStack.Push(program);
            program.Closed += (sender, eventArgs) => _programStack.Pop();
            program.UpdateDisplay();
            return true;
        }

        protected abstract ProgramBase CreateProgram(string key);


        protected void RefreshDisplay()
        {
            if (!IsOnline)
                return;

            var program = GetCurrentProgram();
            if (program != null)
            {
                program.UpdateDisplay();
                return;
            }

            ClearDisplay();
        }

        protected abstract void ClearDisplay();
    }
}