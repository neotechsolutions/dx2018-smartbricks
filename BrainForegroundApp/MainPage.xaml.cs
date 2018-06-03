using BrainLib;
using Lego.Ev3.Core;
using Lego.Ev3.Uwp;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BrainForegroundApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly CancellationTokenSource _tokenSource;
        private readonly DeviceListener _listener;

        public MainPage()
        {
            InitializeComponent();

            _tokenSource = new CancellationTokenSource();
            _listener = new DeviceListener(
                brick: new Brick(new CommunicationFactory(), new FileProvider()),
                notify: NotifyMethodAsync
            );
        }

        private void ConnectClick(object sender, RoutedEventArgs e)
        {
            Task.Run(async () => await _listener.ConnectAsync(_tokenSource.Token));
        }

        private async void InitializeClick(object sender, RoutedEventArgs e)
        {
            await _listener.InitializeAsync();
        }

        private async void LoadClick(object sender, RoutedEventArgs e)
        {
            await _listener.LoadBagAsync();
        }

        private async void CloseClick(object sender, RoutedEventArgs e)
        {
            await _listener.CloseAsync();
        }

        private Task NotifyMethodAsync(string method, string msg)
        {
            return Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
            {
#if DEBUG
                Debug.WriteLine($"[DEBUG] {method} : {msg}");
#endif
                MessageText.Text = $"[DEBUG] {method} : {msg}";
            })).AsTask();
        }
    }
}
