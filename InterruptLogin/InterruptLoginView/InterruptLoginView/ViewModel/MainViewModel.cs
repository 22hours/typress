using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InterruptLoginView.Commands;

namespace InterruptLoginView.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {

        public ICommand Login { get; set; }
        public ICommand Close { get; set; }
        public MainViewModel()
        {
            Login = new Command();

        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
