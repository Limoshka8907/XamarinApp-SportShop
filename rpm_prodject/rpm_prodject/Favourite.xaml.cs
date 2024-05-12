using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rpm_prodject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Favourite : ContentPage
    {
        public Favourite()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            hearth99.Source = ImageSource.FromResource("rpm_prodject.images.hearth99.png");
            back.Source = ImageSource.FromResource("rpm_prodject.images.back.png");

            try
            {
                int i = 0;

                foreach (var favourite in Favourites.FavouritesList)
                {
                    StackLayout stackLayout = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Margin = new Thickness(0, 20, 0, 0),
                        Spacing = 20,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Start
                    };

                    var frame1 = new Frame
                    {
                        BackgroundColor = Color.White,
                        CornerRadius = 20,
                        HasShadow = false,
                        Padding = 10,
                        WidthRequest = 165,
                        HeightRequest = 225,
                        Content = new StackLayout
                        {
                            Children = {
                        new ImageButton {
                            Source = ImageSource.FromUri(new Uri((Favourites.FavouritesList[i].Image))),
                            BackgroundColor = Color.Transparent,
                            HeightRequest = 120,
                            WidthRequest = 145,
                            VerticalOptions = LayoutOptions.Start,
                            HorizontalOptions = LayoutOptions.Center,
                        },
                        new Label {
                          Text = "Акция",
                            TextTransform = TextTransform.Uppercase,
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            TextColor = Color.FromHex("#5B9EE1"),
                            FontFamily = "Roboto",
                            FontSize = 14,
                            FontAttributes = FontAttributes.Italic
                        },
                        new Label {
                          Text = (Favourites.FavouritesList[i].Name),
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            TextColor = Color.Black,
                            FontFamily = "Roboto",
                            FontSize = 20,
                            FontAttributes = FontAttributes.Bold
                        },
                        new StackLayout {
                          Orientation = StackOrientation.Horizontal,
                            VerticalOptions = LayoutOptions.EndAndExpand,
                            Children = {
                              new Label {
                                Text = $"${(Favourites.FavouritesList[i].Price)}",
                                  HorizontalOptions = LayoutOptions.Start,
                                  VerticalOptions = LayoutOptions.Center,
                                  TextColor = Color.Black,
                                  FontFamily = "Roboto",
                                  FontSize = 20,
                                  FontAttributes = FontAttributes.Bold
                              },
                              new StackLayout {
                                Orientation = StackOrientation.Horizontal,
                                  HorizontalOptions = LayoutOptions.EndAndExpand,
                                  VerticalOptions = LayoutOptions.Center
                              }
                            }
                        }
                      }
                        }
                    };



         
                    mainLayour.Children.Add(frame1);
                    i++;
                }
            }
            catch
            {

            }
        }
        private async void Back(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void details_but(object sender, EventArgs e)
        {
            var product = (Product)sender;
            await Navigation.PushAsync(new Details(product));
        }
        private async void uved_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Notifications());
        }
        private async void home_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Home());
        }
        private async void profile_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Profile());
        }
        private async void basket_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MyCart());
        }
    }
}