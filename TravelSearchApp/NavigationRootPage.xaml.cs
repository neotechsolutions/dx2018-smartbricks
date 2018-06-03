using TravelSearchApp.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using Microsoft.Toolkit.Uwp.Helpers;
using TravelSearchApp.Helpers;

namespace TravelSearchApp
{
    public sealed partial class NavigationRootPage : Page
    {
        bool isNavigating = false;

        public NavigationRootPage()
        {
            this.InitializeComponent();
        }


        public TitleBarHelper TitleHelper
        {
            get
            {
                return TitleBarHelper.Instance;
            }
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(HomePage));

            ContentFrame.Navigated += OnNavigated;

            var currentView = SystemNavigationManager.GetForCurrentView();

            currentView.BackRequested += OnCurrentViewBackRequested; ;
        }

        private void OnCurrentViewBackRequested(object sender, BackRequestedEventArgs e)
        {
            GoBack();
        }

        private void OnNavViewItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                isNavigating = true;
                NavigateTo(typeof(SettingsPage));
            }
            else
            {
                var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
                OnNavViewNavigate(item as NavigationViewItem);
            }
        }

        private void OnNavViewNavigate(NavigationViewItem item)
        {
            switch (item.Tag)
            {
                case "home":
                    NavigateTo(typeof(HomePage));
                    break;

                case "bot":
                    NavigateTo(typeof(BotPage));
                    break;

                case "botdirect":
                    NavigateTo(typeof(BotDirectLinePage));
                    break;
            }
        }

        private void NavigateTo(Type pageType)
        {
            isNavigating = true;
            ContentFrame.Navigate(pageType);
        }

        private bool GoBack()
        {
            if (isNavigating)
            {
                return false;
            }

            bool navigated = false;

            if (ContentFrame.CanGoBack)
            {
                isNavigating = true;
                ContentFrame.GoBack();
                navigated = true;
            }

            return navigated;
        }


        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            OnBackRequested();
            args.Handled = true;
        }

        private bool OnBackRequested()
        {
            bool navigated = false;

            if (NavView.IsPaneOpen && (NavView.DisplayMode == NavigationViewDisplayMode.Compact || NavView.DisplayMode == NavigationViewDisplayMode.Minimal))
            {
                return false;
            }
            else
            {
                navigated = GoBack();
            }
            return navigated;
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (ContentFrame.SourcePageType == typeof(SettingsPage))
            {
                NavView.SelectedItem = NavView.SettingsItem as NavigationViewItem;
            }
            else
            {
                Dictionary<Type, string> lookup = new Dictionary<Type, string>() { { typeof(HomePage), "home" }, { typeof(BotPage), "bot" }, { typeof(BotDirectLinePage), "botdirect" }, { typeof(TravelDetailsPage), "home" } };

                String stringTag = lookup[ContentFrame.SourcePageType];

                foreach (NavigationViewItemBase item in NavView.MenuItems)
                {
                    if (item is NavigationViewItem && item.Tag.Equals(stringTag))
                    {
                        item.IsSelected = true;
                        break;
                    }
                }
            }

            isNavigating = false;

            var ignored = DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                var nav = SystemNavigationManager.GetForCurrentView();
                nav.AppViewBackButtonVisibility = ContentFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            });
        }
    }
}
