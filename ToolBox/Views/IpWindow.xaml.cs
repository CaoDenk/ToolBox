using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using ToolBox.Handle;

namespace ToolBox.Views
{
    /// <summary>
    /// IpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class IpWindow : Window
    {
        public IpWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Result1.Text = IpHandler.GetBitWiseAnd(Ip1.Text, SubMask1.Text);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
         

            
            

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Result2.Text = IpHandler.GetBitWiseAnd(Ip2.Text, SubMask2.Text);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
       
        }
    }
}
