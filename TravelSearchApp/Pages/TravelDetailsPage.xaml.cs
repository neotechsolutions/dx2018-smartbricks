using TravelSearchContracts;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TravelSearchApp.Pages
{
    public sealed partial class TravelDetailsPage : Page
    {
        public TravelDetailsPage()
        {
            this.InitializeComponent();
        }

        private Travel TravelData;


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TravelData = e.Parameter as Travel;
            base.OnNavigatedTo(e);
        }
    }
}
