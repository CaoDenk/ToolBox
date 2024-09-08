using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
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
using ToolBox.Handle;

namespace ToolBox.Views
{
    /// <summary>
    /// FileEqual.xaml 的交互逻辑
    /// </summary>
    public partial class FileEqual : Window ,INotifyPropertyChanged
    {
        public FileEqual()
        {
          
            InitializeComponent();
            this.DataContext = this;
        }
        string file1;
        public string File1 { get =>file1;set { file1 = value;OnPropertyChanged();  } }
        string file2;
        public string File2 { get => file2; set { file2 = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            Console.WriteLine(name);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void OpenFile1(object sender, RoutedEventArgs e)
        {
            FileDialog dialog=new OpenFileDialog();
            bool? b=dialog.ShowDialog();
            if(b.HasValue&&b.Value)
            {
                File1 = dialog.FileName;
            }
           
        }

        private void OpenFile2(object sender, RoutedEventArgs e)
        {
            FileDialog dialog = new OpenFileDialog();
            bool? b = dialog.ShowDialog();
            if (b.HasValue && b.Value)
            {
                File2 = dialog.FileName;
            }
        }
        /// <summary>
        /// 通过比较md5，判断文件是否相同
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompareByMd5(object sender, RoutedEventArgs e)
        {
            if(File1== null || File2 == null) return;
            if(File1 == File2)
            {
                MessageBox.Show("同一个文件");
                return;
            }
            FileInfo fileinfo1 = new FileInfo(file1);
            FileInfo fileinfo2 = new FileInfo(file1);

            if (fileinfo1.Length != fileinfo2.Length)
            {
                MessageBox.Show("false","比较结果");
                return;
            }
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(file1);
            byte[] hash = md5.ComputeHash(stream);

            using var stream2 = File.OpenRead(file2);
            byte[] hash2 = md5.ComputeHash(stream2);

            if (Enumerable.SequenceEqual(hash, hash2))
            {
                MessageBox.Show("true");
            }
            else
            {
                MessageBox.Show("false");
            }



        }
        /// <summary>
        /// 通过比较md5，判断文件是否相同
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompareBySha256(object sender, RoutedEventArgs e)
        {
            if (File1 == null || File2 == null) return;
            if (File1 == file2)
            {
                MessageBox.Show("同一个文件");
                return;
            }
            FileInfo fileinfo1 = new FileInfo(file1);
            FileInfo fileinfo2 = new FileInfo(file1);

            if (fileinfo1.Length != fileinfo2.Length)
            {
                MessageBox.Show("false");
                return;
            }
            using var sha256File1 = SHA256.Create();
            using var stream1 = File.OpenRead(file1);
            byte[] hash1 = sha256File1.ComputeHash(stream1);

            using var stream2 = File.OpenRead(file2);
            byte[] hash2 = sha256File1.ComputeHash(stream2);

            //fsha256_1.Text=Encoding.ut.GetString(hash1);
            //fsha256_2.Text=Encoding.

            if (Enumerable.SequenceEqual(hash1, hash2))
            {
                MessageBox.Show("true");
            }
            else
            {
                MessageBox.Show("false");
            }

        }
        private async void CompareByAllBytes(object sender, RoutedEventArgs e)
        {
            Process.Maximum = 1;
            
            await  Task.Run(() =>
            {
                if (file1 == null || file2 == null) return;
                if (file1 == file2)
                {
                    MessageBox.Show("同一个文件");
                    return;
                }
                FileInfo fileinfo1 = new FileInfo(file1);
                FileInfo fileinfo2 = new FileInfo(file1);

                if (fileinfo1.Length != fileinfo2.Length)
                {
                    MessageBox.Show("false");
                    return;
                }
              
                using (var stream1 = File.OpenRead(file1))
                {
                    using var stream2 = File.OpenRead(file2);
                    for (long i = 0; i < stream1.Length; i++)
                    {
                        if (stream1.ReadByte() != stream2.ReadByte())
                        {

                            MessageBox.Show("false");
                            break;
                        }
                        if (i % 128 == 0)
                            Dispatcher.Invoke(() =>
                            {

                                double k = i;
                                Process.Value = k / stream1.Length;

                            });

                    }
                }
                MessageBox.Show("true");
            });
            
        }

        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (e.Data.GetData(DataFormats.FileDrop) is System.Array d )
                {
                    File1 = d.GetValue(0) as string;
                }
            }

        }

        private void StackPanel_Drop_1(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (e.Data.GetData(DataFormats.FileDrop) is System.Array d)
                {
                    File2 = d.GetValue(0) as string;
                }
            }
        }
    }
}
