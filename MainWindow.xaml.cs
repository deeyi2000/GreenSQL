using GreenSQL.ViewModel;
using System.Windows;

namespace GreenSQL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = new MainViewModel(this);
            InitializeComponent();
        }

        public void LoadImages()
        {

        }
    }
}
