
using Rage;
using Rage.Native;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

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

}