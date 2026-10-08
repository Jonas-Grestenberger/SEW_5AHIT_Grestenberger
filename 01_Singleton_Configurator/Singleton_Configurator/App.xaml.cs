using System.Windows;
using Singleton_Configurator.Services;

namespace Singleton_Configurator
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Logger.GetInstance().LogInfo("App gestartet.");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Logger.GetInstance().LogInfo("App beendet.");
            base.OnExit(e);
        }
    }
}
