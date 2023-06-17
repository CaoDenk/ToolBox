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
using System.Windows.Shapes;

namespace UtilsBox.Views
{
    /// <summary>
    /// ConvertWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConvertWindow : Window
    {
        public ConvertWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string v = Input.Text;
            //MessageBox.Show(v);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in v)
            {
                if (c == '\\')
                {
                    stringBuilder.Append('/');
                }
                else
                    stringBuilder.Append(c);
            }
            Result.Text = stringBuilder.ToString();
            Clipboard.SetText(Result.Text);
            Display.Text = "转换结果已经保存在截切版里";
        }
        private void Clear(object sender, RoutedEventArgs e)
        {
            Input.Clear();
            Result.Clear();
            Display.Text = "";
        }

        private void LowerToUpper(object sender, RoutedEventArgs e)
        {
            string v = Input.Text;
            //MessageBox.Show(v);
     
            Result.Text = v.ToUpper();         
            Clipboard.SetText(Result.Text);
            Display.Text = "转换结果已经保存在截切版里";
        }

        //protected override void OnClosed(EventArgs e)
        //{

        //    base.OnClosed(e);

        //}

    }
}
