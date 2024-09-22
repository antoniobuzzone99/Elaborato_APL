using Newtonsoft.Json.Linq;
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
    /// Logica di interazione per Page5.xaml
    /// </summary>
    /// pagina che mostra tutti gli esercizi
    public partial class Page5 : Page
    {
        private readonly string token;
        public Page5(string token)
        {
            InitializeComponent();
            this.token = token;
        }

        private void opt1(object sender, RoutedEventArgs e)
        {
            //stampa tutti gli esercizi
            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                // Naviga verso una nuova pagina
                mainWindow.MainFrame.NavigationService.Navigate(new stampaEs_tutti(token));
                
            }
        }

        private void opt2(object sender, RoutedEventArgs e)
        {
            //stampa esercizi spalle
            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                    // Naviga verso una nuova pagina
                    mainWindow.MainFrame.NavigationService.Navigate(new stampaEs_spalle(token));
                
            }
        }

        private void opt3(object sender, RoutedEventArgs e)
        {
            //stampa esercizi torace 
            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                    // Naviga verso una nuova pagina
                    mainWindow.MainFrame.NavigationService.Navigate(new stampaEs_torace(token));
                
            }
        }

        private void opt4(object sender, RoutedEventArgs e)
        {
            //stampa esercizi torace 
            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                    // Naviga verso una nuova pagina
                    mainWindow.MainFrame.NavigationService.Navigate(new stampaEs_addome(token));
                
            }
        }

        private void opt5(object sender, RoutedEventArgs e)
        {
            //stampa esercizi torace 
            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                    // Naviga verso una nuova pagina
                    mainWindow.MainFrame.NavigationService.Navigate(new stampaEs_schiena(token));
                
            }
        }

        private void opt6(object sender, RoutedEventArgs e)
        {
            //stampa esercizi torace 
            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                    // Naviga verso una nuova pagina
                    mainWindow.MainFrame.NavigationService.Navigate(new stampaEs_braccia(token));
                
            }
        }

        private void opt7(object sender, RoutedEventArgs e)
        {
            //stampa esercizi torace 
            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                    // Naviga verso una nuova pagina
                    mainWindow.MainFrame.NavigationService.Navigate(new stampaEs_glutei(token));
                
            }
        }

        private void opt8(object sender, RoutedEventArgs e)
        {
            //stampa esercizi torace 
            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                    // Naviga verso una nuova pagina
                    mainWindow.MainFrame.NavigationService.Navigate(new stampaEs_gambe(token));
                
            }
        }

        private void opt9(object sender, RoutedEventArgs e)
        {
            //stampa esercizi torace 
            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                    // Naviga verso una nuova pagina
                    mainWindow.MainFrame.NavigationService.Navigate(new stampaEs_cardio(token));
                
            }
        }

        private void opt10(object sender, RoutedEventArgs e)
        {
            //stampa esercizi torace 
            if (Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                
                    // Naviga verso una nuova pagina
                    mainWindow.MainFrame.NavigationService.Navigate(new stampaEs_stretching(token));
                
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
