using System;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using TaskieLib;

namespace Taskie
{
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer dialogTimer;

        public MainPage()
        {
            InitializeComponent();
            SetupTitleBar();
            SetupNavigationMenu();
            TaskieLib.Tools.ListCreatedEvent += UpdateLists;
        }

        private void SetupTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonHoverBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            Window.Current.SetTitleBar(TTB);
        }

        private void SetupNavigationMenu()
        {
            Navigation.MenuItems.Add(new Microsoft.UI.Xaml.Controls.NavigationViewItem() { Tag = "AddItem", Content = "Add a new list", Icon = new SymbolIcon(Symbol.Add) });
            Navigation.MenuItems.Add(new Microsoft.UI.Xaml.Controls.NavigationViewItemSeparator() { Content = "Your lists" });
            foreach (string listName in TaskieLib.Tools.GetLists())
            {
                Navigation.MenuItems.Add(new Microsoft.UI.Xaml.Controls.NavigationViewItem() { Tag = listName, Content = listName, Icon = new SymbolIcon(Symbol.Document) });
            }
        }

        private void UpdateLists(string name)
        {
            // If the timer is running, stop it and restart it to reset the delay
            if (dialogTimer != null && dialogTimer.IsEnabled)
                dialogTimer.Stop();

            // Create a new timer with a delay of 500 milliseconds (adjust as needed)
            dialogTimer = new DispatcherTimer();
            dialogTimer.Interval = TimeSpan.FromMilliseconds(500);
            dialogTimer.Tick += async (s, e) =>
            {
                dialogTimer.Stop(); // Stop the timer before showing the dialog
                await ShowListCreatedDialog(name);
                Navigation.MenuItems.Add(new Microsoft.UI.Xaml.Controls.NavigationViewItem() { Tag = name, Content = name, Icon = new SymbolIcon(Symbol.Document) });
            };
            dialogTimer.Start(); // Start the timer
        }

        private async Task ShowListCreatedDialog(string listName)
        {
            ContentDialog dialog = new ContentDialog() { Content = $"List created: {listName}", PrimaryButtonText = "OK" };
            await dialog.ShowAsync();
        }


        private async void Navigation_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = args.SelectedItem as Microsoft.UI.Xaml.Controls.NavigationViewItem;
            if (selectedItem != null && selectedItem.Tag is string tag)
            {
                if ((string)selectedItem.Tag == "AddItem")
                {
                    contentFrame.Content = null;
                    sender.SelectedItem = null;
                    await ShowAddItemDialog();
                }
                else if (args.IsSettingsSelected)
                {
                    contentFrame.Navigate(typeof(SettingsPage));
                }
                else
                {
                    contentFrame.Navigate(typeof(TaskPage), tag);
                }
            }
        }

        public string RequestedName = "";

        private async Task ShowAddItemDialog()
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = "Create a list";
            TextBox text = new TextBox();
            text.PlaceholderText = "List name";
            text.TextChanged += Text_TextChanged;
            dialog.Content = text;
            dialog.PrimaryButtonText = "Create";
            dialog.SecondaryButtonText = "Cancel";
            dialog.PrimaryButtonClick += Dialog_PrimaryButtonClick;
            await dialog.ShowAsync();
        }

        private void Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            RequestedName = (sender as TextBox).Text;
        }

        private void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Tools.CreateList(RequestedName);
            RequestedName = null;
        }
    }
}
