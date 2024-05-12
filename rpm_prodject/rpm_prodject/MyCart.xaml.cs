using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rpm_prodject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyCart : ContentPage
    {

        public MyCart()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            back.Source = ImageSource.FromResource("rpm_prodject.images.back.png");
            //BlueKross8.Source = ImageSource.FromResource("rpm_prodject.images.BlueKross8.png");
            //WhiteRedKross8.Source = ImageSource.FromResource("rpm_prodject.images.WhiteRedKross8.png");
            //PurpleKross8.Source = ImageSource.FromResource("rpm_prodject.images.PurpleKross8.png");

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
            Busket._products.Remove(prod);
            Busket.productQuantities.Remove(prod.Id);
            price1.Text = $"${Busket.cost}";
            price_sale.Text = $"${Busket.cost - (Busket.cost / 100 * 10)}";
            price2.Text = $"${Busket.cost - (Busket.cost / 100 * 10)}";
            LoadForm();
            MyCart my = new MyCart();
            var navigation = Application.Current.MainPage.Navigation;
            await navigation.PopAsync(); // Убрать текущую страницу
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
                        //Busket.productQuantities.Remove(product.Id);
                        //Busket._products.Remove(product);
                        continue;
                    }
                    //product.Name += $"Size:{Busket.size[i]} Color: {Busket.color[i].ToHex()}";
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


                // Безопасно увеличиваем количество товара, если он есть в словаре
                if (Busket.productQuantities.TryGetValue(product.Id, out int currentQuantity))
                {
                    //Busket.productQuantities[product.Id] = currentQuantity + 1;
                }
                else
                {
                    // Если товара нет в словаре, добавляем его с количеством 1
                    Busket.productQuantities.Add(product.Id, 1);
                }

                // Создайте Grid для карточки
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

                // Создайте изображение продукта
                ImageButton productImage = new ImageButton
                {
                    //Grid.Column = 0,
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
                    //Grid.Column = 0,
                    BackgroundColor = (Busket.color[i]),
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 10,
                    HeightRequest = 10,
                    CornerRadius = 15,
                    Margin = 4
                    //BackgroundColor = Color.Transparent,
                };
                productCardGrid.Children.Add(colorImage);
                Grid.SetColumn(colorImage, 0);
                // Создайте стек для содержимого карточки
                StackLayout productCardContentStackLayout = new StackLayout
                {
                    //Grid.Column = 1,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                productCardGrid.Children.Add(productCardContentStackLayout);
                Grid.SetColumn(productCardContentStackLayout, 1);

                // Создайте заголовок продукта
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
                // Создайте цену продукта
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

                // Создайте стек для кнопок увеличения и уменьшения количества
                StackLayout productQuantityStackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    Spacing = 15
                };

                // Создайте кнопку уменьшения количества
                ImageButton productQuantityMinusButton = new ImageButton
                {
                    Source = "minuse8.png",
                    HorizontalOptions = LayoutOptions.Start,
                    WidthRequest = 25,
                    HeightRequest = 25,
                    BackgroundColor = Color.Transparent,
                    CommandParameter = product.Id

                };
                // Создайте метку количества
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
                        Busket.productQuantities[productId]--; // Уменьшение количества на 1
                        productQuantityLabel.Text = Busket.productQuantities[productId].ToString(); // Обновление текста метки
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
                                    Busket.color.RemoveAt(j);
                                }
                                j++;
                            }
                            Busket._products.Remove(prod);
                            Busket.productQuantities.Remove(prod.Id);

                            LoadForm();
                            MyCart my = new MyCart();
                            var navigation = Application.Current.MainPage.Navigation;
                            navigation.PopAsync(); // Убрать текущую страницу
                            navigation.PushAsync(new MyCart());

                        }
                    }
                };

                // Создайте кнопку увеличения количества
                ImageButton productQuantityPlusButton = new ImageButton
                {
                    Source = "pluce8.png",
                    HorizontalOptions = LayoutOptions.Start,
                    WidthRequest = 25,
                    HeightRequest = 25,
                    BackgroundColor = Color.Transparent,
                    CommandParameter = product.Id

                };
                //Busket.productQuantities.Add("1", int.Parse(product.Id));
                productQuantityPlusButton.Clicked += (sender, args) =>
                {

                    ImageButton button = ((ImageButton)sender);
                    string productId = (string)button.CommandParameter;
                    var prod = Busket._products.FirstOrDefault(p => p.Id == productId);

                    if (Busket.productQuantities.ContainsKey(productId))
                    {
                        Busket.productQuantities[productId]++; // Увеличение количества на 1
                        Busket.cost += float.Parse(prod.Price);
                        price1.Text = $"${Busket.cost}";
                        price_sale.Text = $"${Busket.cost - (Busket.cost / 100 * 10)}";
                        price2.Text = $"${Busket.cost - (Busket.cost / 100 * 10)}";
                        productQuantityLabel.Text = Busket.productQuantities[productId].ToString(); // Обновление текста метки
                    }
                };

                // Добавьте кнопки в стек количества
                productQuantityStackLayout.Children.Add(productQuantityMinusButton);
                productQuantityStackLayout.Children.Add(productQuantityLabel);
                productQuantityStackLayout.Children.Add(productQuantityPlusButton);

                // Добавьте содержимое в стек карточки
                productCardContentStackLayout.Children.Add(productTitleLabel);
                productCardContentStackLayout.Children.Add(productPriceLabel);
                productCardContentStackLayout.Children.Add(productQuantityStackLayout);

                // Создайте стек для размера и кнопки удаления
                StackLayout productSizeAndDeleteStackLayout = new StackLayout
                {
                    // Grid.Column = 2,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                productCardGrid.Children.Add(productSizeAndDeleteStackLayout);
                Grid.SetColumn(productSizeAndDeleteStackLayout, 2);

                // Создайте метку размера
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

                // Создайте кнопку удаления
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
                // Добавьте размер и кнопку в стек размера и удаления
                productSizeAndDeleteStackLayout.Children.Add(productSizeLabel);
                productSizeAndDeleteStackLayout.Children.Add(productDeleteButton);

                // Добавьте содержимое в Grid карточки
                productCardGrid.Children.Add(productImage);
                productCardGrid.Children.Add(productCardContentStackLayout);
                productCardGrid.Children.Add(productSizeAndDeleteStackLayout);

                // Добавьте Grid карточки в стек карточек
                productCardStackLayout.Children.Add(productCardGrid);

                // Добавьте стек карточек в основной макет
                // Content = productCardStackLayout;

                i++;
                Busket.cost += float.Parse(product.Price);
            }
        }

    }
}