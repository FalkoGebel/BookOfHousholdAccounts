using BohaWpf.Utils;

namespace BohaWpf.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        private string _name = string.Empty;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == value)
                    return;

                _name = value;
                NotifyOfPropertyChange(nameof(Name));
            }
        }
    }
}