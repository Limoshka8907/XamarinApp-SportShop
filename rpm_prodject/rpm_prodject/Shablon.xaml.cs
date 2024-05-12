using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FFImageLoading;
namespace rpm_prodject.images
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Shablon : ContentPage
    {
        string srvrdbname = "rpm";
        string srvrname = "192.168.1.96";
        string srvrusername = "samir";
        string srvrpassword = "123456";
        List<string> quantity = new List<string>();
        List<string> colors = new List<string>();
        List<string> sizes = new List<string>();
        List<string> cat_id = new List<string>();
        public Shablon(string pageName)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            InitializeComponent();
            izbrannoe.Text = pageName;
            if (pageName == "Популярное" || pageName == "Новинки")
            {
                try
                {
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
                                int i = 0;
                                List<StackLayout> list = new List<StackLayout>();
                                List<Product> products = new List<Product>();
                                while (reader.Read())
                                {
                                    products.Add(new Product
                                    {
                                        Id = reader["goods_id"].ToString(),
                                        Name = reader["goods_name"].ToString(),
                                        Price = reader["goods_price"].ToString(),
                                        //Image = "rpm_prodject.images.RedKross.png"
                                        Image = reader["goods_img_path"].ToString()
                                    });
                                }
                                products = products.OrderBy(x => Guid.NewGuid()).ToList();
                                Random rnd = new Random();
                                int removeCount = rnd.Next(1, 4); // Количество элементов для удаления
                                products.RemoveRange(products.Count - removeCount, removeCount);

                                foreach (var product in products)
                                {
                                    if (product.Name.Length > 15)
                                    {
                                        product.Name = Home.TruncateText(product.Name, 15);
                                    }
                                    ImageButton img = new ImageButton()
                                    {
                                        Source = ImageSource.FromUri(new Uri(product.Image)),
                                        BackgroundColor = Color.Transparent,
                                        HeightRequest = 120,
                                        WidthRequest = 145,
                                        VerticalOptions = LayoutOptions.Start,
                                        HorizontalOptions = LayoutOptions.Center,
                                        CommandParameter = product,
                                    };


                                 
                                    

                                    StackLayout stackLayout = new StackLayout
                                    {
                                        Orientation = StackOrientation.Horizontal,
                                        Margin = new Thickness(0, 20, 0, 0),
                                        Spacing = 20,
                                        HorizontalOptions = LayoutOptions.Center,
                                        VerticalOptions = LayoutOptions.Start
                                    };
                                    list.Add(stackLayout);
                                
                                    var frame1 = new Frame
                                    {
                                        BackgroundColor = Color.White,
                                        CornerRadius = 20,
                                        HasShadow = false,
                                        Padding = 10,
                                        WidthRequest = 165,
                                        HeightRequest = 225,
                                        Content = new StackLayout
                                        {
                                            Children = {
                                                                        img,

                                                new Label {
                                                  Text = "Акция",
                                                    TextTransform = TextTransform.Uppercase,
                                                    HorizontalOptions = LayoutOptions.Start,
                                                    VerticalOptions = LayoutOptions.Start,
                                                    TextColor = Color.FromHex("#5B9EE1"),
                                                    FontFamily = "Roboto",
                                                    FontSize = 14,
                                                    FontAttributes = FontAttributes.Italic
                                                },
                                                new Label {
                                                  Text = (product.Name),
                                                    HorizontalOptions = LayoutOptions.Start,
                                                    VerticalOptions = LayoutOptions.Start,
                                                    TextColor = Color.Black,
                                                    FontFamily = "Roboto",
                                                    FontSize = 20,
                                                    FontAttributes = FontAttributes.Bold
                                                },
                                                new StackLayout {
                                                  Orientation = StackOrientation.Horizontal,
                                                    VerticalOptions = LayoutOptions.EndAndExpand,
                                                    Children = {
                                                      new Label {
                                                        Text = $"${(product.Price)}",
                                                          HorizontalOptions = LayoutOptions.Start,
                                                          VerticalOptions = LayoutOptions.Center,
                                                          TextColor = Color.Black,
                                                          FontFamily = "Roboto",
                                                          FontSize = 20,
                                                          FontAttributes = FontAttributes.Bold
                                                      },
                                                      new StackLayout {
                                                        Orientation = StackOrientation.Horizontal,
                                                          HorizontalOptions = LayoutOptions.EndAndExpand,
                                                          VerticalOptions = LayoutOptions.Center,
                                                          Children = {
                                                            new Button {
                                                              BackgroundColor = Color.FromHex("#1685D4"),
                                                                WidthRequest = 20,
                                                                HeightRequest = 20,
                                                                CornerRadius = 100
                                                            },
                                                            new Button {
                                                              BackgroundColor = Color.FromHex("#F6C954"),
                                                                WidthRequest = 20,
                                                                HeightRequest = 20,
                                                                CornerRadius = 100
                                                            }
                                                          }
                                                        }
                                                    }
                                                }
                                              }
                                        }
                                    };


                                    img.Clicked += details_but;


                                    if (i % 2 == 0)
                                    {
                                        //stackLayout.Children.Add(frame1);
                                        stackLayout.Children.Add(frame1);
                                        mainLayout.Children.Add(stackLayout);
                                    }
                                    else
                                    {
                                        mainLayout.Children.Add(list[i - 1]);
                                        list[i - 1].Children.Add(frame1);

                                    }
                                    i++;

                                }
                            }
                        }
                    }


                }

                catch
                {

                }
            }
            else if (pageName == "Заказы")
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
                {
                    sqlConnection.Open();

                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandType = System.Data.CommandType.Text;

                        sqlCommand.CommandText = "SELECT o.order_id, o.user_id, o.order_date, o.full_price, oi.quantity, oi.color_hex, oi.size, g.* FROM Orders AS o JOIN OrderItem AS oi ON o.order_id = oi.order_id JOIN Goods AS g ON oi.goods_id = g.goods_id WHERE o.user_id = @UserId;";
                        DbManager dbManager = new DbManager();
                        string user_id = dbManager.SelectUserId();
                        sqlCommand.Parameters.AddWithValue("@UserId", user_id);
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            int i = 0;
                            List<StackLayout> list = new List<StackLayout>();
                            List<Product> products = new List<Product>();
                            while (reader.Read())
                            {
                                products.Add(new Product
                                {
                                    Id = reader["goods_id"].ToString(),
                                    Name = reader["goods_name"].ToString(),
                                    Price = reader["goods_price"].ToString(),
                                    Image = reader["goods_img_path"].ToString()
                                });
                                cat_id.Add(reader["goods_category_id"].ToString());
                                quantity.Add(reader["quantity"].ToString());
                                sizes.Add(reader["size"].ToString());
                                colors.Add(reader["color_hex"].ToString());

                            }
                            foreach (var product in products)
                            {
                                if (product.Name.Length > 15)
                                {
                                    product.Name = Home.TruncateText(product.Name, 15);
                                }
                                if (cat_id[i] != "1" || cat_id[i] != "5")
                                {
                                    ImageButton img = new ImageButton()
                                    {
                                        Source = ImageSource.FromUri(new Uri(product.Image)),
                                        BackgroundColor = Color.Transparent,
                                        HeightRequest = 120,
                                        WidthRequest = 145,
                                        VerticalOptions = LayoutOptions.Start,
                                        HorizontalOptions = LayoutOptions.Center,
                                        CommandParameter = product,
                                    };

                                    StackLayout stackLayout = new StackLayout
                                    {
                                        Orientation = StackOrientation.Horizontal,
                                        Margin = new Thickness(0, 20, 0, 0),
                                        Spacing = 20,
                                        HorizontalOptions = LayoutOptions.Center,
                                        VerticalOptions = LayoutOptions.Start
                                    };
                                    string t1 = $"Количество: {quantity[i]}\nРазмер: {sizes[i]}";
                                    if (sizes[i] == "\t         ")
                                    {
                                        t1 = $"Количество: {quantity[i]}\n";
                                    }
                                    list.Add(stackLayout);
                                    var frame1 = new Frame
                                    {
                                        BackgroundColor = Color.White,
                                        CornerRadius = 20,
                                        HasShadow = false,
                                        Padding = 10,
                                        WidthRequest = 165,
                                        HeightRequest = 225,
                                        Content = new StackLayout
                                        {
                                            Children = {
                                                img,
                                                new Label {
                                                  Text = t1,
                                                    TextTransform = TextTransform.Uppercase,
                                                    HorizontalOptions = LayoutOptions.Start,
                                                    VerticalOptions = LayoutOptions.Start,
                                                    TextColor = Color.FromHex("#5B9EE1"),
                                                    FontFamily = "Roboto",
                                                    FontSize = 14,
                                                    FontAttributes = FontAttributes.Italic
                                                },
                                                new Label {
                                                  Text = (product.Name),
                                                  LineBreakMode = LineBreakMode.NoWrap,
                                                    HorizontalOptions = LayoutOptions.Start,
                                                    VerticalOptions = LayoutOptions.Start,
                                                    TextColor = Color.Black,
                                                    FontFamily = "Roboto",
                                                    FontSize = 20,
                                                    FontAttributes = FontAttributes.Bold
                                                },
                                                new StackLayout {
                                                  Orientation = StackOrientation.Horizontal,
                                                    VerticalOptions = LayoutOptions.EndAndExpand,
                                                    Children = {
                                                      new Label {
                                                        Text = $"${(product.Price)}",
                                                          HorizontalOptions = LayoutOptions.Start,
                                                          VerticalOptions = LayoutOptions.Center,
                                                          TextColor = Color.Black,
                                                          FontFamily = "Roboto",
                                                          FontSize = 20,
                                                          FontAttributes = FontAttributes.Bold
                                                      },
                                                      new StackLayout {
                                                        Orientation = StackOrientation.Horizontal,
                                                          HorizontalOptions = LayoutOptions.EndAndExpand,
                                                          VerticalOptions = LayoutOptions.Center,
                                                          Children =
                                                          {
                                                              new Button
                                                              {
                                                                  HeightRequest = 20,
                                                                  WidthRequest = 20,
                                                                  BackgroundColor = Color.FromHex(colors[i]),
                                                                  CornerRadius = 20
                                                              }
                                                          }
                                                        }
                                                    }
                                                }
                                        }
                                        }
                                    };


                                    img.Clicked += details_but;


                                    if (i % 2 == 0)
                                    {
                                        //stackLayout.Children.Add(frame1);
                                        stackLayout.Children.Add(frame1);
                                        mainLayout.Children.Add(stackLayout);
                                    }
                                    else
                                    {
                                        mainLayout.Children.Add(list[i - 1]);
                                        list[i - 1].Children.Add(frame1);

                                    }
                                    i++;
                                }

                            }
                        }
                    }
                }
            }
            back.Source = ImageSource.FromResource("rpm_prodject.images.back.png");
            //WhiteBlueRed9.Source = ImageSource.FromResource("rpm_prodject.images.WhiteBlueRed9.png");
            //Blue9.Source = ImageSource.FromResource("rpm_prodject.images.Blue9.png");
            //WhiteRed9.Source = ImageSource.FromResource("rpm_prodject.images.WhiteRed9.png");
            //WhiteBlue9.Source = ImageSource.FromResource("rpm_prodject.images.WhiteBlue9.png");

        }

        public Shablon(string categoryId, string categoryName) 
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            back.Source = ImageSource.FromResource("rpm_prodject.images.back.png");
            string srvrdbname = "rpm";
            string srvrname = "192.168.1.96";
            string srvrusername = "samir";
            string srvrpassword = "123456";
            izbrannoe.Text = categoryName;
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = System.Data.CommandType.Text;

                    sqlCommand.CommandText = "SELECT * FROM Goods WHERE goods_category_id = @CatId";
                    sqlCommand.Parameters.AddWithValue("@CatId", categoryId);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        int i = 0;
                        List<StackLayout> list = new List<StackLayout>();
                        List<Product> products = new List<Product>();
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Id = reader["goods_id"].ToString(),
                                Name = reader["goods_name"].ToString(),
                                Price = reader["goods_price"].ToString(),
                                //Image = "rpm_prodject.images.RedKross.png"
                                Image = reader["goods_img_path"].ToString()
                            });
                        }
                        foreach (var product in products)
                        {
                            ImageButton img = new ImageButton()
                            {
                                Source = ImageSource.FromUri(new Uri(product.Image)),
                                Aspect = Aspect.AspectFill,
                                BackgroundColor = Color.Transparent,
                                HeightRequest = 120,
                                WidthRequest = 145,
                                VerticalOptions = LayoutOptions.Start,
                                HorizontalOptions = LayoutOptions.Center,
                                CommandParameter = product,
                            };

                            StackLayout stackLayout = new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Margin = new Thickness(0, 20, 0, 0),
                                Spacing = 20,
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Start
                            };
                            list.Add(stackLayout);
                            if (product.Name.Length > 15)
                            {
                                product.Name = Home.TruncateText(product.Name, 15);
                            }
                            var frame1 = new Frame
                            {
                                BackgroundColor = Color.White,
                                CornerRadius = 20,
                                HasShadow = false,
                                Padding = 10,
                                WidthRequest = 165,
                                HeightRequest = 225,
                                Content = new StackLayout
                                {
                                    Children = {
                                                                        img,

                                                new Label {
                                                  Text = "Акция",
                                                    TextTransform = TextTransform.Uppercase,
                                                    HorizontalOptions = LayoutOptions.Start,
                                                    VerticalOptions = LayoutOptions.Start,
                                                    TextColor = Color.FromHex("#5B9EE1"),
                                                    FontFamily = "Roboto",
                                                    FontSize = 14,
                                                    FontAttributes = FontAttributes.Italic
                                                },
                                                new Label {
                                                  Text = (product.Name),
                                                    HorizontalOptions = LayoutOptions.Start,
                                                    VerticalOptions = LayoutOptions.Start,
                                                    TextColor = Color.Black,
                                                    FontFamily = "Roboto",
                                                    FontSize = 20,
                                                    FontAttributes = FontAttributes.Bold
                                                },
                                                new StackLayout {
                                                  Orientation = StackOrientation.Horizontal,
                                                    VerticalOptions = LayoutOptions.EndAndExpand,
                                                    Children = {
                                                      new Label {
                                                        Text = $"${(product.Price)}",
                                                          HorizontalOptions = LayoutOptions.Start,
                                                          VerticalOptions = LayoutOptions.Center,
                                                          TextColor = Color.Black,
                                                          FontFamily = "Roboto",
                                                          FontSize = 20,
                                                          FontAttributes = FontAttributes.Bold
                                                      },
                                                      new StackLayout {
                                                        Orientation = StackOrientation.Horizontal,
                                                          HorizontalOptions = LayoutOptions.EndAndExpand,
                                                          VerticalOptions = LayoutOptions.Center,
                                                        }
                                                    }
                                                }
                                              }
                                }
                            };


                            img.Clicked += details_but;


                            if (i % 2 == 0)
                            {
                                //stackLayout.Children.Add(frame1);
                                stackLayout.Children.Add(frame1);
                                mainLayout.Children.Add(stackLayout);
                            }
                            else
                            {
                                mainLayout.Children.Add(list[i - 1]);
                                list[i - 1].Children.Add(frame1);

                            }
                            i++;

                        }
                    }
                }
            }

        }

        public Shablon(string find, int a)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            back.Source = ImageSource.FromResource("rpm_prodject.images.back.png");
            string srvrdbname = "rpm";
            string srvrname = "192.168.1.96";
            string srvrusername = "samir";
            string srvrpassword = "123456";
            izbrannoe.Text = "Поиск";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            using (SqlConnection sqlConnection = new SqlConnection(sqlconn))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = System.Data.CommandType.Text;

                    sqlCommand.CommandText = "SELECT * FROM Goods WHERE goods_name LIKE @Find";
                    sqlCommand.Parameters.AddWithValue("@Find", "%" + find + "%");
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        int i = 0;
                        List<StackLayout> list = new List<StackLayout>();
                        List<Product> products = new List<Product>();
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Id = reader["goods_id"].ToString(),
                                Name = reader["goods_name"].ToString(),
                                Price = reader["goods_price"].ToString(),
                                //Image = "rpm_prodject.images.RedKross.png"
                                Image = reader["goods_img_path"].ToString()
                            });
                        }
                        if (products.Count > 0)
                        {


                            foreach (var product in products)
                            {
                                ImageButton img = new ImageButton()
                                {
                                    Source = ImageSource.FromUri(new Uri(product.Image)),
                                    Aspect = Aspect.AspectFill,
                                    BackgroundColor = Color.Transparent,
                                    HeightRequest = 120,
                                    WidthRequest = 145,
                                    VerticalOptions = LayoutOptions.Start,
                                    HorizontalOptions = LayoutOptions.Center,
                                    CommandParameter = product,
                                };

                                StackLayout stackLayout = new StackLayout
                                {
                                    Orientation = StackOrientation.Horizontal,
                                    Margin = new Thickness(0, 20, 0, 0),
                                    Spacing = 20,
                                    HorizontalOptions = LayoutOptions.Center,
                                    VerticalOptions = LayoutOptions.Start
                                };
                                list.Add(stackLayout);
                                if (product.Name.Length > 15)
                                {
                                    product.Name = Home.TruncateText(product.Name, 15);
                                }
                                var frame1 = new Frame
                                {
                                    BackgroundColor = Color.White,
                                    CornerRadius = 20,
                                    HasShadow = false,
                                    Padding = 10,
                                    WidthRequest = 165,
                                    HeightRequest = 225,
                                    Content = new StackLayout
                                    {
                                        Children = {
                                                                        img,

                                                new Label {
                                                  Text = "Акция",
                                                    TextTransform = TextTransform.Uppercase,
                                                    HorizontalOptions = LayoutOptions.Start,
                                                    VerticalOptions = LayoutOptions.Start,
                                                    TextColor = Color.FromHex("#5B9EE1"),
                                                    FontFamily = "Roboto",
                                                    FontSize = 14,
                                                    FontAttributes = FontAttributes.Italic
                                                },
                                                new Label {
                                                  Text = (product.Name),
                                                    HorizontalOptions = LayoutOptions.Start,
                                                    VerticalOptions = LayoutOptions.Start,
                                                    TextColor = Color.Black,
                                                    FontFamily = "Roboto",
                                                    FontSize = 20,
                                                    FontAttributes = FontAttributes.Bold
                                                },
                                                new StackLayout {
                                                  Orientation = StackOrientation.Horizontal,
                                                    VerticalOptions = LayoutOptions.EndAndExpand,
                                                    Children = {
                                                      new Label {
                                                        Text = $"${(product.Price)}",
                                                          HorizontalOptions = LayoutOptions.Start,
                                                          VerticalOptions = LayoutOptions.Center,
                                                          TextColor = Color.Black,
                                                          FontFamily = "Roboto",
                                                          FontSize = 20,
                                                          FontAttributes = FontAttributes.Bold
                                                      },
                                                      new StackLayout {
                                                        Orientation = StackOrientation.Horizontal,
                                                          HorizontalOptions = LayoutOptions.EndAndExpand,
                                                          VerticalOptions = LayoutOptions.Center,
                                                        }
                                                    }
                                                }
                                              }
                                    }
                                };


                                img.Clicked += details_but;


                                if (i % 2 == 0)
                                {
                                    //stackLayout.Children.Add(frame1);
                                    stackLayout.Children.Add(frame1);
                                    mainLayout.Children.Add(stackLayout);
                                }
                                else
                                {
                                    mainLayout.Children.Add(list[i - 1]);
                                    list[i - 1].Children.Add(frame1);

                                }
                                i++;

                            }
                        }
                    }
                }
            }
        }
        private async void Back(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void details_but(object sender, EventArgs e)
        {
            var product = (Product)((ImageButton)sender).CommandParameter;
            await Navigation.PushAsync(new Details(product));
        }
        private async void home_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Home());
        }
        private async void profile_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Profile());
        }
        private async void basket_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MyCart());
        }
        private async void uved_but(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Notifications());
        }
       
    }
   
}