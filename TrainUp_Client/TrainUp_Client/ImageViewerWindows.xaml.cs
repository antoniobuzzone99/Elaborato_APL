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

namespace TrainUp_Client
{
    /// <summary>
    /// Logica di interazione per ImageViewerWindows.xaml
    /// </summary>
    public partial class ImageViewerWindows : Window
    {
        public ImageViewerWindows(byte[] imageData)
        {
            InitializeComponent();

            // Carica l'immagine dai dati dell'immagine
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(imageData);
            bitmap.EndInit();
            imageControl.Source = bitmap;
        }

    }
}
