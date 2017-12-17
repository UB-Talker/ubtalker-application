using System.Windows;
using EyeXFramework.Wpf;

namespace UBTalker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly WpfEyeXHost _eyeXHost;

        public WpfEyeXHost EyeXHost
        {
            get { return _eyeXHost; }
        }

        public App()
        {
            Autocomplete.WordPredictor.Test();
            _eyeXHost = new WpfEyeXHost();
            _eyeXHost.Start();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _eyeXHost.Dispose();
        }
    }
}
