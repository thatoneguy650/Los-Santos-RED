using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Flags]
public enum FoodType
{
    None = 0,
    Generic = 1 << 0,
    American = 1 << 1,
    Italian = 1 << 2,
    Asian = 1 << 3,

    Mexican = 1 << 4,
    Korean = 1 << 5,
    Chinese = 1 << 6,
    Japanese = 1 << 7,
    Snack = 1 << 8,
    Donut = 1 << 9,
    Sandwiches = 1 << 10,
    Seafood = 1 << 11,
    Burger = 1 << 12,
    Pizza = 1 << 13,
    FastFood = 1 << 14,
    Bagels = 1 << 15,
    Coffee = 1 << 16,
    Smoothies = 1 << 17,
    Chicken = 1 << 18,
    Dessert = 1 << 19,
}