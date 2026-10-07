using System.ComponentModel;
using System.Globalization;
using System.Windows;
using Singleton_Configurator.Services;

namespace Singleton_Configurator
{
    public partial class MainWindow : Window
    {
        private readonly Configurator config = Configurator.GetInstance();
        private readonly Logger logger = Logger.GetInstance();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Beim Öffnen: Werte aus ConfigFile.txt laden
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            config.Load();
            txtPrice.Text = config.PriceNetto.ToString("0.00");
            txtQuantity.Text = config.Quantity.ToString();
            txtDiscount.Text = config.DiscountPercent.ToString("0.##");
            txtVat.Text = config.VatPercent.ToString("0.##");
        }

        private void BtnCalc_Click(object sender, RoutedEventArgs e)
        {
            logger.LogTrace("Calc started...");

            if (!TryReadInputs(out decimal price, out int qty, out decimal discount, out decimal vat))
            {
                logger.LogTrace("Calc aborted.");
                return;
            }

            // Warnungen: App läuft weiter
            if (qty == 0)
                logger.LogWarn("Menge = 0 → Berechnung möglicherweise unsinnig.");
            if (discount > 50)
                logger.LogWarn($"Hoher Rabatt: {discount:0.##}%");

            decimal nettoGesamt = price * qty;
            decimal rabatt = nettoGesamt * discount / 100m;
            decimal netto = nettoGesamt - rabatt;
            decimal tax = netto * vat / 100m;
            decimal brutto = netto + tax;

            logger.LogDebug($"Zwischenergebnis: Netto (vor Rabatt)={nettoGesamt:0.00}");
            logger.LogDebug($"Zwischenergebnis: Rabatt={rabatt:0.00}");
            logger.LogDebug($"Zwischenergebnis: Netto nach Rabatt={netto:0.00}");
            logger.LogDebug($"Zwischenergebnis: Steuer={tax:0.00}");

            lblNetto.Text = netto.ToString("N2");
            lblVat.Text = tax.ToString("N2");
            lblBrutto.Text = brutto.ToString("N2");

            logger.LogInfo($"Berechnung OK | Preis={price:0.00}; Menge={qty}; Rabatt%={discount:0.##}; Steuer%={vat:0.##} | " +
                           $"Netto={netto:0.00}; Steuer={tax:0.00}; Brutto={brutto:0.00}");

            // nach jeder Berechnung speichern
            config.Save();

            logger.LogTrace("Calc finished.");
        }

        // Beim Schließen: Werte speichern (sofern gültig)
        private void Window_Closing(object? sender, CancelEventArgs e)
        {
            TryReadInputs(out _, out _, out _, out _, interactive: false);
            config.Save();
        }

        /// <summary>
        /// Liest die Eingaben (akzeptiert "10,00" und "10.00") und überträgt sie in den Configurator.
        /// interactive=false: keine MessageBox und kein Logging (z.B. beim Schließen).
        /// </summary>
        private bool TryReadInputs(out decimal price, out int qty, out decimal discount, out decimal vat, bool interactive = true)
        {
            qty = 0; discount = 0; vat = 0;
            bool ok = true;

            if (!TryParseDecimal(txtPrice.Text, out price) || price < 0)
                ok = InputError("Preis", interactive);
            else if (!int.TryParse(txtQuantity.Text.Trim(), out qty) || qty < 0)
                ok = InputError("Menge", interactive);
            else if (!TryParseDecimal(txtDiscount.Text, out discount) || discount < 0 || discount > 100)
                ok = InputError("Rabatt", interactive);
            else if (!TryParseDecimal(txtVat.Text, out vat))
                ok = InputError("Steuer", interactive);
            else if (vat < 0 || vat > 100)
            {
                // Steuer außerhalb 0..100 → Fatal, Abbruch der Berechnung
                if (interactive)
                {
                    logger.LogFatal($"Abbruch – ungültiger Steuersatz {vat:0.##}% (erlaubt 0..100).");
                    MessageBox.Show("Ungültiger Steuersatz (0-100)!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                ok = false;
            }

            if (ok)
            {
                config.PriceNetto = price;
                config.Quantity = qty;
                config.DiscountPercent = discount;
                config.VatPercent = vat;
            }
            return ok;
        }

        private static bool TryParseDecimal(string text, out decimal value)
        {
            string s = text.Trim().Replace(',', '.');
            return decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
        }

        private bool InputError(string field, bool interactive)
        {
            if (interactive)
            {
                logger.LogError($"Ungültige Eingabe im Feld '{field}'.");
                MessageBox.Show($"Ungültige Eingabe im Feld '{field}'!", "Eingabefehler", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return false;
        }
    }
}
