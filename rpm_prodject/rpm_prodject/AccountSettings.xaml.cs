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
	public partial class AccountSettings : ContentPage
	{
		public AccountSettings ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            back.Source = ImageSource.FromResource("rpm_prodject.images.back.png");
            uved11.Source = ImageSource.FromResource("rpm_prodject.images.uved11.png");
            dostav11.Source = ImageSource.FromResource("rpm_prodject.images.dostav11.png");
            koshel11.Source = ImageSource.FromResource("rpm_prodject.images.koshel11.png");
            bin11.Source = ImageSource.FromResource("rpm_prodject.images.bin11.png");
        }
        public void ButtonTheme(object sender, System.EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            if (button.Source.ToString().Contains("Off.png"))
            {
                button.Source = "On.png";
            }
            else
            {
                button.Source = "Off.png";
            }
        }
        private async void Back(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}