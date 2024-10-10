using Microsoft.Win32;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows;

namespace Frekvenčna_analiza
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal Besedilo besedila = new Besedilo();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = besedila;
            GrafSifriranega();
            GrafReferencnega();
            GrafDesifriranega();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Ponastavi_Click(object sender, RoutedEventArgs e)
        {
            referencniGraf.Reset();
            sifriranGraf.Reset();
            desifriranGraf.Reset();
            GrafSifriranega();
            GrafReferencnega();
            GrafDesifriranega();
            referencno_besedilo.Text = besedila.referencnoBesedilo;
            sifrirano_besedilo.Text = besedila.sifriranoBesedilo;
            desifrirano_besedilo.Text = besedila.sifriranoBesedilo;
        }

        private void zamenjava_Click(object sender, RoutedEventArgs e)
        {
            desifriranGraf.Reset();

            char c1 = Convert.ToChar(crka1.Text);
            char c2 = Convert.ToChar(crka2.Text);

            if (sifrirano_besedilo.Text != String.Empty)
            {
                string temp = desifrirano_besedilo.Text.ToUpper().Replace(c1,c2);
                desifrirano_besedilo.Text = temp;
            }
            GrafDesifriranega();

        }

        private void Desifriraj_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<char, double> expectedFrequencies = new Dictionary<char, double>()
            {
                {'E', 0.1202}, {'T', 0.0910}, {'A', 0.0812}, {'O', 0.0768}, {'I', 0.0731}, {'N', 0.0695},
                {'S', 0.0628}, {'R', 0.0602}, {'H', 0.0592}, {'D', 0.0432}, {'L', 0.0398}, {'U', 0.0288},
                {'C', 0.0271}, {'M', 0.0261}, {'F', 0.0230}, {'Y', 0.0211}, {'W', 0.0209}, {'G', 0.0203},
                {'P', 0.0182}, {'B', 0.0149}, {'V', 0.0111}, {'K', 0.0069}, {'X', 0.0017}, {'Q', 0.0011},
                {'J', 0.0010}, {'Z', 0.0007}
            };

            Dictionary<char, int> frequencies = new Dictionary<char, int>();
            foreach (char c in sifrirano_besedilo.Text.ToUpper())
            {
                if (Char.IsLetter(c))
                {
                    if (frequencies.ContainsKey(c))
                    {
                        frequencies[c]++;
                    }
                    else
                    {
                        frequencies.Add(c, 1);
                    }
                }
            }


            var sortedFrequencies = frequencies.OrderByDescending(x => x.Value);

            Dictionary<char, char> letterMap = new Dictionary<char, char>();
            int i = 0;
            foreach (var item in sortedFrequencies)
            {
                letterMap.Add(item.Key, expectedFrequencies.ElementAt(i).Key);
                i++;
            }

            string decodedString = "";
            foreach (char c in sifrirano_besedilo.Text)
            {
                if (Char.IsLetter(c))
                {
                    if (Char.IsLower(c))
                    {
                        decodedString += Char.ToLower(letterMap[Char.ToUpper(c)]);
                    }
                    else
                    {
                        decodedString += letterMap[c];
                    }
                }
                else
                {
                    decodedString += c;
                }
            }

            desifrirano_besedilo.Text = decodedString;
            GrafDesifriranega();
        }

        string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

        private void SifriranoBesedilo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog uvoziDialog = new OpenFileDialog();
            uvoziDialog.Filter = "TXT datoteke (*.txt)|*.txt";
            uvoziDialog.InitialDirectory = path + "\\besedila";


            if (uvoziDialog.ShowDialog() == true)
            {
                try
                {

                    using (var sr = new StreamReader(uvoziDialog.FileName))
                    {
                        Besedilo besedilo = new Besedilo(sr.ReadToEnd(), String.Empty);
                        besedila.sifriranoBesedilo = besedilo.sifriranoBesedilo;
                        sifrirano_besedilo.Text = besedilo.sifriranoBesedilo;
                        desifrirano_besedilo.Text = besedilo.sifriranoBesedilo;
                        GrafSifriranega();
                        GrafDesifriranega();
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void ReferencnoBesedilo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog uvoziDialog = new OpenFileDialog();
            uvoziDialog.Filter = "TXT datoteke (*.txt)|*.txt";
            uvoziDialog.InitialDirectory = path + "\\besedila";


            if (uvoziDialog.ShowDialog() == true)
            {
                try
                {

                    using (var sr = new StreamReader(uvoziDialog.FileName))
                    {
                        Besedilo refBesedilo = new Besedilo(String.Empty, sr.ReadToEnd());
                        besedila.referencnoBesedilo = refBesedilo.referencnoBesedilo;
                        referencno_besedilo.Text = refBesedilo.referencnoBesedilo;
                        GrafReferencnega();
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(ex.Message);
                }
            }
        }



        private void GrafSifriranega()
        {
            string crke = sifrirano_besedilo.Text.ToUpper();
            char[] posamezneCrke = new char[] { 'A', 'B', 'C', 'Č', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'Š', 'T', 'U', 'V', 'Z', 'Ž', 'X', 'Y', 'W' };
            double[] posBar = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27 };
            Dictionary<string, double> dataY = new Dictionary<string, double>();
            List<double> podatki = new List<double>();
            List<string> posCrkStr = new List<string>();

            foreach (char crka in posamezneCrke)
            {
                int frekvenca = crke.Count(f => f == crka);
                dataY.Add(crka.ToString(), frekvenca);
            }

            

            if(sifrirano_besedilo.Text != String.Empty)
            {
                foreach(var item in dataY.OrderByDescending(x => x.Value))
                {
                    posCrkStr.Add(item.Key);
                    podatki.Add(item.Value);
                }
                double[] Yaxis = podatki.ToArray();
                sifriranGraf.Plot.AddBar(Yaxis, posBar);
                sifriranGraf.Plot.XTicks(posBar, posCrkStr.ToArray());
                sifriranGraf.Plot.YAxis.ManualTickSpacing(10);
                sifriranGraf.Plot.AxisAuto();

                sifriranGraf.Plot.Title("Frekvenčna analiza šifriranega besedila");
                sifriranGraf.Plot.XLabel("Črke");
                sifriranGraf.Plot.YLabel("Frekvenca");

                sifriranGraf.Refresh();
            }


        }

        private void GrafReferencnega()
        {
            string crke = referencno_besedilo.Text.ToUpper();
            char[] posamezneCrke = new char[] { 'A', 'B', 'C', 'Č', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'Š', 'T', 'U', 'V', 'Z', 'Ž', 'X', 'Y', 'W' };
            double[] posBar = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27 };
            Dictionary<string, double> dataY = new Dictionary<string, double>();
            List<double> podatki = new List<double>();
            List<string> posCrkStr = new List<string>();

            foreach (char crka in posamezneCrke)
            {
                int frekvenca = crke.Count(f => f == crka);
                dataY.Add(crka.ToString(), frekvenca);
            }



            if (sifrirano_besedilo.Text != String.Empty)
            {
                foreach (var item in dataY.OrderByDescending(x => x.Value))
                {
                    posCrkStr.Add(item.Key);
                    podatki.Add(item.Value);
                }
                double[] Yaxis = podatki.ToArray();
                referencniGraf.Plot.AddBar(Yaxis, posBar);
                referencniGraf.Plot.XTicks(posBar, posCrkStr.ToArray());
                referencniGraf.Plot.YAxis.ManualTickSpacing(10);
                referencniGraf.Plot.AxisAuto();

                referencniGraf.Plot.Title("Frekvenčna analiza šifriranega besedila");
                referencniGraf.Plot.XLabel("Črke");
                referencniGraf.Plot.YLabel("Frekvenca");

                referencniGraf.Refresh();
            }
        }

        private void GrafDesifriranega()
        {
            string crke = desifrirano_besedilo.Text.ToUpper();
            char[] posamezneCrke = new char[] { 'A', 'B', 'C', 'Č', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'Š', 'T', 'U', 'V', 'Z', 'Ž', 'X', 'Y', 'W' };
            double[] posBar = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27 };
            Dictionary<string, double> dataY = new Dictionary<string, double>();
            List<double> podatki = new List<double>();
            List<string> posCrkStr = new List<string>();

            foreach (char crka in posamezneCrke)
            {
                int frekvenca = crke.Count(f => f == crka);
                dataY.Add(crka.ToString(), frekvenca);
            }



            if (sifrirano_besedilo.Text != String.Empty)
            {
                foreach (var item in dataY.OrderByDescending(x => x.Value))
                {
                    posCrkStr.Add(item.Key);
                    podatki.Add(item.Value);
                }
                double[] Yaxis = podatki.ToArray();
                desifriranGraf.Plot.AddBar(Yaxis, posBar);
                desifriranGraf.Plot.XTicks(posBar, posCrkStr.ToArray());
                desifriranGraf.Plot.YAxis.ManualTickSpacing(10);
                desifriranGraf.Plot.AxisAuto();

                desifriranGraf.Plot.Title("Frekvenčna analiza dešifriranega besedila");
                desifriranGraf.Plot.XLabel("Črke");
                desifriranGraf.Plot.YLabel("Frekvenca");

                desifriranGraf.Refresh();
            }
        }

        private void Izvozi_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog izvoziDialog = new SaveFileDialog();
            izvoziDialog.InitialDirectory = path + "\\besedila";

            izvoziDialog.DefaultExt = "txt";
            izvoziDialog.AddExtension = true;

            if (izvoziDialog.ShowDialog() == true)
            {
                try
                {

                    using (var sr = new StreamWriter(izvoziDialog.FileName))
                    {
                        sr.WriteLine(desifrirano_besedilo.Text);
                        sr.Flush();
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine("The file could not be saved:");
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
