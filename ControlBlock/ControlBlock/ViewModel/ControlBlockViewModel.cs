using ControlBlock.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TypressPacket;

namespace ControlBlock.ViewModel
{
    class ControlBlockViewModel : INotifyPropertyChanged
    {
        DataPacket dp = new DataPacket();
        private string id;
        private int totalPrintCount;
        private int nowMoney;
        private int useMoney;
        private int remainMoney;

        public string Id { get => id; set { this.id = value; OnPropertyChanged("Id "); } }
        public int TotalPrintCount { get => totalPrintCount; set { this.totalPrintCount = value; OnPropertyChanged("TotalPrintCount "); } }
        public int NowMoney { get => nowMoney; set { this.nowMoney = value; OnPropertyChanged("NowMoney "); } }
        public int UseMoney { get => useMoney; set { this.useMoney = value; OnPropertyChanged("UseMoney "); } }
        public int RemainMoney { get => remainMoney; set { this.remainMoney = value; OnPropertyChanged("RemainMoney "); } }


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

            id = dp.Name;
            TotalPrintCount = dp.TotalUsage;
            NowMoney = dp.Money;
            //UseMoney = "-";
            UseMoney += 1000;

            int nowmoney = NowMoney;
            int usemoney = UseMoney;
            RemainMoney = (nowmoney - usemoney);
        }

        private void ExecuteViewMyPage(object obj)
        {
            // Typress.exe 틀어야함
            ViewHandler.OpenMainViewFromWindow();
            System.Environment.Exit(1);
        }

        private void ExecuteCloseWindow(object obj)
        {
            // Window 닫기
            // Close 버튼
            App.SendPacketToServer(new DataPacket());
            MessageBox.Show("인쇄 안함 로그아웃 후 창닫기");
            System.Environment.Exit(1);
        }

        private void ExecutePrint(object obj)
        {
            // 프린트 하기
            if (ShowSelectBox())
            {
                MessageBox.Show("출력을 진행합니다.");
                //DB


                //server로부터 몇장인지 받아온다.
                UpdateDB();

                // close exit status code : 0
                ViewHandler.OpenControlViewFromPrint();
                System.Environment.Exit(0);

            }
            else
            {
                App.SendPacketToServer(new DataPacket());
                MessageBox.Show("로그아웃");

                // close exit status code : 1
                ViewHandler.OpenLoginViewFromMain();
                System.Environment.Exit(1);
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
        private void UpdateDB()
        {
            dp.Money -= 1000;
            dp.TotalUsage += 1;
            App.SendPacketToServer(dp);
        }
    }
}
