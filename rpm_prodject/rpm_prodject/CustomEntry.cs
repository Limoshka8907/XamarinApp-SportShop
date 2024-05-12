using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace rpm_prodject
{
    public class CustomEntry : Entry
    {

        public static readonly BindableProperty BorderColorProperty =
               BindableProperty.Create(
                   nameof(BorderColor),
                   typeof(Color),
                   typeof(CustomEntry),
                   Color.Gray);

   
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty BorderWidthProperty =
        BindableProperty.Create(
            nameof(BorderWidth),
            typeof(int),
            typeof(CustomEntry),
            Device.OnPlatform<int>(1, 2, 2));

    
        public int BorderWidth
        {
            get { return (int)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }

        public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(double),
            typeof(CustomEntry),
            Device.OnPlatform<double>(6, 7, 7));

   
        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty IsCurvedCornersEnabledProperty =
        BindableProperty.Create(
            nameof(IsCurvedCornersEnabled),
            typeof(bool),
            typeof(CustomEntry),
            true);

      
        public bool IsCurvedCornersEnabled
        {
            get { return (bool)GetValue(IsCurvedCornersEnabledProperty); }
            set { SetValue(IsCurvedCornersEnabledProperty, value); }
        }

        public static readonly BindableProperty CursorColorProperty = BindableProperty.Create(
    nameof(CursorColor), typeof(Color),
    typeof(CustomEntry), Color.Black);
      
        public Color CursorColor
        {
            get { return (Color)GetValue(CursorColorProperty); }
            set { SetValue(CursorColorProperty, value); }
        }
        public CustomEntry()
        {   
            this.Unfocused += (sender, e) => {
                this.CursorColor = Color.Black;
            };
            this.Focused += (sender, e) =>
            {
                this.CursorColor = Color.Blue;
            };
        }
        
    }
}
