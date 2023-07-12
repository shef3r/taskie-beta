using System;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI;

namespace Taskie
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            CustomizeTitleBar();
            contentFrame.Navigate(typeof(AddItemPage));
            TaskieLib.Tools.ListCreatedEvent += UpdateLists;
            InitializeNavigationMenu();
            Navigation.SelectedItem = Navigation.MenuItems[0];
        }

        private void CustomizeTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonHoverBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            Window.Current.SetTitleBar(TTB);
        }

        private void InitializeNavigationMenu()
        {
            Navigation.MenuItems.Add(new Microsoft.UI.Xaml.Controls.NavigationViewItem() { Tag = "AddItem", Content = "Add a new list", Icon = new SymbolIcon() { Symbol = Symbol.Add } });
            Navigation.MenuItems.Add(new Microsoft.UI.Xaml.Controls.NavigationViewItemSeparator() { Content = "Your lists" });
            foreach (string list in TaskieLib.Tools.GetLists())
            {
                Navigation.MenuItems.Add(new Microsoft.UI.Xaml.Controls.NavigationViewItem() { Tag = list, Content = list, Icon = new SymbolIcon() { Symbol = Symbol.Document } });
            }
        }

        private async void UpdateLists(string name)
        {
            await new ContentDialog() { Content = $"List created: {name}", PrimaryButtonText = "OK" }.ShowAsync();
            try
            {
                Navigation.MenuItems.Add(new Microsoft.UI.Xaml.Controls.NavigationViewItem() { Tag = name, Content = name, Icon = new SymbolIcon() { Symbol = Symbol.Document } });
            }
            catch (Exception ex)
            {
                // Display a user-friendly error message or log the exception details
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Log the entire exception stack trace
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void Navigation_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            string tag = args.SelectedItemContainer.Tag.ToString();
            if (args.IsSettingsSelected)
            {
                contentFrame.Navigate(typeof(SettingsPage));
            }
            else if (tag == "AddItem")
            {
                contentFrame.Navigate(typeof(AddItemPage));
            }
            else
            {
                contentFrame.Navigate(typeof(TaskPage), tag);
            }
        }
    }
}
