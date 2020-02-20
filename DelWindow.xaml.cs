using GreenSQL.ViewModel;
using System.Windows;

namespace GreenSQL
{
    /// <summary>
    /// Interaction logic for DelWindow.xaml
    /// </summary>
    public partial class DelWindow : Window
    {
        public DelWindow()
        {
            this.DataContext = new DelViewModel(this);
            InitializeComponent();
        }
    }
} 
