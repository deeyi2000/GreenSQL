using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace GreenSQL.Infrastructure.MVVM
{
    public abstract class ViewModelBase<T> : DependencyObject, INotifyPropertyChanged where T : DependencyObject
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

        private T _context;
        public T Context => _context;

        public ViewModelBase(T context) => _context = context;

        public async void InvalidateRequerySuggested() => await Context.Dispatcher.BeginInvoke((Action)(() => { CommandManager.InvalidateRequerySuggested(); }));
    }
}
