using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03_Factory_GetraenkeAutomat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logger logger = Logger.GetInstance();
            logger.LogStart();

            Console.WriteLine("Willkomen beim Getraenkeautomaten!");
            Console.WriteLine("Mögliche Getränke: Kaffe, Limonade, Wasser, Tee, Energydrink, Saft\nGib 'ende' ein, um das Programm zu beenden");
            Console.WriteLine("\n");
            string input = "";

            KaffeeAutomat ka = new KaffeeAutomat();
            LimonadeAutomat li = new LimonadeAutomat();
            WasserAutomat wa = new WasserAutomat();
            TeeAutomat te = new TeeAutomat();
            EnergydrinkAutomat en = new EnergydrinkAutomat();
            SaftAutomat sa = new SaftAutomat();
            while (true) 
            {
                Console.WriteLine("Bitte wähle dein Getränk: ");
                input = Console.ReadLine();
                
                bool gueltig = true;
                switch (input)
                {
                    case "Kaffe":
                        ka.ErzeugeGetraenk().Ausgabe();
                        break;
                    case "Limonade":
                        li.ErzeugeGetraenk().Ausgabe();
                        break;
                    case "Wasser":
                        wa.ErzeugeGetraenk().Ausgabe();
                        break;
                    case "Tee":
                        te.ErzeugeGetraenk().Ausgabe();
                        break;
                    case "Energydrink":
                        en.ErzeugeGetraenk().Ausgabe();
                        break;
                    case "Saft":
                        sa.ErzeugeGetraenk().Ausgabe();
                        break;
                    case "ende":
                        break;
                    default:
                        gueltig = false;
                        Console.WriteLine("Ungültige Eingabe, bitte ereut versuchen");
                        break;
                }

                if (input == "ende")
                    break;

                logger.LogAuswahl(input, gueltig);
            }

            logger.LogEnde();
        }
    }
    public interface IGetraenk
    {
        void Ausgabe();
    }
    public class Kaffee : IGetraenk
    {
        public void Ausgabe()
        {
            Console.WriteLine("Ein Kaffe wurde ausgegeben");
        }
    }
    public class Limonade:IGetraenk
    {
        public void Ausgabe() {
            Console.WriteLine("Eine Limonade wurde ausgegeben");
        }
    }
    public class Wasser:IGetraenk
    {
        public void Ausgabe()
        {
            Console.WriteLine("Ein Wasser wurde ausgegeben");
        }
    }
    public class Tee : IGetraenk
    {
        public void Ausgabe()
        {
            Console.WriteLine("Ein Tee wurde ausgegeben");
        }
    }
    public class Energydrink:IGetraenk
    {
        public void Ausgabe()
        {
            Console.WriteLine("Eine Energydrink wurde ausgegeben");
        }
    }
    public class Saft:IGetraenk
    {
        public void Ausgabe()
        {
            Console.WriteLine("Ein Saft wurde ausgegeben");
        }
    }

    public abstract class Automat
    {
        public abstract IGetraenk ErzeugeGetraenk();
        public abstract void Bedienen();
    }
    public class KaffeeAutomat : Automat
    {
        public override IGetraenk ErzeugeGetraenk()
        {
            return new Kaffee();
        }
        public override void Bedienen() 
        { 
            ErzeugeGetraenk().Ausgabe();
        }

    }
    public class LimonadeAutomat : Automat
    {
        public override IGetraenk ErzeugeGetraenk()
        {
            return new Limonade();
        }
        public override void Bedienen()
        {
            ErzeugeGetraenk().Ausgabe();
        }
    }

    public class WasserAutomat : Automat
    {
        public override IGetraenk ErzeugeGetraenk()
        {
            return new Wasser();
        }
        public override void Bedienen()
        {
            ErzeugeGetraenk().Ausgabe();
        }
    }

    public class TeeAutomat : Automat
    {
        public override IGetraenk ErzeugeGetraenk()
        {
            return new Tee();
        }
        public override void Bedienen()
        {
            ErzeugeGetraenk().Ausgabe();
        }
    }
    public class EnergydrinkAutomat : Automat
    {
        public override IGetraenk ErzeugeGetraenk()
        {
            return new Energydrink();
        }
        public override void Bedienen()
        {
            ErzeugeGetraenk().Ausgabe();
        }
    }
    public class SaftAutomat : Automat
    {
        public override IGetraenk ErzeugeGetraenk()
        {
            return new Saft();
        }
        public override void Bedienen()
        {
            ErzeugeGetraenk().Ausgabe();
        }
    }
}
