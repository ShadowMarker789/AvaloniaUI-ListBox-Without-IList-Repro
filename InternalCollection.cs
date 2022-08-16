using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace AvaloniaApplication1
{

    internal class InternalCollection : KeyedCollection<string, QuickModel>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override string GetKeyForItem(QuickModel item)
        => item.Name;
    }
    
}
