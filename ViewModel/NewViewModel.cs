using GreenSQL.Domain.Model;
using GreenSQL.Infrastructure.MVVM;

namespace GreenSQL.ViewModel
{
    public class NewViewModel : Infrastructure.MVVM.ViewModel
    {
        #region ModelProperty
        protected Image[] _images;
        public Image[] Images
        {
            get => _images;
            set
            {
                _images = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ViewProperty
        protected Image _selectedImage;
        public Image SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged();
            }
        }

        protected string _instanceName;
        public string InstanceName
        {
            get => _instanceName;
            set
            {
                _instanceName = value;
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
                            SelectedImage.Install(InstanceName);
                        },
                        p => (null != SelectedImage) && !string.IsNullOrWhiteSpace(InstanceName));
                }
                return _okCommand;
            }
        }
        #endregion

        public NewViewModel() 
        {
            Images = Image.LoadAll();
            SelectedImage = Images?.GetValue(0) as Image;
        }

    }
}
