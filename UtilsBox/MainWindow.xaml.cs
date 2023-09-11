﻿using System;
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
        ImgMaskWindow imgMaskWindow;
        ConvertArrayWindow convertArrayWindow;
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

        private void ShowImgMaskWindow(object sender, RoutedEventArgs e)
        {
            if (imgMaskWindow == null)
            {
                imgMaskWindow = new ImgMaskWindow();


                imgMaskWindow.Closing += (_s, _e) =>
                {
                    imgMaskWindow = null;
                };

                imgMaskWindow.Show();


            }
            else
            {
                imgMaskWindow.Activate();
            }
        }

        private void ConvertArray(object sender, RoutedEventArgs e)
        {
            if (convertArrayWindow == null)
            {
                convertArrayWindow = new ConvertArrayWindow();


                convertArrayWindow.Closing += (_s, _e) =>
                {
                    convertArrayWindow = null;
                };

                convertArrayWindow.Show();
            }
            else
            {
                convertArrayWindow.Activate();
            }
        }
    }
}
