using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterruptLoginView.Commands;
using System.Threading.Tasks;
using System.Windows.Input;
using TypressPacket;
using System.Windows;

namespace InterruptLoginView.ViewModel
{
    class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
        }

        public void ExecuteLogin(string id, string pw)
        {
            // 파라미터 id와 pw는 받아왔습니다.
            MessageBox.Show("입력된 id : " + id + "\n입력된 pw : " + pw);
        }
    }
}
