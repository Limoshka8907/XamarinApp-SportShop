using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace rpm_prodject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyCart : ContentPage
    {

        public MyCart()
        {
            InitializeComponent();
            Busket.cost = 0;
            NavigationPage.SetHasNavigationBar(this, false);
            back.Source = ImageSource.FromResource("rpm_prodject.images.back.png");

            line8.Source = ImageSource.FromResource("rpm_prodject.images.line8.png");
            LoadForm();
            price1.Text = $"${Busket.cost}";
            price_sale.Text = $"${Busket.cost - (Busket.cost / 100 * 10)}";
            price2.Text = $"${Busket.cost - (Busket.cost / 100 * 10)}";
        }
        public async void delete_item(object sender, EventArgs e)
        {
            ImageButton button = ((ImageButton)sender);
            string productId = (string)button.CommandParameter;
            var prod = Busket._products.FirstOrDefault(p => p.Id == productId);
            Busket.cost -= float.Parse(prod.Price) * Busket.productQuantities[prod.Id];
            Busket.productQuantities.Remove(prod.Id);
            Busket.size.RemoveAt(Busket._products.IndexOf(prod));
            Busket.color.RemoveAt(Busket._products.IndexOf(prod));
            Busket._products.Remove(prod);
            price1.Text = $"${Busket.cost}";
            price_sale.Text = $"${Busket.cost - (Busket.cost / 100 * 10)}";
            price2.Text = $"${Busket.cost - (Busket.cost / 100 * 10)}";
          
            MyCart my = new MyCart();
            var navigation = Application.Current.MainPage.Navigation;
            await navigation.PopAsync(); 
            await navigation.PushAsync(new MyCart());
        }

        private async void buy_all(object sender, EventArgs e)
        {
            bool pass = true;
            if (Busket._products.Count != 0)
            {
                int i = 0;
                DbManager dbManager = new DbManager();
                dbManager.InsertOrdersTableNonItems();
                foreach (var product in Busket._products)
                {
                    int count = int.Parse(dbManager.SelectCountGood(product.Id));
                    if(count <= 0 || count - Busket.productQuantities[product.Id] < 0)
                    {
                        pass = false;
                        await DisplayAlert("Внимание", $"Товар {product.Name} отсутствует на складе, извините за неудобоство, Товара на складе сейчас: {count}", "ОК");
;
                        continue;
                    }
                    
                    dbManager.InsertOrders(product, Busket.cost.ToString(), Busket.productQuantities[product.Id], Busket.color[i].ToHex().ToString(), Busket.size[i]);
                    dbManager.UpdateGoodCount((count - Busket.productQuantities[product.Id]).ToString(), product.Id);
                    i++;
                }
                if(pass == true)
                {

                    await DisplayAlert("Уведомление", "Товары были куплены, оплата по получению. Спасибо за покупку", "ОК");
                    Busket._products.Clear();
                    Busket.cost = 0;
                    Busket.size.Clear();
                    Busket.color.Clear();
                    Busket.productQuantities.Clear();
                    await Navigation.PushAsync(new Home());
                }
            }
            else
            {
                await DisplayAlert("Внимание", "Товаров нет", "Ок");
            }
        }

        private async void Back(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void details_but(object sender, EventArgs e)
        {
            var product = (Product)sender;
            await Navigation.PushAsync(new Details(product));
        }

        private void LoadForm()
        {


            int i = 0;
            foreach (var product in Busket._products)
            {


 
                if (Busket.productQuantities.TryGetValue(product.Id, out int currentQuantity))
                {
                 
                }
                else
                {
              
                    Busket.productQuantities.Add(product.Id, 1);
                }

         
                Grid productCardGrid = new Grid
                {
                    HeightRequest = 100,
                    Margin = new Thickness(0, 20, 0, 0),
                    ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(0.6, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(0.1, GridUnitType.Star) }
                }

                };

             
                ImageButton productImage = new ImageButton
                {
                   
                    Source = ImageSource.FromUri(new Uri(product.Image)),
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 100,
                    HeightRequest = 100,
                    CornerRadius = 30,
                    BackgroundColor = Color.Transparent,
                };
                productCardGrid.Children.Add(productImage);
                Grid.SetColumn(productImage, 0);

                ImageButton colorImage = new ImageButton
                {
                   
                    BackgroundColor = (Busket.color[i]),
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 10,
                    HeightRequest = 10,
                    CornerRadius = 15,
                    Margin = 4
                    
                };
                productCardGrid.Children.Add(colorImage);
                Grid.SetColumn(colorImage, 0);
              
                StackLayout productCardContentStackLayout = new StackLayout
                {
                    
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                productCardGrid.Children.Add(productCardContentStackLayout);
                Grid.SetColumn(productCardContentStackLayout, 1);

               
                Label productTitleLabel = new Label
                {
                    Text = product.Name,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                    FontFamily = "Roboto",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 20,
                    TextColor = Color.Black,
                    HorizontalTextAlignment = TextAlignment.Start
                };
              
                Label productPriceLabel = new Label
                {
                    Text = product.Price,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                    FontFamily = "Roboto",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 16,
                    TextColor = Color.Black,
                    HorizontalTextAlignment = TextAlignment.Start
                };
                StackLayout productQuantityStackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    Spacing = 15
                };
                
                ImageButton productQuantityMinusButton = new ImageButton
                {
                    Source = "minuse8.png",
                    HorizontalOptions = LayoutOptions.Start,
                    WidthRequest = 25,
                    HeightRequest = 25,
                    BackgroundColor = Color.Transparent,
                    CommandParameter = product.Id

                };
                
                Label productQuantityLabel = new Label
                {
                    Text = Busket.productQuantities[product.Id].ToString(),
                    HorizontalOptions = LayoutOptions.Start,
                    FontFamily = "Roboto",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 16,
                    TextColor = Color.Black,
                    HorizontalTextAlignment = TextAlignment.Center,

                };
                productQuantityMinusButton.Clicked += (sender, args) =>
                {

                    ImageButton button = ((ImageButton)sender);
                    string productId = (string)button.CommandParameter;
                    var prod = Busket._products.FirstOrDefault(p => p.Id == productId);

                    if (Busket.productQuantities.ContainsKey(productId))
                    {
                        Busket.productQuantities[productId]--; 
                        productQuantityLabel.Text = Busket.productQuantities[productId].ToString();
                        Busket.cost -= float.Parse(prod.Price);
                        price1.Text = $"${Busket.cost}";
                        price_sale.Text = $"${Busket.cost - (Busket.cost / 100 * 10)}";
                        price2.Text = $"${Busket.cost - (Busket.cost / 100 * 10)}";
                        if (Busket.productQuantities[productId] == 0)
                        {
                            for (int j = 0; j < Busket.productQuantities.Count; j++)
                            {
                                if (Busket.productQuantities[prod.Id] == 0)
                                {
                                    Busket.cost -= float.Parse(prod.Price) * Busket.productQuantities[prod.Id];
                                    Busket.productQuantities.Remove(prod.Id);
                                    Busket.size.RemoveAt(Busket._products.IndexOf(prod));
                                    Busket.color.RemoveAt(Busket._products.IndexOf(prod));
                                    Busket._products.Remove(prod);
                                    
                                }
                                j++;
                            }
                            Busket._products.Remove(prod);
                            Busket.productQuantities.Remove(prod.Id);

                            LoadForm();
                            MyCart my = new MyCart();
                            var navigation = Application.Current.MainPage.Navigation;
                            navigation.PopAsync(); 
                            navigation.PushAsync(new MyCart());

                        }
                    }
                };

                
                ImageButton productQuantityPlusButton = new ImageButton
                {
                    Source = "pluce8.png",
                    HorizontalOptions = LayoutOptions.Start,
                    WidthRequest = 25,
                    HeightRequest = 25,
                    BackgroundColor = Color.Transparent,
                    CommandParameter = product.Id

                };
                
                productQuantityPlusButton.Clicked += (sender, args) =>
                {

                    ImageButton button = ((ImageButton)sender);
                    string productId = (string)button.CommandParameter;
                    var prod = Busket._products.FirstOrDefault(p => p.Id == productId);

                    if (Busket.productQuantities.ContainsKey(productId))
                    {
                        Busket.productQuantities[productId]++; 
                        Busket.cost += float.Parse(prod.Price);
                        price1.Text = $"${Busket.cost}";
                        price_sale.Text = $"${Busket.cost - (Busket.cost / 100 * 10)}";
                        price2.Text = $"${Busket.cost - (Busket.cost / 100 * 10)}";
                        productQuantityLabel.Text = Busket.productQuantities[productId].ToString(); 
                    }
                };

               
                productQuantityStackLayout.Children.Add(productQuantityMinusButton);
                productQuantityStackLayout.Children.Add(productQuantityLabel);
                productQuantityStackLayout.Children.Add(productQuantityPlusButton);

               
                productCardContentStackLayout.Children.Add(productTitleLabel);
                productCardContentStackLayout.Children.Add(productPriceLabel);
                productCardContentStackLayout.Children.Add(productQuantityStackLayout);

              
                StackLayout productSizeAndDeleteStackLayout = new StackLayout
                {
                    
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                productCardGrid.Children.Add(productSizeAndDeleteStackLayout);
                Grid.SetColumn(productSizeAndDeleteStackLayout, 2);

                
                Label productSizeLabel = new Label
                {
                    Text = Busket.size[i],
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 5, 0, 0),
                    VerticalOptions = LayoutOptions.Start,
                    FontFamily = "Roboto",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 14,
                    TextColor = Color.Black,
                    HorizontalTextAlignment = TextAlignment.Center
                };

               
                ImageButton productDeleteButton = new ImageButton
                {
                    Source = "bin8.png",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    Margin = new Thickness(0, 0, 0, 10),
                    WidthRequest = 25,
                    HeightRequest = 25,
                    BackgroundColor = Color.Transparent,
                    CommandParameter = product.Id
                };
                productDeleteButton.Clicked += delete_item;
               
                productSizeAndDeleteStackLayout.Children.Add(productSizeLabel);
                productSizeAndDeleteStackLayout.Children.Add(productDeleteButton);

               
                productCardGrid.Children.Add(productImage);
                productCardGrid.Children.Add(productCardContentStackLayout);
                productCardGrid.Children.Add(productSizeAndDeleteStackLayout);

               
                productCardStackLayout.Children.Add(productCardGrid);

           
                i++;
                Busket.cost += float.Parse(product.Price) * Busket.productQuantities[product.Id];
            }
        }

    }
}