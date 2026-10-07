using System.ComponentModel;
using System.Globalization;
using System.Windows;
using Singleton_Configurator.Services;

namespace Singleton_Configurator
{
    public partial class MainWindow : Window
    {
        private readonly Configurator config = Configurator.GetInstance();

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
            if (!TryReadInputs(out decimal price, out int qty, out decimal discount, out decimal vat))
                return;

            decimal netto = price * qty * (1 - discount / 100m);
            decimal tax = netto * vat / 100m;
            decimal brutto = netto + tax;

            lblNetto.Text = netto.ToString("N2");
            lblVat.Text = tax.ToString("N2");
            lblBrutto.Text = brutto.ToString("N2");

            // nach jeder Berechnung speichern
            config.Save();
        }

        // Beim Schließen: Werte speichern (sofern gültig)
        private void Window_Closing(object? sender, CancelEventArgs e)
        {
            TryReadInputs(out _, out _, out _, out _, showErrors: false);
            config.Save();
        }

        /// <summary>Liest die Eingaben (akzeptiert "10,00" und "10.00") und überträgt sie in den Configurator.</summary>
        private bool TryReadInputs(out decimal price, out int qty, out decimal discount, out decimal vat, bool showErrors = true)
        {
            qty = 0; discount = 0; vat = 0;
            bool ok = true;

            if (!TryParseDecimal(txtPrice.Text, out price) || price < 0)
                ok = Fail("Ungültiger Preis!", showErrors);
            else if (!int.TryParse(txtQuantity.Text.Trim(), out qty) || qty < 0)
                ok = Fail("Ungültige Menge!", showErrors);
            else if (!TryParseDecimal(txtDiscount.Text, out discount) || discount < 0 || discount > 100)
                ok = Fail("Ungültiger Rabatt (0-100)!", showErrors);
            else if (!TryParseDecimal(txtVat.Text, out vat) || vat < 0 || vat > 100)
                ok = Fail("Ungültige Steuer (0-100)!", showErrors);

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

        private static bool Fail(string message, bool show)
        {
            if (show)
                MessageBox.Show(message, "Eingabefehler", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }
    }
}
