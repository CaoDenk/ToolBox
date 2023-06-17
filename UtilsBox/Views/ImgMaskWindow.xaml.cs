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
using UtilsBox.ColorLayer;
using Path = System.IO.Path;
using Window = System.Windows.Window;
namespace UtilsBox.Views
{
    /// <summary>
    /// ImgMaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ImgMaskWindow : Window
    {
        Mat _Mat { get; set; }
        Mat _Mask { get; set; }
    
        string OrignImg;
        Mat ShowMat;
        const float alpha = 0.7f;
        const float beta = 0.3f;

        HashSet<string> layerSet = new HashSet<string>();
        //List<(string,Vec4b)> maskedLayer = new List<(string, Vec4b)>();
        Dictionary<string,Vec4b> maskedLayer = new Dictionary<string, Vec4b>();
        List<string> unmaskedLayer = new List<string>();

        string initialDirectory = @"E:\Dataset\Img_seg\make_dataset\mesad-real\mesad-real\train\images";
        string dir = "E:\\Dataset\\ImageMatching\\dataset\\mesad-real\\mesad-real\\train\\out\\real1_frame_490";
        string outdir = @"E:\\Dataset\\Img_seg\\make_dataset\\mesad-real\\mesad-real\\train\\out";
        public ImgMaskWindow()
        {
            InitializeComponent();
        }


        void OpenImg(object sender, RoutedEventArgs e)
        {

            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = initialDirectory;
            var result = dialog.ShowDialog();
            if (result.HasValue && (bool)result)
            {
                OrignImg = dialog.FileName;
                var covt = new ImageSourceConverter();
                ImageSource t = (ImageSource)covt.ConvertFromString(OrignImg);
                Img.Source = t;
                Img.Height = t.Height;
                Img.Width = t.Width;
                dir = $@"{outdir}\{Path.GetFileNameWithoutExtension(OrignImg)}";
                ListMask();

                ShowMat = Cv2.ImRead(OrignImg, ImreadModes.Unchanged);
                Cv2.CvtColor(ShowMat, ShowMat, ColorConversionCodes.BGR2BGRA);
                ShowMask(ShowMat);
            }


        }

        void ListMask()
        {

            var files = Directory.GetFiles(dir);
            unmaskedLayer = new List<string>();
            maskedLayer.Clear();
            foreach (var file in files)
            {
                if (file.EndsWith(".png"))
                {
                    unmaskedLayer.Add(Path.GetFileName(file));
                }

            }

        }


        void AddMaskOp(object sender, RoutedEventArgs e)
        {


            CheckBox c = (CheckBox)sender;
            string layerName = (string)c.Content;
            Mat layer = OpenMask(layerName);
            AddMask(layerName,layer,BRGAColorMask.GetRandMask());
            ShowMask(ShowMat);
        }

        void RemoveMaskOp(object sender, RoutedEventArgs e)
        {


            CheckBox c = (CheckBox)sender;
            string layerName = (string)c.Content;
            Mat layer = OpenMask(layerName);
            RemoveMask(layerName, layer);
            ShowMask(ShowMat);
        }





        Mat OpenMask(string fileName)
        {

            string file = $@"{dir}\{fileName}";
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
            FindInUnmask(y, x);
            ShowMask(ShowMat);

        }
        private void Mask_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)(sender);
            System.Windows.Point point = e.GetPosition(img);
            int x = (int)point.X;
            int y = (int)point.Y;
            FindInMask(y, x);
            ShowMask(ShowMat);

        }
        void FindInUnmask(int x, int y)
        {
 
            foreach (var str in unmaskedLayer)
            {

                Mat mat = OpenMask(str);
                if (mat.Get<byte>(x, y) != 0)
                {
                   AddMask(str,mat,BRGAColorMask.GetRandMask());      
                   break;
                }
            }
        }

        void FindInMask(int x, int y)
        {

            foreach (var str in maskedLayer.Keys)
            {

                Mat mat = OpenMask(str);
                if (mat.Get<byte>(x, y) != 0)
                {
                    RemoveMask(str, mat );

                    break;
                }
            }
        }


        void AddMask(string layerName,Mat layer,Vec4b v)
        {


            for (int i = 0; i < layer.Rows; ++i)
            {
                for (int j = 0; j < layer.Cols; ++j)
                {
                    if (layer.Get<byte>(i, j) != 0)
                    {
                        Vec4b pixel = ShowMat.Get<Vec4b>(i, j);

                        pixel[0] = (byte)(pixel[0] * alpha + v[0] * beta);
                        pixel[1] = (byte)(pixel[1] * alpha + v[1] * beta);
                        pixel[2] = (byte)(pixel[2] * alpha + v[2] * beta);
                        pixel[3] = (byte)(pixel[3] * alpha + v[3] * beta);
                        ShowMat.Set<Vec4b>(i, j, pixel);

                    }
                }
            }
            unmaskedLayer.Remove(layerName);
            maskedLayer.Add(layerName, v);

        }
        void RemoveMask(string layerName,Mat layer)
        {


            Vec4b v= maskedLayer[layerName];
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
            maskedLayer.Remove(layerName);
            unmaskedLayer.Add(layerName);
        }
        private void SaveImg(object sender, RoutedEventArgs e)
        {
            Mat mat = null;
            foreach (string s in maskedLayer.Keys)
            {
                var m=OpenMask(s);
                if(mat==null)
                {
                    mat = m;
                }else
                {
                    Cv2.BitwiseOr(mat, m,mat);
                }

            }
            string fileName = Path.GetFileName(OrignImg);
            mat.SaveImage($@"E:\Dataset\Img_seg\make_dataset\mask\{fileName}");


            
        }

    }
}
