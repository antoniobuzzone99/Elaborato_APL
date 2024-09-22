using System;
using System.Collections.Generic;
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
    /// Logica di interazione per stampaEs_stretching.xaml
    /// </summary>
    public partial class stampaEs_stretching : Page
    {
        private readonly string token;
        public stampaEs_stretching(string token)
        {
            InitializeComponent();
            this.token = token;
            LoadExercise();
        }

        private async void LoadExercise()
        {
            using (HttpClient client = new HttpClient())
            {

                string url = "http://localhost:5000/loadExer";

                var content = new StringContent(JsonSerializer.Serialize("date"), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                ExerciseResponse data = JsonSerializer.Deserialize<ExerciseResponse>(jsonResponse, options);

                // Raggruppa gli esercizi per MuscleGroup
                var groupedExercises = data.exercise_list.GroupBy(e => e.muscle_group);

                // Itera attraverso i gruppi e stampa gli esercizi
                foreach (var group in groupedExercises)
                {
                    if (group.Key == "stretching")
                    {
                        Label labelMuscleGroup = new Label();
                        labelMuscleGroup.Content = $"Muscle Group: {group.Key}";
                        labelMuscleGroup.FontSize = 14;
                        labelMuscleGroup.FontWeight = FontWeights.Bold;
                        labelMuscleGroup.FontFamily = new FontFamily("Georgia");
                        Panel.Children.Add(labelMuscleGroup);
                        foreach (var exercise in group)
                        {
                            Label labelExercise = new Label();
                            labelExercise.Content = $"Name: {exercise.name}";
                            Panel.Children.Add(labelExercise);
                        }
                    }
                }

            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                    // Naviga verso una nuova pagina
                    mainWindow.MainFrame.NavigationService.Navigate(new Page5(token));
                
            }
        }

    }

}