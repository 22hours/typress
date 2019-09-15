using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MemberMainView.VM;

namespace MemberMainView.View
{
    /// <summary>
    /// EditUserControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MyPageUserControl : UserControl
    {
        public MyPageUserControl()
        {
            this.DataContext = new MyPageViewModel();
            InitializeComponent();
        }

        private void ListBox_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
