using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class RandomItems
{
    public static readonly Random MyRand = new Random();
    public static bool RandomPercent(float Percent)
    {
        if (MyRand.Next(1, 101) <= Percent)
            return true;
        else
            return false;
    }
    public static bool RandomLargePercent(float Percent)
    {
        if (MyRand.Next(1, 1001) <= Percent)
            return true;
        else
            return false;
    }
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[MyRand.Next(s.Length)]).ToArray());
    }
    public static string RandomNumberString(int length)
    {
        const string chars = "01234567890123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[MyRand.Next(s.Length)]).ToArray());
    }
    public static char RandomLetter()
    {
        int num = MyRand.Next(0, 26); // Zero to 25
        char let = (char)('a' + num);
        return let;
    }
    public static char RandomNumber()
    {
        char let = Convert.ToChar(RandomNumberString(1));
        return let;
    }
    public static uint GetRandomNumber(uint minimum, uint maximum)
    {
        return (uint)MyRand.Next((int)minimum, (int)maximum);
    }
    public static uint GetRandomNumber(int minimum, int maximum)
    {
        return (uint)MyRand.Next(minimum, maximum);
    }
    public static int GetRandomNumberInt(int minimum, int maximum)
    {
        return MyRand.Next(minimum, maximum);
    }
    public static double GetRandomNumber(double minimum, double maximum)
    {
        return MyRand.NextDouble() * (maximum - minimum) + minimum;
    }
    public static float GetRandomNumber(float minimum, float maximum)
    {
        return (float)(MyRand.NextDouble() * (maximum - minimum) + minimum);
    }

    internal static float GetRandomNumber(object value, object groupDistance)
    {
        throw new NotImplementedException();
    }

}

