using GreenSQL.ViewModel;
using System.Windows;

namespace GreenSQL
{
    /// <summary>
    /// Interaction logic for NewWindow.xaml
    /// </summary>
    public partial class NewWindow : Window
    {
        public NewWindow()
        {
            this.DataContext = new NewViewModel(this);
            InitializeComponent();
        }

        public void LoadImages()
        {

        }
    }
}
