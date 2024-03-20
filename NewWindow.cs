using System;
using System.Windows.Forms;
using GreenSQL.ViewModel;
using GreenSQL.Infrastructure.MVVM;

namespace GreenSQL
{
    public partial class NewWindow : Form, IView<NewViewModel>
    {
        private NewViewModel _viewModel = new NewViewModel();
        public NewViewModel ViewModel => _viewModel;

        public NewWindow()
        {
            InitializeComponent();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            cbImages.DisplayMember = "FullName";
            cbImages.DataBindings.Add("DataSource", ViewModel, "Images");
            cbImages.DataBindings.Add("SelectedItem", ViewModel, "SelectedImage");
            tbName.DataBindings.Add("Text", ViewModel, "InstanceName");
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            btnOk.Enabled = ViewModel.OkCommand.CanExecute();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ViewModel.OkCommand.Execute(this);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}