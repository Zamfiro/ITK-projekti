using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
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

namespace Nikolić_KT_Metoda
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }
        
        public static List<string> alternative = new List<string>();

        private void IzbrisiAlternativo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            altList.Items.Remove(altList.SelectedItem);
        }

        private void DodajAlt_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = new ListBoxItem();
            
            item.Content = alternativa.Text;
            alternativa.Text = "";
            item.MouseDoubleClick += IzbrisiAlternativo_MouseDoubleClick;
            altList.Items.Add(item);
        }

        private void Parametri_Click(object sender, RoutedEventArgs e)
        {
            foreach(ListBoxItem item in altList.Items)
            {
                Debug.WriteLine(item.Content);
                alternative.Add((string)item.Content);
            }


            Parametri param = new Parametri();
            param.Show();
        }


    }
}
