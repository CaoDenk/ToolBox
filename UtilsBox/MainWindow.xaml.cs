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
using UtilsBox.Views;

namespace UtilsBox
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        ConvertWindow convertWindow;
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void ShowConvertWindow(object sender, RoutedEventArgs e)
        {
            if(convertWindow == null)
            {
                convertWindow = new ConvertWindow();


                convertWindow.Closing += (_s, _e) =>
                {
                    convertWindow = null;
                };
              
               convertWindow.Show();


            }else
            {
               convertWindow.Activate();
            }
     
            
        }

   
    }
}
