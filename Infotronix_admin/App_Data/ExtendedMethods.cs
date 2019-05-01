using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;
using System.Globalization;

public static class ExtendedMethods
{
  
    
    public static Guid GetLogged_Userid(this IPrincipal user)
    {
        Guid usrid = new Guid(user.Identity.GetUserId());

        //return userRoles.Any(u => roles.Contains(u));
        return usrid;
    }

    public static string GetFormatedDate(this DateTime date)
    {
        return date.ToString("dd/MM/yyyy");
    }

    public static string GetFormatedTime(this DateTime date)
    {
        return date.ToString("hh:mm:ss tt");
    }

    public static string GetFormatedDateTime(this DateTime date)
    {
        return date.ToString("dd/MM/yyyy hh:mm:ss tt");
    }

    public static string GetFromatedAmount(this Decimal amt)
    {
        return amt.ToString("c");
    }

    public static bool ValidatePANCardNumber(this String str)
    {

        bool blnResult = false;
        var match = Regex.Match(str, @"[A-Z]{5}\d{4}[A-Z]{1}", RegexOptions.IgnoreCase);

        if (match.Success)
        {
            blnResult = true;
        }

        return blnResult;
    }

    public static bool ValidateUserName(this String str)
    {
        bool blnResult = false;
        var match = Regex.Match(str, @"^[a-zA-Z][a-zA-Z0-9]{3,9}$", RegexOptions.IgnoreCase);

        if (match.Success)
        {
            blnResult = true;
        }
        return blnResult;
    }

    public static bool ValidateDate(this String strDate, string dateFormat)
    {
        bool blnResult = false;
        if (Regex.IsMatch(strDate, "(((0|1)[0-9]|2[1-9]|3[0-1])\\/(0[1-9]|1[0-2])\\/((19|20)\\d\\d))$"))
        {
            DateTime dt;
            blnResult = DateTime.TryParseExact(strDate, dateFormat, new CultureInfo("en-GB"), DateTimeStyles.None, out dt);
        }
        else {
            blnResult = false;
        }
        return blnResult;
    }

    public static string ListToCSVString<T>(this ICollection<T> lst, string seprater)
    {
        return string.Join(seprater, lst);
    }

    public static string GetSubString(this string strValue, int Length)
    {
        if (strValue.Length > Length)
        {
            return strValue.Substring(0, Length);
        }
        else {
            return strValue;
        }
    }

    public static string GetSubString(this Int64 strValue, int Length)
    {
        if (strValue.ToString().Length > Length)
        {
            return strValue.ToString().Substring(0, Length);
        }
        else {
            return strValue.ToString();
        }
    }

    public static void SortList<T>(List<T> list, string columnName, string sortDir)
    {
        var property = typeof(T).GetProperty(columnName);
        var multiplier = sortDir != "asc" ? -1 : 1;
        list.Sort((t1, t2) =>
        {
            var col1 = property.GetValue(t1);
            var col2 = property.GetValue(t2);
            return multiplier * Comparer<object>.Default.Compare(col1, col2);
        });
    }

    public static bool ValiDateMasterArry(string[] strarr)
    {
        bool blnResult = true;
        if (strarr != null)
        {
            if (strarr.Count() > 0)
            {
                foreach (string el in strarr)
                {
                    Guid outID = new Guid();
                    if (Guid.TryParse(el, out outID))
                    {
                        if (el.ToString() == "00000000-0000-0000-0000-000000000000")
                        {
                            blnResult = false;
                            break;
                        }
                    }
                    else { blnResult = false; break; }
                }
            }
            else { blnResult = false; }
        }
        else { blnResult = false; }
        return blnResult;
    }    
}
