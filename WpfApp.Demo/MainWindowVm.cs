using System.Collections.Generic;
using System.ComponentModel;

namespace AppDemo
{
    class MainWindowVm : INotifyPropertyChanged
    {
        public MainWindowVm()
        {

        }

        private readonly List<string> _stringValues = new List<string> { "Value 1", "Value 2", "Value 3" };
        public IEnumerable<string> StringValues
        {
            get { return _stringValues; }
        }


        private string _selectedStringValue;
        public string SelectedStringValue
        {
            get
            {
                return _selectedStringValue;
            }
            set
            {
                _selectedStringValue = value;
                OnPropertyChanged(nameof(SelectedStringValue));

                Text = _selectedStringValue; // change the text
            }
        }


        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
