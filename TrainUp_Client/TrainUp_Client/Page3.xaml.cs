using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.PortableExecutable;
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
using TrainUp_Client;

namespace WpfApp1
{
    /// <summary>
    /// Logica di interazione per Page3.xaml
    /// </summary>
    /// register
    public partial class Page3 : Page //partial in quando è il metodo è condiviso tra la page xaml e il .cs
    {
        public Page3()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Accedi al NavigationService del Frame dalla finestra principale
            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                // Naviga verso una nuova pagina
                mainWindow.MainFrame.NavigationService.Navigate(new Page0());
                
            }
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            string age = AgeBox.Text;
            string weight = WeightBox.Text;
            

            using (HttpClient client = new HttpClient())
            {
                string url = $"http://localhost:5002/register";
                var data = new { username, password, confirmPassword, age, weight};

                // Converti i dati in formato JSON
                string jsonData = JsonSerializer.Serialize(data);

                // Crea un oggetto StringContent con il JSON
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Esegui la richiesta HTTP POST
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Leggi la risposta come stringa
                string responseString = await response.Content.ReadAsStringAsync();

                // Controlla la risposta JSON per il successo
                var responseObject = JsonSerializer.Deserialize<Dictionary<string, string>>(responseString);
                string state = responseObject["state"];
                

                string success = "success";
                string fault1 = "fault1";
                string fault2 = "fault2";
                string fault3 = "fault3";
                string fault4 = "fault4";
                if (state == success)
                {
                    string token = responseObject["token"];
                    // Accedi al NavigationService del Frame dalla finestra principale
                    if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
                    {

                        // Naviga verso una nuova pagina
                        mainWindow.MainFrame.NavigationService.Navigate(new Page1(token));

                    }
                }
                else if (state == fault2)
                {
                    //password diverse
                    outputLabel.Visibility = Visibility.Visible;
                    outputLabel2.Visibility = Visibility.Hidden;
                    outputLabel3.Visibility = Visibility.Hidden;
                    outputLabel4.Visibility = Visibility.Hidden;
                }
                else if (state == fault1)
                {
                    //campi vuoti
                    outputLabel2.Visibility = Visibility.Visible;
                    outputLabel.Visibility = Visibility.Hidden;
                    outputLabel3.Visibility = Visibility.Hidden;
                    outputLabel4.Visibility = Visibility.Hidden;
                }
                else if (state == fault3) {
                    //età non valida 
                    outputLabel2.Visibility = Visibility.Hidden;
                    outputLabel.Visibility = Visibility.Hidden;
                    outputLabel3.Visibility = Visibility.Visible;
                    outputLabel4.Visibility = Visibility.Hidden;
                }
                else if(state == fault4)
                {
                    //peso non valido
                    outputLabel4.Visibility = Visibility.Visible;
                    outputLabel.Visibility = Visibility.Hidden;
                    outputLabel2.Visibility = Visibility.Hidden;
                    outputLabel3.Visibility = Visibility.Hidden;
                }
            }

        }
    }
}
