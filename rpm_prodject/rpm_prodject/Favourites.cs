using System;
using System.Collections.Generic;
using System.Text;

namespace rpm_prodject
{
    public class Favourites
    {
        public static List<Product> FavouritesList { get; set; }

        static Favourites()
        {
            FavouritesList = new List<Product>();
        }
    }
    
}
