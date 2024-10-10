using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;

class Program
{
    public static Dictionary<string, int[]> readCSV(string filename)
    {
        Dictionary<string, int[]> result = new Dictionary<string, int[]>();
        try
        {
            Console.WriteLine("Prebrana je bila datoteka " + filename);

            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + @"\data\";
            string pot = path + filename;

            string[] vrstice = File.ReadAllLines(pot);
            string[] alternative = vrstice[0].Split(',');
            string[] vrednosti1 = vrstice[1].Split(',');
            string[] vrednosti2 = vrstice[2].Split(',');

            for (int i = 1; i < alternative.Length; i++)
            {
                int[] rez = { Convert.ToInt32(vrednosti1[i]), Convert.ToInt32(vrednosti2[i]) };
                result.Add(alternative[i], rez);
            }

        }
        catch (Exception e)
        {
            Console.WriteLine("Napaka pri branju", e);
        }

        return result;
    }
    public static void prebranaDatoteka(string filename)
    {
        Dictionary<string, int[]> pariAlternativ = readCSV(filename);
        foreach (var item in pariAlternativ.Keys)
        {
            Console.WriteLine(item + " " + pariAlternativ[item][0] + "," + pariAlternativ[item][1]);
        }

    }

    public static string Optimist(Dictionary<string, int[]> dict) //maksimalna vrednost izmed maksimalnih
    {
        Dictionary<string, int> list = new Dictionary<string, int>();

        foreach (var key in dict.Keys)
        {
            list.TryAdd(key, dict[key].Max());
        }

        var maxValueKey = list.OrderByDescending(x => x.Value).First().Key;
        var maxValue = list.OrderByDescending(x => x.Value).First().Value;
        return maxValueKey + "(" + maxValue + ")";
    }

    public static string Pesimist(Dictionary<string, int[]> dict) //maksimalna vrednost izmed minimalnih
    {
        Dictionary<string, int> list = new Dictionary<string, int>();

        foreach (var key in dict.Keys)
        {
            list.TryAdd(key, dict[key].Min());
        }
        var minMaxValueKey = list.OrderByDescending(x => x.Value).First().Key;
        var minMaxValue = list.OrderByDescending(x => x.Value).First().Value;
        return minMaxValueKey + "(" + minMaxValue + ")";
    }

    public static string Laplace(Dictionary<string, int[]> dict) //maksimalna vrednost izmed povprecnih
    {
        Dictionary<string, double> list = new Dictionary<string, double>();

        foreach (var key in dict.Keys)
        {
            list.TryAdd(key, dict[key].Average());
        }
        var maxAvgValueKey = list.OrderByDescending(x => x.Value).First().Key;
        var maxAvgValue = list.OrderByDescending(x => x.Value).First().Value;
        return maxAvgValueKey + "(" + maxAvgValue + ")";
    }

    public static string Savage(Dictionary<string, int[]> dict) //najmanjse obzalovanje
    {
        Dictionary<string, double> prvaIzbira = new Dictionary<string, double>();
        Dictionary<string, double> drugaIzbira = new Dictionary<string, double>();
        Dictionary<string, double[]> obzalovanja = new Dictionary<string, double[]>();
        Dictionary<string, double> maksimalnaObzalovanja = new Dictionary<string, double>();

        foreach (var key in dict.Keys)
        {
            prvaIzbira.TryAdd(key, dict[key][0]);
            drugaIzbira.TryAdd(key, dict[key][1]);
        }

        var maxValueKeyPrva = prvaIzbira.OrderByDescending(x => x.Value).First().Key;
        var maxValuePrva = prvaIzbira.OrderByDescending(x => x.Value).First().Value;

        var maxValueKeyDruga = drugaIzbira.OrderByDescending(x => x.Value).First().Key;
        var maxValueDruga = drugaIzbira.OrderByDescending(x => x.Value).First().Value;


        foreach (var key in dict.Keys)
        {
            var obzalovanje1 = maxValuePrva - dict[key][0];
            var obzalovanje2 = maxValueDruga - dict[key][1];

            double[] skupekObzalovanj = { obzalovanje1, obzalovanje2 };

            obzalovanja.TryAdd(key, skupekObzalovanj);
        }

        foreach (var key in obzalovanja.Keys)
        {
            maksimalnaObzalovanja.TryAdd(key, obzalovanja[key].Max());
        }

        var minimalnoObzalovanjeKey = maksimalnaObzalovanja.OrderBy(x => x.Value).First().Key;
        var minimalnoObzalovanjeValue = maksimalnaObzalovanja.OrderBy(x => x.Value).First().Value;


        return minimalnoObzalovanjeKey + "(" + minimalnoObzalovanjeValue + ")";
    }


