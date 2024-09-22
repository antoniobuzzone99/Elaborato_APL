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

using TrainUp_Client;


using System.Diagnostics;
using System.Text.Json.Serialization;
using System.ComponentModel;


namespace WpfApp1
{
    /// <summary>
    /// Logica di interazione per Page1.xaml
    /// </summary>
    /// HOME
    public partial class Page1 : Page
    {
        private readonly string token;

        public Page1(string token)
        {
            InitializeComponent();
            this.token = token;
            LoadDataFromServer();
            LoadCardFromDb();
        }
        

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"http://localhost:5002/logout";
                var data = new{token};

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
                string state = (string)responseObject["state"];
                

                if (state == "success")
                {
                    // Accedi al NavigationService del Frame dalla finestra principale
                    if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
                    {
                        
                        // Naviga verso una nuova pagina
                        mainWindow.MainFrame.NavigationService.Navigate(new Page0());
                        
                    }
                }
            }

        }

        private async void New_card_ClickAsync(object sender, RoutedEventArgs e) {

            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                // Naviga verso una nuova pagina
                mainWindow.MainFrame.NavigationService.Navigate(new creaScheda(token));
                
            }
                
            
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            //FUNZIONE AGGIORNA DATI

            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                // Naviga verso una nuova pagina
                mainWindow.MainFrame.NavigationService.Navigate(new Page4(token));
                
            }
        }

        private void Exercise_ButtonClick(object sender, RoutedEventArgs e)
        {
            //stampa gli esercizi della scheda

            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                // Naviga verso una nuova pagina
                mainWindow.MainFrame.NavigationService.Navigate(new Page5(token));
                
            }
        }

        private void Statistiche_ButtonClick(object sender, RoutedEventArgs e)
        {
            //stampa gli esercizi della scheda

            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                // Naviga verso una nuova pagina
                mainWindow.MainFrame.NavigationService.Navigate(new Statistiche(token));
                
            }
        }


        //###########################################################
        //caricamento schde statiche
        private async void LoadDataFromServer()
        {
            //FUNZIONE CHE STAMPA LE SCHEDE DI DEFAULT
            using (HttpClient client = new HttpClient())
            {

                string url = "http://localhost:5000/home";


                var content = new StringContent(JsonSerializer.Serialize("date"), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                string jsonString = await response.Content.ReadAsStringAsync();

                JsonResponse jsonResponse = JsonSerializer.Deserialize<JsonResponse>(jsonString);
                var trainingCards = jsonResponse.UserCards;


                foreach (UserCard trainingCard in trainingCards)
                {
                    cardPanel.HorizontalAlignment = HorizontalAlignment.Left;
                    // Verifica se un bottone con lo stesso CardId esiste già nel cardPanel
                    bool buttonExists = cardPanel.Children.OfType<Button>().Any(b => b.Name.Equals("bottone_" + trainingCard.CardId.ToString()));
                    if (!buttonExists)
                    {
                        Button button = new Button();
                        button.Name = "bottone_" + trainingCard.CardId.ToString();
                        button.Content = "Scheda " + trainingCard.CardId;
                        button.BorderBrush = new SolidColorBrush(Colors.DarkOliveGreen);
                        button.BorderThickness = new Thickness(1.5);
                        button.Background = new SolidColorBrush(Colors.WhiteSmoke);
                        button.FontSize = 13;
                        button.Width = cardPanel.ActualWidth;
                        button.Height = 30;
                        button.FontFamily = new FontFamily("Georgia");
                        button.Margin = new Thickness(0, 0, 0, 10);
                        cardPanel.Children.Add(button);

                        // Aggiunta del gestore dell'evento senza chiamarlo direttamente
                        button.Click += (sender, e) => CardButtonClick(sender, e, trainingCard);

                    }

                }

            }
        }

        private async void CardButtonClick(object sender, RoutedEventArgs e, UserCard trainingCard)
        {
            //stampa gli esercizi della scheda

            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                // Naviga verso una nuova pagina
                mainWindow.MainFrame.NavigationService.Navigate(new Page2(token, trainingCard));
                
            }
        }

        //#######################################################
        //caricamento schde create dall'utente
        private async void LoadCardFromDb()
        {
            //FUNZIONE CHE STAMPA LE SCHEDE CREATE DAGLI UTENTI
            using (HttpClient client = new HttpClient())
            {

                string url = "http://localhost:5000/LoadCardFromDb";
                var data = new { token };

                // Converti i dati in formato JSON
                string jsonData = JsonSerializer.Serialize(data);

                // Crea un oggetto StringContent con il JSON
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Esegui la richiesta HTTP POST
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Leggi la risposta come stringa
                string responseString = await response.Content.ReadAsStringAsync();

                JsonResponse jsonResponse = JsonSerializer.Deserialize<JsonResponse>(responseString);
                var trainingCards = jsonResponse.UserCards;

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                FavoriteResponse dati = JsonSerializer.Deserialize<FavoriteResponse>(responseString, options);
                var listfavorites = dati.favorites_list;



                foreach (var card in trainingCards)
                {
                   
                    bool buttonExists = CardPanel2.Children.OfType<Button>().Any(b => b.Name.Equals("bottone_" + card.CardId.ToString()));
                    if (!buttonExists)
                    {
                        Button button = new Button();
                        button.Name = "bottone_" + card.CardId.ToString();
                        button.Content = "Scheda " + card.CardId + "  Utente:" +card.UserId;
                        button.BorderBrush = new SolidColorBrush(Colors.DarkOliveGreen);
                        button.BorderThickness = new Thickness(1.5);
                        button.Background = new SolidColorBrush(Colors.WhiteSmoke);
                        button.FontSize = 12;
                        button.Width = cardPanel.ActualWidth -200;
                        button.Height = 30;
                        button.FontFamily = new FontFamily("Georgia");
                        button.Margin = new Thickness(0, 0, 0, 10);


                        foreach (var fav in listfavorites)
                        {
                            
                            if (int.Parse(fav.card_id) == card.CardId)
                            {
                                button.Background = new SolidColorBrush(Colors.DarkOliveGreen);

                            }
                        }

                        CardPanel2.Children.Add(button);
                        // Aggiunta del gestore dell'evento senza chiamarlo direttamente
                        button.Click += (sender, e) => UserCardButtonClick(sender, e, card);
                    }
                }


            }
        }

        private async void UserCardButtonClick(object sender, RoutedEventArgs e, UserCard trainingCard)
        {
            //stampa gli esercizi della scheda

            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                // Naviga verso una nuova pagina
                mainWindow.MainFrame.NavigationService.Navigate(new stampaUserCard(token, trainingCard));
                
            }
        }
    }

   
    
    public class JsonResponse
    {
        [JsonPropertyName("user_cards")]
        public List<UserCard> UserCards { get; set; }
    }

    public class UserCard
    {
        [JsonPropertyName("card_id")]
        public int CardId { get; set; }

        [JsonPropertyName("exercises")]
        public List<Exercise> Exercises { get; set; }

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
    }

    public class Exercise
    {
        [JsonPropertyName("day")]
        public string Day { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("reps")]
        public int Reps { get; set; }

        [JsonPropertyName("sets")]
        public int Sets { get; set; }
    }

    //favorites
    public class Favorite{
        [JsonPropertyName("user_id")]
        public string user_id { get; set; }

        [JsonPropertyName("card_id")]
        public string card_id { get; set; }
    }

    public class FavoriteResponse
    {
        [JsonPropertyName("favorites_list")]
        public List<Favorite> favorites_list { get; set; }
    }

}