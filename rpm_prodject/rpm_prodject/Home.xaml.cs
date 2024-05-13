
using rpm_prodject.images;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rpm_prodject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        private ObservableCollection<Product> products;
        private ObservableCollection<Categories> categories;

        public Home()
        {
            InitializeComponent();
            Fourpoint.Source = ImageSource.FromResource("rpm_prodject.images.Fourpoint.png");
            location.Source = ImageSource.FromResource("rpm_prodject.images.location.png");
            Basket.Source = ImageSource.FromResource("rpm_prodject.images.Basket.png");
            Search.Source = ImageSource.FromResource("rpm_prodject.images.Search.png");
          

            picker.SelectedIndex = 0;
            NavigationPage.SetHasNavigationBar(this, false);
          
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

                    sqlCommand.CommandText = "SELECT * FROM Goods";
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           
                            products.Add(new Product
                            {
                                Id = reader["goods_id"].ToString(),
                                Name = reader["goods_name"].ToString(),
                                Price = reader["goods_price"].ToString(),
                                
                                Image = reader["goods_img_path"].ToString()
                            });
                        }

                        reader.Close();
                    }
                }


                foreach (var product in products)
                {

                    var stackLayout = new StackLayout
                    {
                        BindingContext = product,
                        BackgroundColor = Color.White,
                        WidthRequest = 150

                    };

                    var uri = new Uri(product.Image);
                    var imageButton = new ImageButton
                    {
                        Source = ImageSource.FromUri(uri),
                        Aspect = Aspect.AspectFill,
                        Margin = new Thickness(0, 20, 0, 0),
                        VerticalOptions = LayoutOptions.Start,
                        BackgroundColor = Color.Transparent,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        HeightRequest = 70,
                        WidthRequest = 100,
                        CommandParameter = product
                    };
                    imageButton.Clicked += details_but;

                    var label1 = new Label
                    {
                        Text = "Акция",
                        Margin = new Thickness(20, 0, 20, 0),
                        TextTransform = TextTransform.Uppercase,
                        TextColor = Color.FromHex("#5B9EE1"),
                        FontAttributes = FontAttributes.Italic,
                        FontSize = 12
                    };
                    if(product.Name.Length > 15)
                    {
                        product.Name = TruncateText(product.Name, 15);
                    }
                    var label2 = new Label
                    {
                        
                        Text = product.Name,
                        Margin = new Thickness(20, 0, 20, 0),
                        TextColor = Color.FromHex("#1A2530"),
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 16
                    };

                    var stackLayout2 = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        VerticalOptions = LayoutOptions.EndAndExpand
                    };

                    var label3 = new Label
                    {
                        Text = $"${product.Price}",
                        Margin = new Thickness(20, 0, 20, 0),
                        TextColor = Color.FromHex("#1A2530"),
                        HorizontalOptions = LayoutOptions.Start,
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 16,
                        VerticalTextAlignment = TextAlignment.Center
                    };

                    var imageButton2 = new ImageButton
                    {

                        Source = ImageSource.FromResource("rpm_prodject.images.Pluce.png"),
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        BackgroundColor = Color.Transparent,
                        HeightRequest = 35,
                        WidthRequest = 35
                    };

                    stackLayout2.Children.Add(label3);
                    stackLayout2.Children.Add(imageButton2);

                    stackLayout.Children.Add(imageButton);
                    stackLayout.Children.Add(label1);
                    stackLayout.Children.Add(label2);
                    stackLayout.Children.Add(stackLayout2);

                    scrollGoods.Children.Add(stackLayout);


                }

                categories = new ObservableCollection<Categories>();
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = System.Data.CommandType.Text;

                    sqlCommand.CommandText = "SELECT * FROM Categories";
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new Categories
                            {
                                Id = reader["category_id"].ToString(),
                                Name = reader["category_name"].ToString()
                            });
                        }
                    }
                    foreach (var category in categories)
                    {
                        Button button = new Button
                        {
                            Text = category.Name, 
                            TextColor = Color.White,
                            TextTransform = TextTransform.None,
                            BackgroundColor = Color.FromHex("#5B9EE1"),
                            CornerRadius = 60,
                            HeightRequest = 35,
                            WidthRequest = 170,
                            Padding = new Thickness(15, 0, 15, 0),
                            FontSize = 16,
                            CommandParameter = category
                        };
                     
                        button.Clicked += krossovki_but;

                    
                        categoryStack.Children.Add(button);
                    }

                }
                int i = 0;
                Random random = new Random();
                foreach (var product in products.OrderBy(x => random.Next()))
                {
                    while (i < 2)
                    {

                        Frame frame = new Frame
                        {
                            BackgroundColor = Color.White,
                            CornerRadius = 15,
                            HasShadow = false,
                            WidthRequest = 250
                        };

                        Grid grid = new Grid
                        {
                            ColumnDefinitions =
                            {
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(0.8, GridUnitType.Star) }
                            }
                        };

                        StackLayout stackLayout1 = new StackLayout
                        {
                            
                            
                        };
                        Label lbl1 = new Label
                        {
                            Text = "СПЕЦИАЛЬНО ДЛЯ ВАС",
                            TextColor = Color.FromHex("#5B9EE1"),
                            FontSize = 11,
                            Margin = new Thickness(0, 10, 0, 0)
                        };
                        Label lbl2 = new Label
                        {
                            Text = product.Name,
                            TextColor = Color.FromHex("#1A2530"),
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 16,
                            Margin = new Thickness(0, 10, 0, 0)
                        };
                        Label lbl3 = new Label
                        {
                            Text = $"$ {product.Price}",
                            TextColor = Color.FromHex("#1A2530"),
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 16,
                            Margin = new Thickness(0, 10, 0, 0)
                        };

                        StackLayout stackLayout2_2 = new StackLayout
                        {   
                        };
                       ImageButton imgbtn = new ImageButton
                        {
                            Source = ImageSource.FromUri(new Uri(product.Image)),
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.End,
                            BackgroundColor = Color.Transparent,
                            HeightRequest = 100,
                            WidthRequest = 200,
                            CommandParameter = product
                       };
                        imgbtn.Clicked += details_but;
                        
                        stackLayout2_2.Children.Add(imgbtn);
                        stackLayout1.Children.Add(lbl1);
                        stackLayout1.Children.Add(lbl2);
                        stackLayout1.Children.Add(lbl3);
                        grid.Children.Add(stackLayout1);
                        grid.Children.Add(stackLayout2_2);
                        Grid.SetColumn(stackLayout1, 0);
                        Grid.SetColumn(stackLayout2_2,1);
                        stack_special.Children.Add(frame);
                        frame.Content = grid;
                        break;
                    }
                        i++;

                }
            }


        }

        public void SearchS_but(object sender, System.EventArgs e)
        {
            string sear = SearchS.Text.Trim(); 

            if (string.IsNullOrEmpty(sear) || sear.Length < 1 || sear.Length > 30 || string.IsNullOrWhiteSpace(sear))
            {
                DisplayAlert("Ошибка", "Запрос должен содержать как минимум 1 символ и максимум 30", "OK");
                return;
            }

            if (!IsValidInput(sear))
            {
                DisplayAlert("Ошибка", "Запрос содержит недопустимые символы", "OK");
                return;
            }

 
        }

        private bool IsValidInput(string input)
        {
            
            return input.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c));
        }

        private async void Fourpoint_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SideMenu());
        }
        private async void home_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Home());
        }
        private async void details_but(object sender, EventArgs e)
        {
            var product = (Product)((ImageButton)sender).CommandParameter;
            await Navigation.PushAsync(new Details(product));
        }
        private async void profile_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Profile());
        }
        private async void basket_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MyCart());
        }
        private async void krossovki_but(object sender, System.EventArgs e)
        {
            var cat = (Categories)((Button)sender).CommandParameter;
            await Navigation.PushAsync(new Shablon(cat.Id, cat.Name));
        }
        private async void popul_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Shablon("Популярное"));
        }
        private async void novin_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Shablon("Новинки"));
        }
        private async void uved_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Notifications());
        }
        public static string TruncateText(string text, int maxLength)
        {
            if (text.Length > maxLength)
            {
                return text.Substring(0, maxLength - 3) + "...";
            }
            else
            {
                return text;
            }
        }
        private void OnSearchCompleted(object sender, EventArgs e)
        {
         
            string entered = ((Entry)sender).Text;
            Navigation.PushAsync(new Shablon(entered, 0));
        }
    }
}