    public static Dictionary<string, List<double>> HurwitzevKriterij(Dictionary<string, int[]> dict) //h * dict[key].Max() + (1 - h) * dict[key].Min()
    {
        Dictionary<string, List<double>> hurwitzDict = new Dictionary<string, List<double>>();
        List<double> hji = new List<double>();

        for (double h = 0.0; h < 1; h += 0.1)
        {
            hji.Add(Math.Round(h, 2));
        }
        hurwitzDict.TryAdd("h", hji);

        foreach (var key in dict.Keys)
        {
            double h = 0.0;
            List<double> hurwitzVrednosti = new List<double>();

            while (h <= 1)
            {
                double hurwitz = Math.Round(h * dict[key].Max() + (1 - h) * dict[key].Min(), 2);

                hurwitzVrednosti.Add(hurwitz);

                h += 0.1;
            }

            hurwitzDict.TryAdd(key, hurwitzVrednosti);
        }

        return hurwitzDict;
    }

    public static void IzpisPodatkovHurwitz(Dictionary<string, List<double>> dictionary)
    {
        int i = 0;
        List<string> izpis = new List<string>();

        foreach (var key in dictionary.Keys)
        {
            izpis.Add(key);
        }

        for (i = 0; i < 11; i++)
        {
            foreach (var key in dictionary.Keys)
            {

                izpis.Add(dictionary[key][i].ToString());
            }
        }

        i = 0;
        foreach (var item in izpis)
        {
            if (i != 0 && i % dictionary.Keys.Count == 0)
            {
                Console.WriteLine();
            }

            Console.Write(String.Format("{0, -11}|", item));
            i++;
        }
    }

    public static void IzrisGrafa(Dictionary<string, List<double>> dictionary, string filename)
    {
        string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + @"\data\";

        var model = new PlotModel();
        var h = new Dictionary<string, List<double>>();
        LinearAxis x = new LinearAxis();
        LinearAxis y = new LinearAxis();

        Legend legend = new Legend();
        legend.LegendMaxWidth = 400;
        legend.LegendMaxHeight = 100;
        legend.LegendBorderThickness = 10;
        legend.LegendPlacement = LegendPlacement.Outside;
        legend.LegendPosition = LegendPosition.BottomCenter;
        legend.LegendOrientation = LegendOrientation.Horizontal;
        legend.LegendBorderThickness = 1;
        legend.LegendSymbolPlacement = LegendSymbolPlacement.Left;

        x.Position = AxisPosition.Bottom;
        x.Title = "h";

        x.Minimum = dictionary["h"].Min();
        x.Maximum = dictionary["h"].Max();


        y.Position = AxisPosition.Left;
        y.Title = "Vrednost alternativ";
        y.MajorStep = 5;
        y.Minimum = 0;

        int j = 0;
        foreach (var key in dictionary.Keys.Skip(1))
        {
            int i = 0;
            LineSeries alt = new LineSeries();
            y.Maximum = dictionary[key].Max() + 20;

            alt.MarkerType = MarkerType.Circle; 
            
            alt.Title = key;
            foreach (var item in dictionary[key])
            {
                alt.LineStyle = LineStyle.Solid;
                DataPoint dp = new DataPoint(dictionary["h"][i], item);
                alt.Points.Add(dp);
                i++;
            }
            model.Series.Add(alt);

        }

        model.Title = "Vrednotenje z Hurwitzerjevem kriterijem";
        model.Axes.Add(x);
        model.Axes.Add(y);
        model.Legends.Add(legend);

        try
        {
            using (var stream = File.Create(path + filename))
            {
                var pdfExporter = new PdfExporter { Width = 600, Height = 400 };
                pdfExporter.Export(model, stream);
            };

            Console.WriteLine($"Graf je shranjen v datoteko (/data/{filename})");

        }
        catch (Exception e)
        {
            Console.WriteLine("Napaka pri shranjevanju grafa...", e);
        }

    }

    public static void Main(string[] args)
    {
        string filename = "prodaja";
        string csv = ".csv";
        string pdf = ".pdf";

        Console.WriteLine("Izpis osnovnih metod odločanja");
        Dictionary<string, int[]> datoteka = readCSV(filename + csv);

        Console.WriteLine("\n");
        Console.WriteLine("Optimist: " + Optimist(datoteka));
        Console.WriteLine("Pesimist: " + Pesimist(datoteka));
        Console.WriteLine("Laplace: " + Laplace(datoteka));
        Console.WriteLine("Savage: " + Savage(datoteka));
        Console.WriteLine("\n");

        Console.WriteLine("Hurwiczev kriterij:");
        HurwitzevKriterij(datoteka);
        IzpisPodatkovHurwitz(HurwitzevKriterij(datoteka));
        Console.WriteLine("\n");
        IzrisGrafa(HurwitzevKriterij(datoteka), filename + pdf);
    }
}