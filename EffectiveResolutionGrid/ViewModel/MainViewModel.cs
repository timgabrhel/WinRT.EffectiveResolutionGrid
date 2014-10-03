using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EffectiveResolutionGrid.Model;

namespace EffectiveResolutionGrid.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Item> _items;

        public ObservableCollection<Item> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        public MainViewModel()
        {
            Items = new ObservableCollection<Item>();

            var random = new Random();
            for (int x = 1; x <= 30; x++)
            {
                Items.Add(new Item()
                {
                    Name = string.Format("Item {0}", x),
                    Color = String.Format("#FF{0:X6}", random.Next(0x1000000))
                });
            }
        }

        public void Measure(double size)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                item.Size = size;
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
