using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rpm_prodject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        string srvrdbname = "rpm";
        string srvrname = "192.168.1.96";
        string srvrusername = "samir";
        string srvrpassword = "123456";
        public Profile()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            back.Source = ImageSource.FromResource("rpm_prodject.images.back.png");
            Emil.Source = ImageSource.FromResource("rpm_prodject.images.Emil.png");
            izm9.Source = ImageSource.FromResource("rpm_prodject.images.izm9.png");
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = System.Data.CommandType.Text;

                sqlCommand.CommandText = "select user_name, user_password FROM UsersNotAdmins WHERE login =  (@Login)";
                sqlCommand.Parameters.AddWithValue("@Login", DbManager.user_login);
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NameEntry.Text = reader["user_name"].ToString();
                        MyName.Text = reader["user_name"].ToString();
                        PasswordEntry.Text = reader["user_password"].ToString();
                    }

                }
            }
        }
        private async void Back(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void izm9_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AccountSettings());
        }
        public void Vhod(object sender, System.EventArgs e)
        {

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
            dbManager.UpdateUser(name, password);
            DisplayAlert("", "Вы успешно изменили профиль!", "OK");
      
      
    
            Navigation.PushAsync(new Home());
        }
    }
}