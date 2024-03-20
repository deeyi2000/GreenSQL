using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GreenSQL.Infrastructure.MVVM
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (null == PropertyChanged) return;

            string[] propertyNames = propertyName.Split(new char[] { ',' });
            foreach (string name in propertyNames)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name.Trim()));
            }
        }
    }
}
