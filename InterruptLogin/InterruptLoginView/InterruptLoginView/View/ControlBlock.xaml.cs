using InterruptLoginView.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using TypressPacket;

namespace InterruptLoginView.View
{
    /// <summary>
    /// ControlBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ControlBlock : Window
    {
        public ControlBlock() 
        {
            InitializeComponent();
            this.MouseLeftButtonDown += MoveWindow;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.DataContext = new ControlBlockViewModel();
        }
        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
        void MoveWindow(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }

        void ClickLogin(object sender, RoutedEventArgs e)
        {
       
        }
        void ClickQuit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public static void TypressServerConnect()
        {
         
        }
    }
}
