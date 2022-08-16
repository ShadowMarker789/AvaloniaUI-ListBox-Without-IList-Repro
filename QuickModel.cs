using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1
{
    public class QuickModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public bool CheckChange<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if(!Equals(oldValue, newValue))
            {
                oldValue = newValue;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => CheckChange(ref _name, value);
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set => CheckChange(ref _description, value);
        }
    }
}
