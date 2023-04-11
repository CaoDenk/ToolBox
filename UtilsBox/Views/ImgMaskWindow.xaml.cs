using Microsoft.Win32;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;
using Window = System.Windows.Window;
using Microsoft.Win32;
namespace UtilsBox.Views
{
    /// <summary>
    /// ImgMaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ImgMaskWindow : Window
    {
        Mat _Mat { get; set; }
        Mat _Mask { get; set; }
        List<string> masks;
        string OrignImg;
        Mat ShowMat;
        const float alpha = 0.7f;
        const float beta = 0.3f;
        Vec4b v = new Vec4b(0, 255, 255, 100);
        HashSet<string> layerSet = new HashSet<string>();

        List<string> maskedLayer = new List<string>();
        List<string> unmaskedLayer = new List<string>();
        string dir = "E:\\Dataset\\ImageMatching\\dataset\\mesad-real\\mesad-real\\train\\out\\real1_frame_490";
        public ImgMaskWindow()
        {
            InitializeComponent();
        }


        void OpenImg(object sender, RoutedEventArgs e)
        {

            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = @"E:\Dataset\ImageMatching\dataset\mesad-real\mesad-real\train\images";
            var result = dialog.ShowDialog();
            if (result.HasValue && (bool)result)
            {
                OrignImg = dialog.FileName;
                var covt = new ImageSourceConverter();
                ImageSource t = (ImageSource)covt.ConvertFromString(OrignImg);
                Img.Source = t;
                Img.Height = t.Height;
                Img.Width = t.Width;
                dir = $@"E:\\Dataset\\ImageMatching\\dataset\\mesad-real\\mesad-real\\train\\out\\{Path.GetFileNameWithoutExtension(OrignImg)}";
                ListMask();

                ShowMat = Cv2.ImRead(OrignImg, ImreadModes.Unchanged);
                Cv2.CvtColor(ShowMat, ShowMat, ColorConversionCodes.BGR2BGRA);
            }



        }

        void ListMask()
        {



            var files = Directory.GetFiles(dir);

            CheckBoxStack.Children.Clear();
            masks = new List<string>();

            foreach (var file in files)
            {
                if (file.EndsWith(".png"))
                {

                    string filename = Path.GetFileNameWithoutExtension(file);
                    masks.Add(filename);
                }

            }
            masks = masks.OrderBy(f => int.Parse(f)).ToList();
            foreach (var mask in masks)
            {
                CheckBox c = new CheckBox();
                c.Content = mask;

                c.Checked += AddMaskOp;
                c.Unchecked += RemoveMaskOp;
                CheckBoxStack.Children.Add(c);

            }



        }
        private void ChooseDirectory(object sender, RoutedEventArgs e)
        {


            //var dialog = new CommonOpenFileDialog();
            //dialog.IsFolderPicker = true;
            //CommonFileDialogResult result = dialog.ShowDialog();
            //if (result == CommonFileDialogResult.Ok)
            //{
            //    dir = dialog.FileName;
            //}
            ListMask();
        }








        void AddMaskOp(object sender, RoutedEventArgs e)
        {


            CheckBox c = (CheckBox)sender;
            Mat layer = OpenMask((string)c.Content);
            AddMask(layer);
            ShowMask(ShowMat);
        }

        void RemoveMaskOp(object sender, RoutedEventArgs e)
        {


            CheckBox c = (CheckBox)sender;
            Mat layer = OpenMask((string)c.Content);
            RemoveMask(layer);
            ShowMask(ShowMat);
        }





        Mat OpenMask(string fileName)
        {

            string file = $@"{dir}\{fileName}.png";

            return Cv2.ImRead(file, ImreadModes.Unchanged);
        }
        void ShowMask(Mat mat)
        {
            int width = mat.Width;
            int height = mat.Height;
            int stride = width * (mat.Type() == MatType.CV_8UC4 ? 4 : 3);
            IntPtr ptr = mat.Data;
            BitmapSource bitmapSource = BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null, ptr, height * stride, stride);
            Mask.Source = bitmapSource;
            Mask.Height = bitmapSource.Height;
            Mask.Width = bitmapSource.Width;
        }

        private void Mask_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)(sender);
            System.Windows.Point point = e.GetPosition(img);
            int x = (int)point.X;
            int y = (int)point.Y;
            FindMask(y, x);

            ShowMask(ShowMat);

        }

        void FindMask(int x, int y)
        {

            foreach (var str in masks)
            {

                Mat mat = OpenMask(str);

                if (mat.Get<byte>(x, y) != 0)
                {
                    AddMask(mat);
                }



            }


        }
        void AddMask(Mat layer)
        {


            for (int i = 0; i < layer.Rows; ++i)
            {
                for (int j = 0; j < layer.Cols; ++j)
                {
                    if (layer.Get<byte>(i, j) != 0)
                    {
                        //mask.Set<Vec4b>(i, j, new Vec4b(0, 255, 255, 100));
                        Vec4b pixel = ShowMat.Get<Vec4b>(i, j);

                        pixel[0] = (byte)(pixel[0] * alpha + v[0] * beta);
                        pixel[1] = (byte)(pixel[1] * alpha + v[1] * beta);
                        pixel[2] = (byte)(pixel[2] * alpha + v[2] * beta);
                        pixel[3] = (byte)(pixel[3] * alpha + v[3] * beta);
                        ShowMat.Set<Vec4b>(i, j, pixel);

                    }
                }
            }


        }
        void RemoveMask(Mat layer)
        {
            for (int i = 0; i < layer.Rows; ++i)
            {
                for (int j = 0; j < layer.Cols; ++j)
                {
                    if (layer.Get<byte>(i, j) != 0)
                    {
                        Vec4b pixel = ShowMat.Get<Vec4b>(i, j);

                        pixel[0] = (byte)((pixel[0] - v[0] * beta) / alpha);
                        pixel[1] = (byte)((pixel[1] - v[1] * beta) / alpha);
                        pixel[2] = (byte)((pixel[2] - v[2] * beta) / alpha);
                        pixel[3] = (byte)((pixel[3] - v[3] * beta) / alpha);
                        ShowMat.Set<Vec4b>(i, j, pixel);

                    }
                }
            }
        }

    }
}
