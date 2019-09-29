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
