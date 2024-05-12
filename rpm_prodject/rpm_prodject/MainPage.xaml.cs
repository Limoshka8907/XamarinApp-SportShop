using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace rpm_prodject
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            image_first_screen.Source = ImageSource.FromResource("rpm_prodject.images.image_first_screen.png");
            image_first_screen.Aspect = Aspect.Fill;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void But1_first_screen(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SignIn());
        }
    }
}
