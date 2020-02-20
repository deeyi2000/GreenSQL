using GreenSQL.Domain.Model;
using GreenSQL.Infrastructure.Helper;
using GreenSQL.Infrastructure.MVVM;

namespace GreenSQL.ViewModel
{
    public class DelViewModel : ViewModelBase<DelWindow>
    {
        #region ModelProperty
        protected Instance _instance;
        public Instance Instance
        {
            get => _instance;
            set
            {
                _instance = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ViewProperty
        protected bool _isDeleteFiles;
        public bool IsDeleteFiles
        {
            get => _isDeleteFiles;
            set
            {
                _isDeleteFiles = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Command
        protected Command _okCommand;
        public Command OkCommand
        {
            get
            {
                if (null == _okCommand)
                {
                    _okCommand = new Command(
                        p =>
                        {
                            if(Instance.Uninstall() && IsDeleteFiles) {
                                FileHelper.Delete($"{Instance.Path}\\{Instance.InstanceId}");
                            }
                            Context.DialogResult = true;
                            Context.Close();
                        });
                }
                return _okCommand;
            }
        }

        protected Command _cancelCommand;
        public Command CancelCommand
        {
            get
            {
                if (null == _cancelCommand)
                {
                    _cancelCommand = new Command(
                        p =>
                        {
                            Context.DialogResult = false;
                            Context.Close();
                        });
                }
                return _cancelCommand;
            }
        }
        #endregion

        public DelViewModel(DelWindow context) : base(context) { }
    }
}
