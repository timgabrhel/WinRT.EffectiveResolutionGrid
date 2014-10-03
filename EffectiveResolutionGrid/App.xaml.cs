using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace EffectiveResolutionGrid
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
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

                // set the grid view item sizing resources
                InitializeGridResources();

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        public static void InitializeGridResources()
        {
            // set the pages content to underlay the task bar
            ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
            
            // take the width of the screen and subtract off the total outer margin of the grid (5 + 5)
            // on higher resolution devices that are calculated by scaling, the long decimal values
            // end up creating tiles too large to fit. So we round down to the whole number. 
            // Technically, there is less than 1px of inaccuracy for the full width.
            var width = Math.Truncate((Window.Current.Bounds.Width - 10));
            var height = Math.Truncate((Window.Current.Bounds.Height - 10));
            
            // total left + right margin for each tile (ItemContainerStyle)
            var margin = 10;

            var twoColumnGridPortrait = (width / 2) - margin;
            var threeColumnGridPortrait = (width / 3) - margin;
            
            var twoColumnGridLandscape = (height / 2) - margin;
            var threeColumnGridLandscape = (height / 3) - margin;
            var fourColumnGridLandscape = (height / 4) - margin;
            var sixColumnGridLandscape = (height / 6) - margin;

            double portrait;
            double landscape;

            // based on the effective resolution, set our desired item sizes
            if (Math.Abs(Window.Current.Bounds.Height - 873) < 1 &&
                Math.Abs(Window.Current.Bounds.Width - 491) < 1)
            {
                // 1080P 6 inch

                portrait = threeColumnGridPortrait;
                landscape = sixColumnGridLandscape;
            }
            else if (Math.Abs(Window.Current.Bounds.Height - 800) < 1 &&
                     Math.Abs(Window.Current.Bounds.Width - 450) < 1)
            {
                // 1080P 5.5 inch

                portrait = threeColumnGridPortrait;
                landscape = sixColumnGridLandscape;
            }
            else if (Math.Abs(Window.Current.Bounds.Height - 711) < 1 &&
                     Math.Abs(Window.Current.Bounds.Width - 400) < 1)
            {
                // 720p 4.7 inch

                portrait = twoColumnGridPortrait;
                landscape = fourColumnGridLandscape;

            }

            else if (Math.Abs(Window.Current.Bounds.Height - 640) < 1 &&
                     Math.Abs(Window.Current.Bounds.Width - 384) < 1)
            {
                // WXGA 4.5 inch

                portrait = twoColumnGridPortrait;
                landscape = threeColumnGridLandscape;
            }
            else
            {
                // WVGA 4 inch 400x666

                portrait = twoColumnGridPortrait;
                landscape = twoColumnGridLandscape;
            }

            // store the set sizes in application resources
            Application.Current.Resources["GridViewItemPortrait"] = Math.Round(portrait, 2);
            Application.Current.Resources["GridViewItemLandscape"] = Math.Round(landscape, 2);
        }

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

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}