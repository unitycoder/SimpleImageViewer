using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] images;
        int index = 0;

        public MainWindow()
        {
            InitializeComponent();
            Start();
        }


        void Start()
        {
            // ask folder
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                // get folder path
                var folder = Path.GetDirectoryName(openFileDialog.FileName);

                Console.WriteLine(folder);

                // get all images
                images = Directory.GetFiles(folder).Where(file => Regex.IsMatch(file, @"^.+\.(png|jpg)$", RegexOptions.IgnoreCase)).ToArray();
                for (int i = 0; i < images.Length; i++)
                {
                    Console.WriteLine(images[i]);
                }
            }


            // show
        }



        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Console.WriteLine("keydown");
            switch (e.Key)
            {
                case System.Windows.Input.Key.Left:
                    index = --index;
                    if (index < 0) index = images.Length - 1;
                    break;
                case System.Windows.Input.Key.Right:
                    index = ++index % images.Length;

                    break;
                default:
                    break;
            }

            LoadImage();

        }

        void LoadImage()
        {
            // assign image
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(images[index]);
            bitmap.EndInit();
            imgViewer.Source = bitmap;
        }
    }
}
