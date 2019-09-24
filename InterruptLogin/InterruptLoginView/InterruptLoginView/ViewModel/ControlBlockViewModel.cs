using InterruptLoginView.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TypressPacket;

namespace InterruptLoginView.ViewModel
{
    class ControlBlockViewModel : INotifyPropertyChanged
    {
        DataPacket dp = new DataPacket();
        private string id;
        private string totalPrintCount;
        private string nowMoney;
        private string useMoney;
        private string remainMoney;

        public string Id { get => id; set { this.id = value; OnPropertyChanged("Id "); } }
        public string TotalPrintCount { get => totalPrintCount; set { this.totalPrintCount = value; OnPropertyChanged("TotalPrintCount "); } }
        public string NowMoney { get => nowMoney; set { this.nowMoney = value; OnPropertyChanged("NowMoney "); } }
        public string UseMoney { get => useMoney; set { this.useMoney = value; OnPropertyChanged("UseMoney "); } }
        public string RemainMoney { get => remainMoney; set { this.remainMoney = value; OnPropertyChanged("RemainMoney "); } }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

       public ICommand Print { get; set; }
        public ICommand Close { get; set; }
        public ICommand ViewMyPage { get; set; }

        public ControlBlockViewModel()
        {
            Print = new Command(ExecutePrint, CanExecute);
            Close = new Command(ExecuteCloseWindow, CanExecute);
            ViewMyPage = new Command(ExecuteViewMyPage, CanExecute);


            dp = ((App)Application.Current).getNowDataPacket();

            id = "Winterlood";
            TotalPrintCount = "20";
            NowMoney = "20000";
            UseMoney = "-";
            UseMoney += "1000";

            dp.Money = Int32.Parse("4040");
            dp.Id = "winterlood";

            //idBox.Text = dp.Id;
            //UseMoneyBox1.Text = "-";
            //UseMoneyBox1.Text += "3202";
            //TotalPrintCountBox.Text = "20";

            //NowMoneyBox.Text = dp.Money.ToString();
            //NowMoneyBox2.Text = NowMoneyBox.Text;

            int nowmoney = Int32.Parse(NowMoney.ToString());
            int usemoney = Int32.Parse(UseMoney.ToString());
            RemainMoney = (nowmoney + usemoney).ToString();
        }

        private void ExecuteViewMyPage(object obj)
        {
            // Typress.exe 틀어야함
            MessageBox.Show("뷰 페이지 온");

        }

        private void ExecuteCloseWindow(object obj)
        {
            // Window 닫기
            // Close 버튼
            MessageBox.Show("인쇄 안함 로그아웃 후 창닫기");

        }

        private void ExecutePrint(object obj)
        {
            // 프린트 하기
            if (ShowSelectBox())
            {
                MessageBox.Show("논 로그아웃");
            }
            else
            {
                MessageBox.Show("로그아웃");
            }
        }

        private Boolean ShowSelectBox()
        {
            if (MessageBox.Show("출력할 인쇄물이 남았습니까? ", "Yes-No", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                return true;
            }
            else return false;
        }

        private bool CanExecute(object arg)
        {
            return true;
        }
    }
}
