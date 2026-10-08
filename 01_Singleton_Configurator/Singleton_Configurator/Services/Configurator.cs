using System.Globalization;
using System.IO;

namespace Singleton_Configurator.Services
{
    /// <summary>
    /// Singleton, der die vier Eingabewerte in ConfigFile.txt speichert und lädt.
    /// </summary>
    public sealed class Configurator
    {
        // private Variablen
        private static Configurator? instance;
        private static readonly object _lock = new object();
        private readonly string fileName;

        // öffentliche Properties für Wertübergabe (Standardwerte)
        public decimal PriceNetto { get; set; } = 10.00m;
        public int Quantity { get; set; } = 2;
        public decimal DiscountPercent { get; set; } = 20m;
        public decimal VatPercent { get; set; } = 16m;

        // privater Konstruktor
        private Configurator()
        {
            fileName = Path.Combine(AppContext.BaseDirectory, "ConfigFile.txt");
        }

        // Singleton-Instanz (thread-safe)
        public static Configurator GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new Configurator();
                    }
                }
            }
            return instance;
        }

        public void Load()
        {
            if (!File.Exists(fileName))
            {
                Save(); 
                return;
            }

            foreach (string rawLine in File.ReadAllLines(fileName))
            {
                string line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith("#"))
                    continue;

                int idx = line.IndexOf('=');
                if (idx < 0)
                    continue;

                string key = line.Substring(0, idx).Trim().ToLowerInvariant();
                string value = line.Substring(idx + 1).Trim();
                CultureInfo ci = CultureInfo.InvariantCulture;

                switch (key)
                {
                    case "price_netto":
                        if (decimal.TryParse(value, NumberStyles.Number, ci, out decimal p)) PriceNetto = p;
                        break;
                    case "quantity":
                        if (int.TryParse(value, NumberStyles.Integer, ci, out int q)) Quantity = q;
                        break;
                    case "discount_percent":
                        if (decimal.TryParse(value, NumberStyles.Number, ci, out decimal d)) DiscountPercent = d;
                        break;
                    case "vat_percent":
                        if (decimal.TryParse(value, NumberStyles.Number, ci, out decimal v)) VatPercent = v;
                        break;
                }
            }
        }

        public void Save()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            string[] lines =
            {
                "# HTL-Clubbing Calculator Config",
                "price_netto=" + PriceNetto.ToString("0.00", ci),
                "quantity=" + Quantity.ToString(ci),
                "discount_percent=" + DiscountPercent.ToString("0.##", ci),
                "vat_percent=" + VatPercent.ToString("0.##", ci)
            };
            File.WriteAllLines(fileName, lines);
        }
    }
}
