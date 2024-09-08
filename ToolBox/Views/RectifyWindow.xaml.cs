using Microsoft.ML.OnnxRuntime;
using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
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
    /// RectifyWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RectifyWindow : System.Windows.Window
    {
        public RectifyWindow()
        {
            InitializeComponent();
        }
        Mat img;
        private void OpenImg(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfileDialog = new OpenFileDialog
            {
                Title = "选择图片",
                Filter = "图像文件 (*.jpg;*.jpeg;*.png;*.bmp;*.webp)|*.jpg;*.jpeg;*.png;*.bmp;*.webp",
                RestoreDirectory = true
            };

            bool? show = openfileDialog.ShowDialog();
            if (show.HasValue && show.Value)
            {

                string imgpath = openfileDialog.FileName;
                RectifyImg rectify = new RectifyImg();
                img= rectify.Rectify(imgpath);
                Img.Source= BitmapSourceConverter.ToBitmapSource(img);
                Img.Height= img.Height;
                Img.Width= img.Width;
            }

       
        }

        private void SaveImg(object sender, RoutedEventArgs e)
        {
            if (img != null)
            {

                SaveFileDialog saveFileDialog = new()
                {
                    Title = "保存图片",
                    Filter = "图像文件 (*.jpg;*.jpeg;*.png;*.bmp;*.webp)|*.jpg;*.jpeg;*.png;*.bmp;*.webp",
                    RestoreDirectory = true
                };
                var show=saveFileDialog.ShowDialog();
                if(show==true)
                {
                    if(Cv2.ImWrite(saveFileDialog.FileName,img))
                    {
                        MessageBox.Show("success");
                    }else
                    {
                        MessageBox.Show("fail");
                    }
                }
            
            }else
            {
                MessageBox.Show("图片为空");
            }
        }

        private void Zoomin(object sender, MouseWheelEventArgs e)
        {
            if (Img != null)
            {
                // 检查滚轮滚动方向
                if (e.Delta > 0)
                {
                    // 向上滚动，放大
                    Zoom(1.1); // 放大1.1倍
                }
                else if (e.Delta < 0)
                {
                    // 向下滚动，缩小
                    Zoom(0.9); // 缩小到0.9倍
                }
            }
        }

        private void Zoom(double factor)
        {
            // 应用缩放因子
            ScaleTransform scaleTransform = Img.RenderTransform as ScaleTransform;

            if (scaleTransform == null)
            {
                scaleTransform = new ScaleTransform();
                Img.RenderTransform = scaleTransform;
            }

            scaleTransform.ScaleX *= factor;
            scaleTransform.ScaleY *= factor;
        }

    }
}
