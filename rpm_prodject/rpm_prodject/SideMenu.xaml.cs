using rpm_prodject.images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rpm_prodject
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SideMenu : ContentPage
	{
		public SideMenu ()
		{
			InitializeComponent ();
            Emil.Source = ImageSource.FromResource("rpm_prodject.images.Emil.png");
            prof6.Source = ImageSource.FromResource("rpm_prodject.images.prof6.png");
            home6.Source = ImageSource.FromResource("rpm_prodject.images.home6.png");
            basket6.Source = ImageSource.FromResource("rpm_prodject.images.basket6.png");
            izbr6.Source = ImageSource.FromResource("rpm_prodject.images.izbr6.png");
            zakaz6.Source = ImageSource.FromResource("rpm_prodject.images.zakaz6.png");
            uved6.Source = ImageSource.FromResource("rpm_prodject.images.uved6.png");
            viti6.Source = ImageSource.FromResource("rpm_prodject.images.viti6.png");
            line6.Source = ImageSource.FromResource("rpm_prodject.images.line6.png");
            NavigationPage.SetHasNavigationBar(this, false);
            DbManager db = new DbManager();
            db.SelectUserName();
            nameEntry.Text = DbManager.user_name;
        }
        private async void viti_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
        private async void home6_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Home());
        }
        private async void basket_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MyCart());
        }
        private async void profile_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Profile());
        }
        private async void izbr_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Favourite());
        }
        private async void zakaz_but(object sender, System.EventArgs e)
        {

            await Navigation.PushAsync(new Shablon("Заказы"));
        }
        private async void uved_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Notifications());
        }
    }
}