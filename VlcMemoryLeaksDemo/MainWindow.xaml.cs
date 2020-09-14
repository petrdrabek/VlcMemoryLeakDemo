using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VlcMemoryLeaksDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VlcViewer _vlcViewer;

        public MainWindow()
        {
            InitializeComponent();
            var timer = new Timer(2000);
            timer.AutoReset = true;
            timer.Elapsed += PlayVlcVideo;
            timer.Start();
        }


        private void PlayVlcVideo(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (_vlcViewer != null)
                {
                    _vlcViewer.Stop();
                    _vlcViewer.Dispose();
                }

                _vlcViewer = new VlcViewer();
                ParentGrid.Children.Add(_vlcViewer.Display);
                _vlcViewer.Play();
            });
        }
    }
}
