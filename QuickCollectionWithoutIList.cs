using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1
{
    internal class QuickCollectionWithoutIList : IReadOnlyList<QuickModel>, INotifyCollectionChanged
    {
        private Nito.AsyncEx.AsyncReaderWriterLock _lock = new Nito.AsyncEx.AsyncReaderWriterLock();

        private InternalCollection _items = new();

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public int Count => _items.Count;
        public QuickModel this[int index] => _items[index];

        public IEnumerator<QuickModel> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        
        public void AddItem(QuickModel item)
        {
            using var lockyLock = _lock.WriterLock();
            _items.Add(item);
            int index = _items.IndexOf(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
                 NotifyCollectionChangedAction.Add,
                 item,
                 index
                ));
        }

        public bool RemoveItem(QuickModel item)
        {
            using var lockyLock = _lock.WriterLock();
            int index = _items.IndexOf(item);
            var retval = _items.Remove(item);
            if (retval)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove, item, index));
            return retval;
        }
    }
}
