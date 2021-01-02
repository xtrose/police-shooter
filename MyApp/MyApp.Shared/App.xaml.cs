using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;





// Namespace
namespace MyApp
{
    




    // US Police Shooter
    public sealed partial class App : Application
    {





        // Nur Windows Phone
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        



        // Wird beim ersten Start der App ausgeführt
        // --------------------------------------------------------
        public App()
        {
            // UI Komponenten laden
            this.InitializeComponent();

            // Was auch immer
            this.Suspending += this.OnSuspending;
        }
        // --------------------------------------------------------





        // Wird beim Start der App ausgeführt
        // --------------------------------------------------------
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

            // Nur Debug
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            // Neues Root Frame erstellen
            Frame rootFrame = Window.Current.Content as Frame;

            // Wenn noch nicht in Root Frame geladen
            if (rootFrame == null)
            {
                // Root Frame neu erstellen
                rootFrame = new Frame();

                // Root Frame Cache einrichten
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Root Frame laden
                Window.Current.Content = rootFrame;
            }

            // Wenn noch nicht in Root Frame geladen
            if (rootFrame.Content == null)
            {

                // Nur Windows Phone
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                // Main Page in Root Frame laden
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Fenster aktivieren
            Window.Current.Activate();
        }
        // --------------------------------------------------------





        // Nur Windows Phone
        // --------------------------------------------------------
#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif
        // --------------------------------------------------------
        





        // Wenn die App in den Hintergrund versetzt wird
        // --------------------------------------------------------
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
        // --------------------------------------------------------





    }
}