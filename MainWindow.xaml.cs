using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string title;

        string[] images;

        bool isPlaying = false;
        bool isLoading = false;

        private int _index = 0;
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;

                if (_index < 0) _index = images.Length - 1;
                if (_index >= images.Length) _index = 0;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    LoadImage();
                }));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        void Start()
        {
            title = window.Title;

            // ask folder
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                // get folder path
                var folder = Path.GetDirectoryName(openFileDialog.FileName);

                Console.WriteLine(folder);

                // get all images
                images = Directory.GetFiles(folder).Where(file => Regex.IsMatch(file, @"^.+\.(png|jpg)$", RegexOptions.IgnoreCase)).ToArray();
                //for (int i = 0; i < images.Length; i++)
                {
                    //Console.WriteLine(images[i]);
                }

                LoadImage();

                // start timer
                Timer aTimer = new Timer();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                aTimer.Interval = 200;
                aTimer.Enabled = true;
            }
        }



        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.A: // prev image
                case System.Windows.Input.Key.Left: // prev image
                    Index--;
                    break;
                case System.Windows.Input.Key.D: // prev image
                case System.Windows.Input.Key.Right: // next image
                    Index++;
                    break;
                case System.Windows.Input.Key.Space:
                    isPlaying = !isPlaying;
                    Console.WriteLine("isplaying:" + isPlaying);
                    break;
                default:
                    break;
            }
        }

        void LoadImage()
        {
            isLoading = true;

            // load image
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(images[Index]);
            bitmap.EndInit();
            imgViewer.Source = bitmap;

            // show info
            var resolution = +(int)bitmap.Width + " x " + (int)bitmap.Height;
            var fileName = images[Index];
            var frames = Index + "/" + images.Length;
            window.Title = title + "  -  " + resolution + "  -  " + frames + "  -  " + fileName;

            isLoading = false;

        }

        private void MenuBrowse_Click(object sender, RoutedEventArgs e)
        {
            var filePathOnly = Path.GetDirectoryName(images[Index]);
            string argument = "/select, \"" + images[Index] + "\"";
            Process.Start("explorer.exe", argument);

        }

        void Explore(string folder)
        {
            if (Directory.Exists(folder) == true)
            {
                Process.Start(folder);
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (isPlaying == false) return;
            if (isLoading == true) return;

            Index++;
        }

    }
}
