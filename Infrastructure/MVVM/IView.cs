using System;
using System.Windows.Forms;

namespace GreenSQL.Infrastructure.MVVM
{
    public interface IView<T> where T : ViewModel
    {
        T ViewModel { get; }
    }
}
