using System;
using System.Windows.Forms;
using GreenSQL.ViewModel;
using GreenSQL.Infrastructure.MVVM;

namespace GreenSQL
{
    public partial class MainWindow : Form, IView<MainViewModel>
    {
        private MainViewModel _viewModel = new MainViewModel();
        public MainViewModel ViewModel => _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            dgServers.AutoGenerateColumns = false;
            dgServers.DataBindings.Add("DataSource", ViewModel, "Instances");
            btnNew.Click += (sender, e) => ViewModel.NewCommand.Execute();
            btnDelete.Click += (sender, e) => ViewModel.DelCommand.Execute();
            btnStart.Click += (sender, e) => ViewModel.StartCommand.Execute();
            btnStop.Click += (sender, e) => ViewModel.StopCommand.Execute();
            btnQuery.Click += (sender, e) => ViewModel.QueryCommand.Execute();
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            btnDelete.Enabled = ViewModel.DelCommand.CanExecute();
            btnStart.Enabled = ViewModel.StartCommand.CanExecute();
            btnStop.Enabled = ViewModel.StopCommand.CanExecute();
        }

        private void dgServers_SelectionChanged(object sender, EventArgs e)
        {
             if(dgServers.SelectedRows.Count == 0) {
                 ViewModel.SelectedInstance = null;
             } else {
                 ViewModel.SelectedInstance = (Domain.Model.Instance)dgServers.SelectedRows[0].DataBoundItem;
             }
        }
    }
}