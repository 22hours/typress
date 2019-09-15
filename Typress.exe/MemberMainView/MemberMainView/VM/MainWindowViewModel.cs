using MemberMainView.M;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MemberMainView.View;
using System.Windows;

namespace MemberMainView.VM
{
    class MainWindowViewModel: INotifyPropertyChanged
    {
        private MyPageViewModel _mypage;
        private UsageViewModel _usage;
        private ChargeViewModel _charge;
        private LicenseViewModel _licesnse;
        private EditViewModel _edit;
        private LoginViewModel _login;

        public ICommand myPageOn { get; set; }
        public ICommand editPageOn { get; set; }
        public ICommand usagePageOn { get; set; }
        public ICommand chargePageOn { get; set; }
        public ICommand licensePageOn { get; set; }
        public ICommand LoginPageOn { get; set; }



        public MainWindowViewModel()
        {
            this._mypage = new MyPageViewModel();
            this._usage = new UsageViewModel();
            this._charge = new ChargeViewModel();
            this._licesnse = new LicenseViewModel();
            this._edit = new EditViewModel();
            this._login = new LoginViewModel();

            LoginPageOn = new Command(LoadLoginPage, CE);
            myPageOn = new Command(LoadMyPage, CE);
            usagePageOn = new Command(LoadUsage, CE);
            chargePageOn = new Command(LoadCharge, CE);
            licensePageOn = new Command(LoadLicense,CE);
            editPageOn = new Command(LoadEditPage,CE);
            ContentView = null;
        }
        private bool CE(object obj)
        {
            return true;
        }
        private object _contentView;

        private void LoadLoginPage(object obj)
        {
            this.ContentView = this._login;
        }
        private void LoadMyPage(object obj)
        {
            this.ContentView = this._mypage;
        }
        private void LoadEditPage(object obj)
        {
            this.ContentView = this._edit;
        }
        private void LoadCharge(object obj)
        {
            this.ContentView = this._charge;
        }
        private void LoadUsage(object obj)
        {
            this.ContentView = this._usage;
        }
        private void LoadLicense(object obj)
        {
            this.ContentView = this._licesnse;
        }
        public object ContentView
        {
            get { return this._contentView; }
            set
            {
                this._contentView = value;
                this.OnPropertyChanged("ContentView");
            }
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}

