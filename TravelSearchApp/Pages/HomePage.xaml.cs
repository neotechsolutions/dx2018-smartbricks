using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using TravelSearchApp.Services;
using TravelSearchContracts;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TravelSearchApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private const int MaxElements = 50;

        private ObservableCollection<Travel> _travels;
        private int _persistedItemIndex = -1;

        public HomePage()
        {
            _travels = new ObservableCollection<Travel>();

            this.InitializeComponent();

            LoaderProgressRing.IsActive = true;

            Task.Run(() =>
            {
                return TravelService.GetAllTravelsAsync();
            }).ContinueWith(x =>
            {
                foreach (var travel in x.Result.Take(MaxElements))
                {
                    Travels.Add(travel);
                }

                LoaderProgressRing.IsActive = false;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }


        private void OnListViewItemClick(object sender, ItemClickEventArgs e)
        {
            _persistedItemIndex = TravelGrid.Items.IndexOf(e.ClickedItem);
            Frame.Navigate(typeof(TravelDetailsPage), e.ClickedItem);
        }
        public ObservableCollection<Travel> Travels => _travels;

        #region Animations
        private void OnTravelGridContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            args.ItemContainer.Loaded += OnItemContainerLoaded;
        }

        private void OnItemContainerLoaded(object sender, RoutedEventArgs e)
        {
            var itemsPanel = (ItemsStackPanel)TravelGrid.ItemsPanelRoot;
            var itemContainer = (GridViewItem)sender;

            var itemIndex = TravelGrid.IndexFromContainer(itemContainer);

            var relativeIndex = itemIndex - itemsPanel.FirstVisibleIndex;

            var uc = itemContainer.ContentTemplateRoot as Grid;

            if (itemIndex != _persistedItemIndex && itemIndex >= 0 && itemIndex >= itemsPanel.FirstVisibleIndex && itemIndex <= itemsPanel.LastVisibleIndex)
            {
                var itemVisual = ElementCompositionPreview.GetElementVisual(uc);
                ElementCompositionPreview.SetIsTranslationEnabled(uc, true);

                var easingFunction = Window.Current.Compositor.CreateCubicBezierEasingFunction(new Vector2(0.1f, 0.9f), new Vector2(0.2f, 1f));

                // Create KeyFrameAnimations
                var offsetAnimation = Window.Current.Compositor.CreateScalarKeyFrameAnimation();
                offsetAnimation.InsertKeyFrame(0f, 100);
                offsetAnimation.InsertKeyFrame(1f, 0, easingFunction);
                offsetAnimation.Target = "Translation.X";
                offsetAnimation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;
                offsetAnimation.Duration = TimeSpan.FromMilliseconds(700);
                offsetAnimation.DelayTime = TimeSpan.FromMilliseconds(relativeIndex * 100);

                var fadeAnimation = Window.Current.Compositor.CreateScalarKeyFrameAnimation();
                fadeAnimation.InsertExpressionKeyFrame(0f, "0");
                fadeAnimation.InsertExpressionKeyFrame(1f, "1");
                fadeAnimation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;
                fadeAnimation.Duration = TimeSpan.FromMilliseconds(700);
                fadeAnimation.DelayTime = TimeSpan.FromMilliseconds(relativeIndex * 100);

                // Start animations
                itemVisual.StartAnimation("Translation.X", offsetAnimation);
                itemVisual.StartAnimation("Opacity", fadeAnimation);
            }
            else
            {
                Debug.WriteLine("Skipping");
            }

            itemContainer.Loaded -= this.OnItemContainerLoaded;
        }
        #endregion
    }
}
