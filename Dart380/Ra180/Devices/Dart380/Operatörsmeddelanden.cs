using System;
using System.Collections;
using System.Collections.Generic;

namespace Ra180.Devices.Dart380
{
    public class Operatörsmeddelanden : ICollection<Operatörsmeddelande>
    {
        private readonly List<Operatörsmeddelande> _list = new List<Operatörsmeddelande>();

        public event EventHandler ItemAdded;

        public IEnumerator<Operatörsmeddelande> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _list).GetEnumerator();
        }

        public void Add(string text, bool isPersisted = false)
        {
            Add(new Operatörsmeddelande {IsPersisted = isPersisted, Text = text});
        }

        public void Add(Operatörsmeddelande item)
        {
            _list.Add(item);
            OnItemAdded();
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(Operatörsmeddelande item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(Operatörsmeddelande[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(Operatörsmeddelande item)
        {
            return _list.Remove(item);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<Operatörsmeddelande>)_list).IsReadOnly; }
        }

        protected virtual void OnItemAdded()
        {
            var handler = ItemAdded;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}