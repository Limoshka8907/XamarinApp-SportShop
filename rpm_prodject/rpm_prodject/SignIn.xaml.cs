

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rpm_prodject
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignIn : ContentPage
	{
		public SignIn ()
		{
			InitializeComponent ();
            back.Source = ImageSource.FromResource("rpm_prodject.images.back.png");
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void Zareg(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SignUp());
        }
        private async void Zabpass(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Recover());
        }
        private async void Back(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
        public void Vhod(object sender, System.EventArgs e)
        {
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;

            if (email == null || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                DisplayAlert("Ошибка", "Некорректный адрес электронной почты", "OK");
                return;
            }

            if (password == null || password.Length < 8 || password.Length > 15 || string.IsNullOrWhiteSpace(password))
            {
                DisplayAlert("Ошибка", "Пароль должен содержать как минимум 8 символов и максимум 15", "OK");
                return;
            }

            if (!Regex.IsMatch(password, "[a-zA-Z]"))
            {
                DisplayAlert("Ошибка", "Пароль должен содержать хотя бы одну букву", "OK");
                return;
            }

            if (!Regex.IsMatch(password, "[0-9]"))
            {
                DisplayAlert("Ошибка", "Пароль должен содержать хотя бы одну цифру", "OK");
                return;
            }

            if (password.Contains(' '))
            {
                DisplayAlert("Ошибка", "Пароль не должен содержать пробелы", "OK");
                return;
            }

            DbManager dbManager = new DbManager();
            DbManager.user_login = email;
            if(dbManager.SelectUsersNotAdmins(email, password) == false)
            {
                DisplayAlert("Ошибка", "Пользователь не найден", "OK");
                return;
            }
            
            DbManager.user_login = email;
            DisplayAlert("", "Вы успешно вошли в систему!", "OK");
            Navigation.PushAsync(new Home());
            //Navigation.PushAsync(new AccountSettings());
        }
    }
}