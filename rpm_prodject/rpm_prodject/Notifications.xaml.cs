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
	public partial class Notifications : ContentPage
	{
		public Notifications ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            Kross1.Source = ImageSource.FromResource("rpm_prodject.images.Kross1.png");
            Kross2.Source = ImageSource.FromResource("rpm_prodject.images.Kross2.png");
            Kross44.Source = ImageSource.FromResource("rpm_prodject.images.Kross44.png");
            Kross4.Source = ImageSource.FromResource("rpm_prodject.images.Kross4.png");
            if (IsClearedP.alreadyViewed == true) { time1.Text = (IsClearedP.date); time2.Text = (IsClearedP.date); btn_blue1.IsVisible = false; btn_blue2.IsVisible = false; }
            IsClearedP.alreadyViewed = true;
            if(IsClearedP.check == true)
            {
                lay1.IsVisible = false;
                lay2.IsVisible = false;
            }
        }
        private async void Back(object sender, System.EventArgs e)
        {
            string dateTime = DateTime.Now.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            IsClearedP.date = dateTime ;
            await Navigation.PopAsync();
        }
        private async void details_but(object sender, EventArgs e)
        {

        }

        private async void clear_all(object sender, EventArgs e)
        {
            lay1.IsVisible = false;
            lay2.IsVisible = false;
            IsClearedP.SetTrue();

        }
    }

    public class IsClearedP
    {
        public static bool check = false;
        public static string date;
        public static bool alreadyViewed = false;
        public static void SetTrue()
        {
            check = true;
        }



    }
}