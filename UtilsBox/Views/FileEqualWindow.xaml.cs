using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
    /// FileEqual.xaml 的交互逻辑
    /// </summary>
    public partial class FileEqual : Window
    {
        public FileEqual()
        {
            InitializeComponent();
        }
        string file1;
        string file2;
        private void OpenFile1(object sender, RoutedEventArgs e)
        {
            FileDialog dialog=new OpenFileDialog();
            bool? b=dialog.ShowDialog();
            if(b.HasValue&&b.Value)
            {
                file1 = dialog.FileName;
                f1.Text = file1;
            }
           
        }

        private void OpenFile2(object sender, RoutedEventArgs e)
        {
            FileDialog dialog = new OpenFileDialog();
            bool? b = dialog.ShowDialog();
            if (b.HasValue && b.Value)
            {
                file2 = dialog.FileName;
                f2.Text = file2;
            }
        }
        /// <summary>
        /// 通过比较md5，判断文件是否相同
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompareByMd5(object sender, RoutedEventArgs e)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(file1))
                {
                    byte[] hash = md5.ComputeHash(stream);

                    using (var stream2 = File.OpenRead(file2))
                    {
                        byte[] hash2 = md5.ComputeHash(stream2);

                        if(Enumerable.SequenceEqual(hash, hash2))
                        {
                            MessageBox.Show("true");
                        }else
                        {
                            MessageBox.Show("false");
                        }

                    }

                }
            }



        }
        /// <summary>
        /// 通过比较md5，判断文件是否相同
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompareBySha256(object sender, RoutedEventArgs e)
        {
            using (var sha256File1 = SHA256.Create())
            {
                using (var stream1 = File.OpenRead(file1))
                {
                    byte[] hash1 = sha256File1.ComputeHash(stream1);

                    using (var stream2= File.OpenRead(file2))
                    {
                        byte[] hash2 = sha256File1.ComputeHash(stream2);

                        if (Enumerable.SequenceEqual(hash1, hash2))
                        {
                            MessageBox.Show("true");
                        }
                        else
                        {
                            MessageBox.Show("false");
                        }

                    }

                }
            }

        }
        private void CompareByAllBytes(object sender, RoutedEventArgs e)
        {

        }
    }
}
