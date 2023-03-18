using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniTools
{

    /// <summary>
    /// Convert the given value in a percentage string, with the given number of digits (optional, default = 0).
    /// </summary>
    /// <param name="value"></param>
    /// <param name="digits"></param>
    /// <returns></returns>
    public static string ConvertPercentage(float value, int decimalPlaces = 0)
    {
        var d = Convert.ToDouble(value * 100);
        return Math.Round(d, decimalPlaces) + "%";
    }

    /// <summary>
    /// Convert the given number of bytes into a more readable string of Kilobytes, Megabytes, Gigabytes and so on, with the given number of digits (optional, default = 1).
    /// </summary>
    /// <param name="value"></param>
    /// <param name="decimalPlaces"></param>
    /// <returns></returns>
    public static string ConvertBytes(float value, int decimalPlaces = 1)
    {
        string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        int mag = (int)Math.Log(value, 1024);
        decimal adjustedSize = (decimal)value / (1L << (mag * 10));
        if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
        {
            mag += 1;
            adjustedSize /= 1024;
        }

        return string.Format("{0:n" + decimalPlaces + "} {1}", adjustedSize, SizeSuffixes[mag]);
    }

    /// <summary>
    /// Return the today date converted in a string suitable to be saved on Database (default format: 'yyyy-MM-dd H:mm:ss').
    /// <para>format (optional) : the format to use to get the string.</para>
    /// </summary>
    /// <returns></returns>
    public static string GetDate(string format = "yyyy-MM-dd H:mm:ss")
    {
        return DateTime.Now.ToString(format);
    }

    /// <summary>
    /// Return the given date converted in a string suitable to be saved on Database (default format: 'yyyy-MM-dd H:mm:ss').
    /// <para>date : the date object to convert.</para>
    /// <para>format (optional) : the format to use to get the string.</para>
    /// </summary>
    /// <param name="date"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string GetDate(DateTime date, string format = "yyyy-MM-dd H:mm:ss")
    {
        return date.ToString("yyyy-MM-dd H:mm:ss");
    }

    /// <summary>
    /// Return a DateTime object from the given date in string format.
    /// <para>date : the date as a string.</para>
    /// <para>cultureInfo (optional, 'en-US' by default): the country code the data is formatted to.</para>
    /// </summary>
    /// <param name="date"></param>
    /// <param name="cultureInfo"></param>
    /// <returns></returns>
    public static DateTime GetDateFromString(string date, string cultureInfo = "en-US")
    {
        if (date == "") return default;

        try
        {
            var ci = new System.Globalization.CultureInfo(cultureInfo);
            return DateTime.Parse(date, ci);
        }
        catch (Exception)
        {
            return default;
        }
        
    }

}
