using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1;

namespace TrainUp_Client
{
    /// <summary>
    /// Logica di interazione per Page2.xaml
    /// </summary>
    /// pagina stampa schede da evento dalla home
    public partial class Page2 : Page
    {
        private readonly string token;
        public Page2(string token, UserCard trainingCard)
        {
            
            InitializeComponent();
            this.token = token;

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
                    labelExercise.Margin = new Thickness(0, 0, 0, -5);
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
