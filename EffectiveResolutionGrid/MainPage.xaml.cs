using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641
using EffectiveResolutionGrid.ViewModel;

namespace EffectiveResolutionGrid
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize the item sizes now, and subscribe to orientation changed
            SetGridViewItemSize();
            DisplayInformation.GetForCurrentView().OrientationChanged += OnOrientationChanged;
        }

        private void OnOrientationChanged(DisplayInformation sender, object args)
        {
            SetGridViewItemSize();
        }

        private void SetGridViewItemSize()
        {
            // depending on the orientation, pass the appropriate resource into the Measure method.
            // The databinding will change each items size.

            var viewModel = (DataContext as MainViewModel);

            if (DisplayInformation.GetForCurrentView().CurrentOrientation == DisplayOrientations.Portrait)
            {
                viewModel.Measure((double)Application.Current.Resources["GridViewItemPortrait"]);
            }
            else
            {
                viewModel.Measure((double)Application.Current.Resources["GridViewItemLandscape"]);
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        
    }
}
