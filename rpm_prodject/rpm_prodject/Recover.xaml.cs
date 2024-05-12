using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rpm_prodject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Recover : ContentPage
    {
        public Recover()
        {
            InitializeComponent();
            back.Source = ImageSource.FromResource("rpm_prodject.images.back.png");
            NavigationPage.SetHasNavigationBar(this, false);
        }
        public void Vhod(object sender, System.EventArgs e)
        {
            string email = EmailEntry.Text;

            if (email == null || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                DisplayAlert("Ошибка", "Некорректный адрес электронной почты", "OK");
                return;
            }
            if (email.Contains(' '))
            {
                DisplayAlert("Ошибка", "Email не должен содержать пробелы", "OK");
                return;
            }

            DisplayAlert("", "Вы успешно восстановили!", "OK");
            Navigation.PushAsync(new Home());
        }
        private async void Zareg(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SignUp());
        }
        private async void Back(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}