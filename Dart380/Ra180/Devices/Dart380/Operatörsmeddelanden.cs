using System;
using System.Collections;
using System.Collections.Generic;

namespace Ra180.Devices.Dart380
{
    public class Operat�rsmeddelanden : ICollection<Operat�rsmeddelande>
    {
        private readonly List<Operat�rsmeddelande> _list = new List<Operat�rsmeddelande>();

        public event EventHandler ItemAdded;

        public IEnumerator<Operat�rsmeddelande> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _list).GetEnumerator();
        }

        public void Add(string text, bool isPersisted = false)
        {
            Add(new Operat�rsmeddelande {IsPersisted = isPersisted, Text = text});
        }

        public void Add(Operat�rsmeddelande item)
        {
            _list.Add(item);
            OnItemAdded();
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(Operat�rsmeddelande item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(Operat�rsmeddelande[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(Operat�rsmeddelande item)
        {
            return _list.Remove(item);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<Operat�rsmeddelande>)_list).IsReadOnly; }
        }

        protected virtual void OnItemAdded()
        {
            var handler = ItemAdded;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}