using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Logica di interazione per stampaUserCard.xaml
    /// </summary>
    public partial class stampaUserCard : Page
    {
        private readonly string token;
        public stampaUserCard(string token, UserCard trainingCard)
        {
            this.token = token;
            InitializeComponent();


            Label CardId = new Label();
            CardId.Content = $"Card Id: {trainingCard.CardId}";
            CardId.FontSize = 22;
            CardId.Foreground = Brushes.DarkOliveGreen;
            CardId.FontWeight = FontWeights.Bold;
            CardId.FontFamily = new FontFamily("Georgia");
            cardPanel.Children.Add(CardId);

            Button addFavorites = new Button();
            addFavorites.Content = "Aggiungi ai preferiti";
            addFavorites.Click += (sender, e) => addFavoriteClick(sender, e, trainingCard.CardId);
            addFavorites.Width = 200;
            addFavorites.Margin = new Thickness(0, 10, 0, 10);
            addFavorites.HorizontalAlignment = HorizontalAlignment.Left;
            cardPanel.Children.Add(addFavorites);

            Button RemoveFavorites = new Button();
            RemoveFavorites.Content = "Rimuovi dai preferiti";
            RemoveFavorites.Click += (sender, e) => RemoveFavoriteClick(sender, e, trainingCard.CardId, addFavorites);
            RemoveFavorites.Width = 200;
            RemoveFavorites.HorizontalAlignment = HorizontalAlignment.Left;
            RemoveFavorites.Margin = new Thickness(0, 0, 0, 20);
            cardPanel.Children.Add(RemoveFavorites);

            checkFavoritesAsync(trainingCard, addFavorites, RemoveFavorites);

            // Raggruppa gli esercizi per giorno
            var groupedExercises = trainingCard.Exercises.GroupBy(e => e.Day);

            // Itera attraverso i gruppi e stampa gli esercizi
            foreach (var group in groupedExercises)
            {
                Label labelday = new Label();
                labelday.Content = $"Day: {group.Key}";
                labelday.FontSize = 14;
                labelday.FontWeight = FontWeights.Bold;
                labelday.FontFamily = new FontFamily("Georgia");
                cardPanel.Children.Add(labelday);
                foreach (var exercise in group)
                {
                    Label labelExercise = new Label();
                    labelExercise.Content = $"Name: {exercise.Name} - Set: {exercise.Sets} Rep: {exercise.Reps}";
                    cardPanel.Children.Add(labelExercise);
                }
            }
        }

        private async void addFavoriteClick(object sender, RoutedEventArgs e, int trainingCrad_id) {

            using (HttpClient client = new HttpClient())
            {
                string url = $"http://localhost:5000/add_favorite_card";
                var data = new { token , trainingCrad_id };

                // Converti i dati in formato JSON
                string jsonData = JsonSerializer.Serialize(data);

                // Crea un oggetto StringContent con il JSON
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Esegui la richiesta HTTP POST
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Leggi la risposta come stringa
                string responseString = await response.Content.ReadAsStringAsync();


                // Controlla la risposta JSON per il successo
                //var responseObject = JsonSerializer.Deserialize<Dictionary<string, int>>(responseString);
                //int state = (int)responseObject["state"];
            }
        }

        private async void RemoveFavoriteClick(object sender, RoutedEventArgs e, int trainingCrad_id, Button addFavorites)
        {

            using (HttpClient client = new HttpClient())
            {
                string url = $"http://localhost:5000/remove_favorite_card";
                var data = new { token, trainingCrad_id };

                // Converti i dati in formato JSON
                string jsonData = JsonSerializer.Serialize(data);

                // Crea un oggetto StringContent con il JSON
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Esegui la richiesta HTTP POST
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Leggi la risposta come stringa
                string responseString = await response.Content.ReadAsStringAsync();

                //Controlla la risposta JSON per il successo
                var responseObject = JsonSerializer.Deserialize<Dictionary<string, int>>(responseString);
                int state = (int)responseObject["state"];

                if (state == 0) {
                    addFavorites.IsEnabled = true;
                }
            }
        }

        private async void checkFavoritesAsync(UserCard training_card, Button addFavorites, Button RemoveFavorites) {

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

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                FavoriteResponse dati = JsonSerializer.Deserialize<FavoriteResponse>(responseString, options);
                var listfavorites = dati.favorites_list;

                foreach (var fav in listfavorites) {
                    //se la scheda è tra i preferiti disattivo il tasto di aggiunta
                    if (int.Parse(fav.card_id) == training_card.CardId)
                    {
                        addFavorites.IsEnabled = false;
                        RemoveFavorites.IsEnabled = true;
                    }
                    else {
                        addFavorites.IsEnabled = true;
                        RemoveFavorites.IsEnabled = false;
                    }
                }

            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                    // Naviga verso una nuova pagina
                    mainWindow.MainFrame.NavigationService.Navigate(new Page1(token));
                
            }
        }
    }
}
