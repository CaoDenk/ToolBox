using Microsoft.ML.OnnxRuntime;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToolBox.Views
{
    /// <summary>
    /// ShowOnnxModelInfo.xaml 的交互逻辑
    /// </summary>
    public partial class ShowOnnxModelInfo : Window
    {
        //public required string OnnxModelPath { set; private get; }
        //public string InfoText { set; get; }
        public ShowOnnxModelInfo()
        {
            InitializeComponent();
        }


        private void SelectOnnxModel(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog openfileDialog = new()
            {
                Title = "选择onnx文件",
                Filter = "onnx文件(*.onnx)|*.onnx",
                RestoreDirectory = true
            };
            bool? show = openfileDialog.ShowDialog();
            if (show==true)
            {
                OnnxInfo.Text= "";
                string onnxfile = openfileDialog.FileName;

                StringBuilder stringBuilder = new StringBuilder();
                InferenceSession session = new InferenceSession(onnxfile);
                var inputdata = session.InputMetadata;
                
                var inputNames = session.InputNames;
                stringBuilder.AppendLine($"name :{inputNames[0]}");

                
                foreach (var input in inputdata) 
                {
                    if(input.Value.IsTensor)
                    {
                        string dim=string.Join(",", input.Value.Dimensions);
                        stringBuilder.AppendLine($"key:{input.Key},\tdim:[{dim}],valueType: {input.Value.ElementType.Name}");
                    }
      
                }
                OnnxInfo.Text = stringBuilder.ToString();

            }

        }
    }
}
