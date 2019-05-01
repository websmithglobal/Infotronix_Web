using System;
using System.Collections.Generic;
using ENT = Josheph.Framework.Entity;

public static class GlobalVarible
{
    //public static FormResultEntity FormResult = new FormResultEntity();
    //public static ENT.SystemSettings SystemSettings = new ENT.SystemSettings();
    //public static ENT.SystemSettings UserSettings = new ENT.SystemSettings();
    //public static ENT.UserProfile UserProfile = new ENT.UserProfile();

    public static string GetMessage()
    {
        try
        {
            if (MySession.Current.MessageResult.Message.Count != 0)
            {
                string strResult = "";
                if (!MySession.Current.MessageResult.isReadData)
                {
                    string errorlist = "<ul>";
                    foreach (string str in MySession.Current.MessageResult.Message)
                    {
                        errorlist += string.Format("<li>{0}</li>", str);
                    }
                    errorlist += "</ul>";
                    if (MySession.Current.MessageResult.EntryStatus)
                    {
                        strResult = string.Format("<div class='alert alert-success'><a href ='#' class='close' data-dismiss='alert' aria-label='close'></a>{0}</div>", errorlist);
                    }
                    else
                    {
                        strResult = string.Format("<div class='alert alert-danger'><a href ='#' class='close' data-dismiss='alert' aria-label='close'></a>{0}</div>", errorlist);
                    }
                }
                return strResult;
            }
            else { return string.Empty; }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            Clear();
        }
    }

    public static string GetMessageHTML()
    {
        try
        {
            if (MySession.Current.MessageResult.Message.Count != 0)
            {
                string strResult = "";
                if (!MySession.Current.MessageResult.isReadData)
                {
                    string errorlist = "<ul>";
                    foreach (string str in MySession.Current.MessageResult.Message)
                    {
                        errorlist += string.Format("<li>{0}</li>", str);
                    }
                    errorlist += "</ul>";
                    if (MySession.Current.MessageResult.EntryStatus)
                    {
                        strResult = string.Format("<div class='alert alert-success'><a href ='#' class='close' data-dismiss='alert' aria-label='close'></a>{0}</div>", errorlist);
                    }
                    else
                    {
                        strResult = string.Format("<div class='alert alert-danger'><a href ='#' class='close' data-dismiss='alert' aria-label='close'></a>{0}</div>", errorlist);
                    }
                }
                return strResult;
            }
            else { return string.Empty; }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public static void Clear()
    {
        MySession.Current.MessageResult.isReadData = true;
        MySession.Current.MessageResult.EntryStatus = false;
        MySession.Current.MessageResult.Message.Clear();
    }

    public static void AddError(string Error)
    {
        MySession.Current.MessageResult.isReadData = false;
        MySession.Current.MessageResult.EntryStatus = false;
        MySession.Current.MessageResult.Message.Add(Error);
        MySession.Current.MessageResult.MessageHtml = GetMessageHTML();
    }

    public static void AddMessage(string Error)
    {
        MySession.Current.MessageResult.isReadData = false;
        MySession.Current.MessageResult.EntryStatus = true;
        MySession.Current.MessageResult.Message.Add(Error);
        MySession.Current.MessageResult.MessageHtml = GetMessageHTML();
    }
    
}
