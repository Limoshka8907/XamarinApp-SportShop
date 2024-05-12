using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rpm_prodject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Details : ContentPage
    {
        private ObservableCollection<Product> products;
       

        Product _product;
        string Id_g = "";
        string cat_id = "";

        public Details(Product __product)
        {
            InitializeComponent();
            LoadFavourites();
            _product = __product;
            
            NavigationPage.SetHasNavigationBar(this, false);
            back.Source = ImageSource.FromResource("rpm_prodject.images.back.png");
            Basket.Source = ImageSource.FromResource("rpm_prodject.images.Basket.png");


            products = new ObservableCollection<Product>();

            string srvrdbname = "rpm";
            string srvrname = "192.168.1.96";
            string srvrusername = "samir";
            string srvrpassword = "123456";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";



            using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = System.Data.CommandType.Text;

                    sqlCommand.CommandText = "SELECT * FROM Goods WHERE goods_id = @Id";
                    sqlCommand.Parameters.AddWithValue("@Id", __product.Id);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Id_g = reader["goods_id"].ToString();
                            string Name = reader["goods_name"].ToString();
                            string Price = reader["goods_price"].ToString();
                            string Image = reader["goods_img_path"].ToString();
                            string Description = reader["goods_description"].ToString();
                            cat_id = reader["goods_category_id"].ToString();

                            nameEnter.Text = Name;
                            descriptionEnter.Text = Description;
                            big_kross7.Source = ImageSource.FromUri(new Uri(Image));
                            big_kross7.Aspect = Aspect.AspectFill;
                            priceEnter.Text = $"$ {Price}";
                            price_Enter.Text = $"$ {Price}";
                        }
                        sizeStack.IsVisible = true;
                        colorsStack.IsVisible = true;
                        colorLabel.IsVisible =true;
                        sizeLabel.IsVisible = true;
                        sizeStandart.IsVisible = true;
                        if (cat_id != "1" && cat_id != "5")
                        {
                            sizeStack.IsVisible = false;
                            colorsStack.IsVisible = false;
                            colorLabel.IsVisible = false;
                            sizeLabel.IsVisible = false;
                            sizeStandart.IsVisible = false;
                            if (cat_id == "2")
                            {
                                StackLayout sizeStack2 = new StackLayout
                                {
                                    Orientation = StackOrientation.Horizontal,
                                    Spacing = 10,
                                    Margin = new Thickness(0, 0, 0, 50),
                                };
                                        var btn1 = new Button
                                        {
                                            Text = "S",
                                            TextColor = Color.FromHex("#707B81"),
                                            FontFamily = "Roboto",
                                            FontAttributes = FontAttributes.Italic,
                                            FontSize = 18,
                                            BackgroundColor = Color.FromHex("#F8F9FA"),
                                            CornerRadius = 100,
                                            WidthRequest = 50
                                        };
                                        var btn2 = new Button
                                        {
                                            Text = "M",
                                            TextColor = Color.FromHex("#707B81"),
                                            FontFamily = "Roboto",
                                            FontAttributes = FontAttributes.Italic,
                                            FontSize = 18,
                                            BackgroundColor = Color.FromHex("#F8F9FA"),
                                            CornerRadius = 100,
                                            WidthRequest = 50
                                        };
                                        var btn3 = new Button
                                        {
                                            Text = "L",
                                            TextColor = Color.FromHex("#707B81"),
                                            FontFamily = "Roboto",
                                            FontAttributes = FontAttributes.Italic,
                                            FontSize = 18,
                                            BackgroundColor = Color.FromHex("#F8F9FA"),
                                            CornerRadius = 100,
                                            WidthRequest = 50
                                        };
                                        var btn4 = new Button
                                        {
                                            Text = "XL",
                                            TextColor = Color.FromHex("#707B81"),
                                            FontFamily = "Roboto",
                                            FontAttributes = FontAttributes.Italic,
                                            FontSize = 18,
                                            BackgroundColor = Color.FromHex("#F8F9FA"),
                                            CornerRadius = 100,
                                            WidthRequest = 50
                                        };
                                        sizeStack2.Children.Add(btn1);
                                        sizeStack2.Children.Add(btn2);   
                                        sizeStack2.Children.Add(btn3);
                                        sizeStack2.Children.Add(btn4);
                                        forSizeStack.Content = sizeStack2;
                                        btn1.Clicked += btn_blur;
                                        btn2.Clicked += btn_blur;
                                        btn3.Clicked += btn_blur;
                                        btn4.Clicked += btn_blur;
                            }
                            
                        }
                        reader.Close();
                    }


                }

            }
        }

        private Button lastSelectedSizeButton = null; 

        private void btn_blur(object sender, EventArgs e)
        {
            Button currentButton = (Button)sender;

 
            if (currentButton == lastSelectedSizeButton)
            {
                currentButton.BackgroundColor = Color.Transparent;
                lastSelectedSizeButton = null;
            }
            else
            {

                if (lastSelectedSizeButton != null)
                {
                    lastSelectedSizeButton.BackgroundColor = Color.Transparent;
                }


                currentButton.BackgroundColor = Color.FromHex("#B2A7A7A7");
                lastSelectedSizeButton = currentButton;

        
            }
        }
        List<Button> buttons_color = new List<Button>();
        List<Color> colors = new List<Color>();
        private Button lastSelectedButton = null; 
        private Color lastSelectedColor; 

        private void btn_color_blur(object sender, EventArgs args)
        {
            Button currentButton = (Button)sender;
            Color currentColor = currentButton.BackgroundColor;


            if (currentButton == lastSelectedButton)
            {
                currentButton.BackgroundColor = lastSelectedColor;
                lastSelectedButton = null;
            }
            else
            {
       
                if (lastSelectedButton != null)
                {
                    lastSelectedButton.BackgroundColor = lastSelectedColor;
                }

          
                lastSelectedButton = currentButton;
                lastSelectedColor = currentColor;

       
                currentButton.BackgroundColor = Color.FromHex("A2A7A7A7");

               
            }

        }

        private async void Back(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void basket_but(object sender, System.EventArgs e)
        {

            if (Busket._products.Any(p => p.Id == Id_g))
            {
      
                await DisplayAlert("Ошибка", "Такой товар уже добавлен.", "Ok");
            }
            else
            {
                if (_product != null && lastSelectedButton != null && lastSelectedSizeButton != null && lastSelectedColor != null)
                {
                    Busket._products.Add(_product);
                    Busket.color.Add(lastSelectedColor);
                    Busket.size.Add(lastSelectedSizeButton.Text);
                    await Navigation.PushAsync(new MyCart());
                }
                else if(_product != null && (cat_id != "1" || cat_id != "5"))
                {
                    
                    Busket._products.Add(_product);
                    Busket.color.Add(Color.Transparent);
                    Busket.size.Add(null);
                    await Navigation.PushAsync(new MyCart());
                }
                else
                {
                    await DisplayAlert("Уведомление", "Ошибка, обратите внимание и убедитесь что все параметры выбраны", "ОК");
                }
            }
           
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
        private void LoadFavourites()
        {

            var settings = Application.Current.Properties;


            if (settings.ContainsKey("FavouritesList"))
            {
                Favourites.FavouritesList = (List<Product>)settings["FavouritesList"];
            }
        }

        private void SaveFavourites()
        {
    
            var settings = Application.Current.Properties;


            settings["FavouritesList"] = Favourites.FavouritesList;
        }

        private void add_favourite(object sender, EventArgs e)
        {

            Favourites.FavouritesList.Add(_product);


            SaveFavourites();
        }

    }

    public class Busket
    {
        public static Dictionary<string, int> productQuantities = new Dictionary<string, int>();
        public static List<Product> _products = new List<Product>();
        public static List<string> size = new List<string>();
        public static List<Color> color = new List<Color>();
        public static float cost = 0;
    }

}