using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Nikolić_KT_Metoda
{
    /// <summary>
    /// Interaction logic for Ocene.xaml
    /// </summary>
    public partial class Ocene : Window
    {
        public Ocene()
        {
            InitializeComponent();
            UstvariTabelo();
        }

        List<Alternativa> seznamAlternativ = new List<Alternativa>();

        public void UstvariTabelo()
        {
            StackPanel prviColumn = new StackPanel();
            Label prazen = new Label();
            prazen.Content = "    ";
            prazen.MinWidth = 75;
            prazen.MinHeight = 25;
            prviColumn.Orientation = System.Windows.Controls.Orientation.Vertical;
            prviColumn.Children.Add(prazen);
            Container.Children.Add(prviColumn);
            foreach (var parameter in Parametri.seznamParametrov)
            {
                Label parameterTxt = new Label();
                parameterTxt.BorderBrush = System.Windows.Media.Brushes.Black;
                parameterTxt.BorderThickness = new Thickness(1);
                parameterTxt.MinWidth = 75;
                parameterTxt.MinHeight = 31;
                parameterTxt.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                parameterTxt.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                parameterTxt.Content = parameter.Ime + " (" + parameter.Utez + ")";
                parameterTxt.Name = parameter.Ime;
                parameterTxt.MouseDoubleClick += Parameter_MouseDoubleClick;
                prviColumn.Children.Add(parameterTxt);
            }

            StackPanel tabela = new StackPanel();
            tabela.Orientation = System.Windows.Controls.Orientation.Horizontal;
            tabela.Name = "tabela";

            foreach (var item in MainWindow.alternative)
            {
                StackPanel columns = new StackPanel();
                columns.Orientation = System.Windows.Controls.Orientation.Vertical;

                Label alternativa = new Label();
                alternativa.BorderBrush = System.Windows.Media.Brushes.Black;
                alternativa.BorderThickness = new Thickness(1);
                alternativa.MinWidth = 75;
                alternativa.MinHeight = 25;
                alternativa.Content = item;
                alternativa.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                alternativa.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;

                columns.Children.Add(alternativa);
                foreach (var parameter in Parametri.seznamParametrov)
                {
                    TextBox ocena = new TextBox();
                    ocena.MinWidth = 75;
                    ocena.MinHeight = 31;
                    ocena.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                    ocena.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                    ocena.Name = $"{parameter.Ime}{item}";
                    columns.Children.Add(ocena);
                }
                tabela.Children.Add(columns);
            }
            Container.Children.Add(tabela);
        }

        private void Izracun_Click(object sender, RoutedEventArgs e)
        {
            StackPanel tabela = (StackPanel)Container.Children[Container.Children.Count - 1];
            foreach (StackPanel column in tabela.Children)
            {
                Alternativa alternativa = new Alternativa();
                List<int> oceneSeznam = new List<int>();

                foreach (var vrednost in column.Children)
                {
                    if (vrednost is Label)
                    {
                        alternativa.Ime = ((Label)vrednost).Content.ToString();
                    }
                    else if (vrednost is TextBox)
                    {
                        oceneSeznam.Add(Convert.ToInt32(((TextBox)vrednost).Text));
                    }
                }
                alternativa.Ocene = oceneSeznam;
                seznamAlternativ.Add(alternativa);
            }

            Odlocanje(seznamAlternativ);

        }

        Dictionary<string, int> seznamOcenjenih = new Dictionary<string, int>();

        private void Odlocanje(List<Alternativa> seznamAlternativ)
        {

            seznamOcenjenih.Clear();

            foreach (var alternativa in seznamAlternativ)
            {
                int koncnaOcena = 0;
                List<int> utezeneOcene = new List<int>();
                int i = 0;
                foreach (int ocena in alternativa.Ocene)
                {
                    int izracun = ocena * Parametri.seznamParametrov[i].Utez;
                    utezeneOcene.Add(izracun);
                    i++;
                }
                koncnaOcena = utezeneOcene.Sum();
                seznamOcenjenih.Add(alternativa.Ime, koncnaOcena);
            }

            string najboljsaAlt = seznamOcenjenih.OrderByDescending(x => x.Value).First().Key + "(" + seznamOcenjenih.OrderByDescending(x => x.Value).First().Value + ")";

            KoncnaOcena.Content = "Najboljša alternativa: " + najboljsaAlt;
            IzrisBarChart();
            IzrisPieChart();

        }

        private void IzrisBarChart()
        {
            List<double> oceneList = new List<double>();
            List<double> posList = new List<double>();
            List<string> labelList = new List<string>();


            int pos = 0;
            while (pos < seznamOcenjenih.Keys.Count)
            {
                posList.Add(pos);
                pos++;
            }

            foreach (var alternativa in seznamOcenjenih)
            {
                labelList.Add(alternativa.Key);
                oceneList.Add(alternativa.Value);
            }

            BarGraph.Plot.AddBar(oceneList.ToArray(), posList.ToArray());
            BarGraph.Plot.XTicks(posList.ToArray(), labelList.ToArray());
            BarGraph.Plot.SetAxisLimits(yMin: 0);
            BarGraph.Plot.YAxis.ManualTickSpacing(20);

            BarGraph.Plot.Title("Primerjava alternativ");
            BarGraph.Plot.XLabel("Alternative");
            BarGraph.Plot.YLabel("Vrednost");

            BarGraph.Refresh();
        }

        private void IzrisPieChart()
        {
            List<double> oceneList = new List<double>();
            List<double> posList = new List<double>();
            List<string> labelList = new List<string>();

            System.Drawing.Color[] sliceColors =
            {
                ColorTranslator.FromHtml("#178600"),
                ColorTranslator.FromHtml("#B07219"),
                ColorTranslator.FromHtml("#3572A5"),
                ColorTranslator.FromHtml("#B845FC"),
                ColorTranslator.FromHtml("#4F5D95"),
            };

            System.Drawing.Color[] labelColors =
                new System.Drawing.Color[] {
                System.Drawing.Color.FromArgb(255, System.Drawing.Color.White),
                System.Drawing.Color.FromArgb(100, System.Drawing.Color.White),
                System.Drawing.Color.FromArgb(250, System.Drawing.Color.White),
                System.Drawing.Color.FromArgb(150, System.Drawing.Color.White),
                System.Drawing.Color.FromArgb(200, System.Drawing.Color.White),
            };

            foreach (var parameter in Parametri.seznamParametrov)
            {
                labelList.Add(parameter.Ime);
                double ocenaParametra = 0;

                foreach (var alternativa in seznamAlternativ)
                {
                    ocenaParametra = ocenaParametra + (double)alternativa.Ocene[Parametri.seznamParametrov.FindIndex(x => x.Ime.Contains(parameter.Ime))];

                }
                oceneList.Add(ocenaParametra);
            }

            var pie = PieChart.Plot.AddPie(oceneList.ToArray());
            pie.Explode = true;
            pie.ShowLabels = true;
            pie.SliceLabels = labelList.ToArray();
            pie.SliceFillColors = sliceColors;
            pie.SliceLabelColors = labelColors;

            PieChart.Plot.Title("Parametri");
            PieChart.Refresh();

        }

        private void Parameter_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            PodrobenGraf podrobenGraf = new PodrobenGraf();

            Dictionary<string, double> altOcena = new Dictionary<string, double>();
            Dictionary<string, List<double>> utezenAlt = new Dictionary<string, List<double>>();
            double[] ys = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };


            List<double> oceneList = new List<double>();
            List<double> posList = new List<double>();
            List<string> labelList = new List<string>();

            Label param = (Label)sender;
            string parameter = (string)param.Content;
            string sliced = parameter.Substring(0, parameter.IndexOf("(") - 1);


            foreach (var alternativa in seznamAlternativ)
            {
                double ocenaParametra = (double)alternativa.Ocene[Parametri.seznamParametrov.FindIndex(x => x.Ime.Contains(sliced))];

                altOcena.Add(alternativa.Ime, ocenaParametra);
            }

            List<double> utezeneOcene = new List<double>();
            foreach (var item in altOcena)
            {
                List<double> utezi = new List<double>();
                double utezena = 0;
                int i = 0;
                while (i <= 10)
                {
                    utezena = item.Value * i;
                    utezi.Add(utezena);
                    i++;
                }
                utezenAlt.Add(item.Key, utezi);
            }

            podrobenGraf.Title = parameter;
            podrobenGraf.LineChart.Plot.Title(parameter);

            foreach (var alt in utezenAlt)
            {

                podrobenGraf.LineChart.Plot.AddScatter(alt.Value.ToArray(), ys, label: alt.Key);
            }

            podrobenGraf.LineChart.Refresh();

            podrobenGraf.Show();
        }
    }
}
