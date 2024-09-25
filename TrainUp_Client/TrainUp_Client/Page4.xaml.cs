using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Logica di interazione per Page4.xaml
    /// </summary>
    /// aggiorna dati utente
    public partial class Page4 : Page
    {
        private readonly string token;
        public Page4(string token)
        {
            InitializeComponent();
            this.token = token;
            Combobox.Items.Add("age");
            Combobox.Items.Add("weight");
            
        }

        //private readonly HttpRequestService httpRequestService = new HttpRequestService();
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                // Naviga verso una nuova pagina
                mainWindow.MainFrame.NavigationService.Navigate(new Page1(token));
                
            }
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            string data = TestBox.Text;
            string data1 = Combobox.SelectedItem.ToString();

            // il ? indica che il tipo è un nullable type. Questo significa che il tipo può avere un valore normale o essere null. 
            DateTime? selectedDate = dataPicker.SelectedDate;
            // Verifica se è stata selezionata una data
            string dateString = selectedDate.HasValue ? selectedDate.Value.ToString("yyyy-MM-dd") : null;


            using (HttpClient client = new HttpClient())
            {
                string url = "http://localhost:5002/update/data";
                var data2 = new { data, data1, dateString, token };

                // Converti i dati in formato JSON
                string jsonData = JsonSerializer.Serialize(data2);

                // Crea un oggetto StringContent con il JSON
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Esegui la richiesta HTTP POST
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Leggi la risposta come stringa
                string responseString = await response.Content.ReadAsStringAsync();

                // Controlla la risposta JSON per il successo
                var responseObject = JsonSerializer.Deserialize<Dictionary<string, int>>(responseString);
                int state = (int)responseObject["state"];

                if (state == 1)
                {
                    outputLabel2.Visibility = Visibility.Hidden;
                    outputLabel.Visibility = Visibility.Visible;
                    outputLabel3.Visibility = Visibility.Hidden;
                }
                else if (state == 0) {
                    
                    outputLabel2.Visibility = Visibility.Visible;
                    outputLabel.Visibility = Visibility.Hidden;
                    outputLabel3.Visibility = Visibility.Hidden;
                }
                else if ( state == 2)
                {
                    outputLabel3.Visibility = Visibility.Visible;
                    outputLabel.Visibility = Visibility.Hidden;
                    outputLabel2.Visibility = Visibility.Hidden;
                }
            }
        }


        private async void UpdatePassword_Click(object sender, RoutedEventArgs e) {
            string nuovaPassword = NuovaPasswordTestBox.Password;
            string confermaPassword = ConfermaPasswordTestBox.Password;
            string vecchiaPassword = vecchiaPasswordTestBox.Password;   

            using (HttpClient client = new HttpClient())
            {
                string url = "http://localhost:5002/update/password";
                var data = new { nuovaPassword, confermaPassword, vecchiaPassword, token };

                // Converti i dati in formato JSON
                string jsonData = JsonSerializer.Serialize(data);

                // Crea un oggetto StringContent con il JSON
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Esegui la richiesta HTTP POST
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Leggi la risposta come stringa
                string responseString = await response.Content.ReadAsStringAsync();

                // Controlla la risposta JSON per il successo
                var responseObject = JsonSerializer.Deserialize<Dictionary<string, int>>(responseString);
                int state = (int)responseObject["state"];

                if (state == 1)
                {
                    pass_aggiornataLabel.Visibility = Visibility.Hidden;
                    pass_campierratiLabel.Visibility = Visibility.Visible;
                }
                else if (state == 0)
                {
                    pass_aggiornataLabel.Visibility = Visibility.Visible;
                    pass_campierratiLabel.Visibility = Visibility.Hidden;
                }
            }
        }
        
        private async void DeleteCard_Click(object sender, RoutedEventArgs e){
            string id_scheda = id_schedaTestBox.Text;   
            string password_ = passTextBox.Password; 

            using (HttpClient client = new HttpClient())
            {
                string url = "http://localhost:5000/delete_trainingCard";
                var data = new { id_scheda, password_, token };

                // Converti i dati in formato JSON
                string jsonData = JsonSerializer.Serialize(data);

                // Crea un oggetto StringContent con il JSON
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Esegui la richiesta HTTP POST
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Leggi la risposta come stringa
                string responseString = await response.Content.ReadAsStringAsync();

                // Controlla la risposta JSON per il successo
                var responseObject = JsonSerializer.Deserialize<Dictionary<string, int>>(responseString);
                int state = (int)responseObject["state"];

                if (state == 1)
                {
                    schedaEliminata.Visibility = Visibility.Hidden;
                    campiErratiScheda.Visibility = Visibility.Visible;
                }
                else if (state == 0)
                {
                    schedaEliminata.Visibility = Visibility.Visible;
                    campiErratiScheda.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
