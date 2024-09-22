using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
using WpfApp1;


namespace TrainUp_Client
{
    /// <summary>
    /// Logica di interazione per statistiche.xaml
    /// </summary>
    public partial class Statistiche : Page
    {
        private readonly string token;
        public Statistiche(string token)
        {
            InitializeComponent();
            this.token = token;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                    // Naviga verso una nuova pagina
                    mainWindow.MainFrame.NavigationService.Navigate(new Page1(token));
                
            }
        }

        private async void OpenImageViewerWindowButton_Click(object sender, RoutedEventArgs e)
        {
            //stampa finestra con grafico schede piu usate
            try
            {

                // Fai la richiesta al server per ottenere l'immagine
                string imageUrl = "http://localhost:5000/cards_most_used"; 

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(imageUrl);

                    // Controlla se la richiesta ha avuto successo
                    if (response.IsSuccessStatusCode)
                    {
                        // Ottieni l'immagine come array di byte
                        byte[] imageData = await response.Content.ReadAsByteArrayAsync();

                        // Visualizza l'immagine nella finestra ImageViewerWindow
                        ImageViewerWindows imageViewerWindow = new ImageViewerWindows(imageData);
                        imageViewerWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Errore nel caricamento dell'immagine.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Si è verificato un errore: " + ex.Message);
            }
        }

        private async void OpenImageViewerWindowButton_Click2(object sender, RoutedEventArgs e)
        {
            //Numero di esercizi per Card
            try
            {
                // Fai la richiesta al server per ottenere l'immagine
                string imageUrl1 = "http://localhost:5000/numberExe";
                var data = new { token};

                using (HttpClient client = new HttpClient())
                {
                    
                    HttpResponseMessage response = await client.GetAsync(imageUrl1);

                    // Controlla se la richiesta ha avuto successo
                    if (response.IsSuccessStatusCode)
                    {
                        // Ottieni l'immagine come array di byte
                        byte[] imageData1 = await response.Content.ReadAsByteArrayAsync();

                        // Visualizza l'immagine nella finestra ImageViewerWindow
                        ImageViewerWindows imageViewerWindow1 = new ImageViewerWindows(imageData1);
                        imageViewerWindow1.Show();
                    }
                    else
                    {
                        MessageBox.Show("Errore nel caricamento dell'immagine.");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Si è verificato un errore: " + ex.Message);
            }
        }

        private async void OpenImageViewerWindowButton_Click3(object sender, RoutedEventArgs e)
        {
            //media età utenti registrati
            try
            {
                // Fai la richiesta al server per ottenere l'immagine
                string imageUrl1 = "http://localhost:5000/ave_age";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(imageUrl1);

                    // Controlla se la richiesta ha avuto successo
                    if (response.IsSuccessStatusCode)
                    {
                        // Ottieni l'immagine come array di byte
                        byte[] imageData1 = await response.Content.ReadAsByteArrayAsync();

                        // Visualizza l'immagine nella finestra ImageViewerWindow
                        ImageViewerWindows imageViewerWindow1 = new ImageViewerWindows(imageData1);
                        imageViewerWindow1.Show();
                    }
                    else
                    {
                        MessageBox.Show("Errore nel caricamento dell'immagine.");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Si è verificato un errore: " + ex.Message);
            }
        }

        private async void OpenImageViewerWindowButton_Click4(object sender, RoutedEventArgs e)
        {
            //avanzamneto peso
            try
            {
                // Fai la richiesta al server per ottenere l'immagine
                string imageUrl1 = "http://localhost:5000/avanz_peso";

                using (HttpClient client = new HttpClient())
                {
                    
                    var data = new { token };

                    // Converti i dati in formato JSON
                    string jsonData = JsonSerializer.Serialize(data);

                    // Crea un oggetto StringContent con il JSON
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Esegui la richiesta HTTP POST
                    HttpResponseMessage response = await client.PostAsync(imageUrl1, content);

                    // Controlla se la richiesta ha avuto successo
                    if (response.IsSuccessStatusCode)
                    {
                        // Ottieni l'immagine come array di byte
                        byte[] imageData1 = await response.Content.ReadAsByteArrayAsync();

                        // Visualizza l'immagine nella finestra ImageViewerWindow
                        ImageViewerWindows imageViewerWindow1 = new ImageViewerWindows(imageData1);
                        imageViewerWindow1.Show();
                    }
                    else
                    {
                        MessageBox.Show("Errore nel caricamento dell'immagine.");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Si è verificato un errore: " + ex.Message);
            }

        }


    }
}
