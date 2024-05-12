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
    public partial class SignUp : ContentPage
    {
        public SignUp()
        {
            InitializeComponent();
            back.Source = ImageSource.FromResource("rpm_prodject.images.back.png");
            NavigationPage.SetHasNavigationBar(this, false);
        }
        private async void Back(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
        public void Vhod(object sender, System.EventArgs e)
        {
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;
            string name = NameEntry.Text;

            if (name == null)
            {
                DisplayAlert("Ошибка", "Имя должно содержать хотя бы один символ", "OK");
                return;
            }
            if (!Regex.IsMatch(name, "[a-zA-Z]"))
            {
                DisplayAlert("Ошибка", "Имя должено содержать хотя бы одну латинскую букву", "OK");
                return;
            }
            if (!Regex.IsMatch(name, "^[a-zA-Z]+$"))
            {
                DisplayAlert("Ошибка", "Имя не должно содержать цифры или специальные символы", "OK");
                return;
            }

            if (name.Contains(' '))
            {
                DisplayAlert("Ошибка", "Имя не должен содержать пробелы", "OK");
                return;
            }

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
            if (dbManager.EmailExists(email) != false) { 
                dbManager.InsertIntoUsersNotAdmins(name, email, password);
                DbManager.user_login = email;
            }
            else 
            {
                DisplayAlert("Ошибка", "Пользователь уже создан с таким Email", "OK");
                return;
            }


            DisplayAlert("", "Регистрация прошла успешно!", "OK");
            Navigation.PushAsync(new Home());
        }
    }
}