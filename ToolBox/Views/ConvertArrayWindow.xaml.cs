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
using ToolBox.Handle;

namespace ToolBox.Views
{
    /// <summary>
    /// ConvertArrayWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConvertArrayWindow : Window
    {
        public ConvertArrayWindow()
        {
            InitializeComponent();
        }

        private void ConvertCSharp(object sender, RoutedEventArgs e)
        {
            string  s=Input.Text;
            
            ParseMatrix parse=new ParseMatrix();
            var list=parse.Parse(s);
            Result.Text = GetString(list);
            Clipboard.SetText(Result.Text);
            Display.Text = "转换结果已经保存在截切版里";
        }

        string GetString(List<List<int>> list)
        {
            int len=list.Count;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"int[][] {VariableName.Text}=new int[{len}][];");
            for(int i=0; i<len; i++)
            {
                string arr=string.Join(",", list[i]);
                sb.AppendLine($"{VariableName.Text}[{i}]=new int[{list[i].Count}]{{{arr}}};");
            }

            return sb.ToString();
        }
    }
}
