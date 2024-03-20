using System.Windows.Forms;
using GreenSQL.Domain.Model;
using GreenSQL.Infrastructure.Helper;
using GreenSQL.Infrastructure.MVVM;

namespace GreenSQL.ViewModel
{
    public class MainViewModel : Infrastructure.MVVM.ViewModel
    {
        #region ModelProperty
        protected Instance[] _instances;
        public Instance[] Instances
        {
            get => _instances;
            set
            {
                _instances = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ViewProperty
        protected Instance _selectedInstance;
        public Instance SelectedInstance
        {
            get => _selectedInstance;
            set 
            {
                _selectedInstance = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Command
        protected Command _newCommand;
        public Command NewCommand
        {
            get
            {
                if (null == _newCommand)
                {
                    _newCommand = new Command(
                        p => {
                            if (new NewWindow().ShowDialog() == DialogResult.OK)
                                Instances = Instance.LoadAll();
                        });
                }
                return _newCommand;
            }
        }

        protected Command _delCommand;
        public Command DelCommand
        {
            get 
            {
                if (null == _delCommand)
                {
                    _delCommand = new Command(
                        p => {
                            var result = MessageBox.Show($"确定删除实例{SelectedInstance.InstanceName}?", "删除", MessageBoxButtons.OKCancel);
                            if (result == DialogResult.OK) {
                                SelectedInstance.Uninstall();
                                //if (SelectedInstance.Uninstall()) {
                                //    FileHelper.Delete($"{SelectedInstance.Path}\\{SelectedInstance.InstanceId}");
                                //}
                                Instances = Instance.LoadAll();
                            }
                        },
                        p => (null != SelectedInstance));
                }
                return _delCommand;
            }
        }

        protected Command _startCommand;
        public Command StartCommand
        {
            get
            {
                if (null == _startCommand)
                {
                    _startCommand = new Command(
                        p => SelectedInstance.Start(),
                        p => (null != SelectedInstance));
                }
                return _startCommand;
            }
        }

        protected Command _stopCommnad;
        public Command StopCommand
        {
            get
            {
                if (null == _stopCommnad)
                {
                    _stopCommnad = new Command(
                        p => SelectedInstance.Stop(),
                        p => (null != SelectedInstance));
                }
                return _stopCommnad;
            }
        }

        protected Command _queryCommnad;
        public Command QueryCommand
        {
            get
            {
                if (null == _queryCommnad)
                {
                    _queryCommnad = new Command(
                        p => Infrastructure.Helper.FileHelper.Execute(@"MiniSqlQuery\MiniSqlQuery.exe"));
                }
                return _queryCommnad;
            }
        }
        #endregion

        public MainViewModel() {
            _instances = Instance.LoadAll();
        }

    }
}
