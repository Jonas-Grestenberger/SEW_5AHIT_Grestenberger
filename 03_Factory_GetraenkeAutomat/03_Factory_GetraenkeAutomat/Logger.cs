using System;
using System.IO;
using System.Text;

namespace _03_Factory_GetraenkeAutomat
{
    public sealed class Logger
    {
        private static Logger instance;
        private static readonly object _lock = new object();
        private readonly string fileName;

        private Logger()
        {
            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GetraenkeAutomat_Log.txt");
        }

        public static Logger GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new Logger();
                    }
                }
            }
            return instance;
        }

        public string FileName
        {
            get { return fileName; }
        }

        public void LogStart()
        {
            Log("START", "Applikation gestartet");
        }

        public void LogEnde()
        {
            Log("ENDE", "Applikation beendet");
        }

        public void LogAuswahl(string auswahl, bool gueltig)
        {
            if (gueltig)
                Log("AUSWAHL", "Kunde hat '" + auswahl + "' gewählt");
            else
                Log("AUSWAHL", "Ungültige Eingabe: '" + auswahl + "'");
        }

        public void Log(string kategorie, string nachricht)
        {
            string zeile = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " [" + kategorie + "] " + nachricht;

            lock (_lock)
            {
                try
                {
                    File.AppendAllText(fileName, zeile + Environment.NewLine, Encoding.UTF8);
                }
                catch (IOException)
                {
                    
                }
            }
        }
    }
}
