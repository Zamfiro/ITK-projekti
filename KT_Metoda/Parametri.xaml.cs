using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Nikolić_KT_Metoda
{
    /// <summary>
    /// Interaction logic for Parametri.xaml
    /// </summary>
    public partial class Parametri : Window
    {
        public Parametri()
        {
            InitializeComponent();
        }

        public static List<Parameter> seznamParametrov = new List<Parameter>();


        private void IzbrisiParameter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            paramList.Items.Remove(paramList.SelectedItem);
        }

        private void DodajParam_Click(object sender, RoutedEventArgs e)
        {    
            ListBoxItem item = new ListBoxItem();
            item.Content = parameter.Text + " (" + utez.Text + ")";
            item.MouseDoubleClick += IzbrisiParameter_MouseDoubleClick;
            paramList.Items.Add(item);
            parameter.Text = "";
            utez.Text = "";
        }

        private void Ocene_Click(object sender, RoutedEventArgs e)
        {
            foreach(ListBoxItem item in paramList.Items)
            {
                string p = (string)item.Content;
                string ime = p.Substring(0, p.IndexOf("(") - 1);
                string utez = p.Substring(p.IndexOf("(")).Trim('(',')');
                
                Parameter noviParameter = new Parameter(ime, Convert.ToInt32(utez));

                seznamParametrov.Add(noviParameter);
            }

            Ocene ocene = new Ocene();

            ocene.Show();
        }
    }
}
