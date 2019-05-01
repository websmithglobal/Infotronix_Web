using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ENT = Josheph.Framework.Entity;

/// <summary>
/// Summary description for MySession
/// </summary>
public class MySession
{
    // private constructor
    private MySession()
    {
        Username = null;
        PasswordChange = null;
         MessageResult = new FormResultEntity();
    
    }

    // Gets the current session.
    public static MySession Current
    {
        get
        {
            MySession session =
              (MySession)HttpContext.Current.Session["__MySession__"];
            if (session == null)
            {
                session = new MySession();
                HttpContext.Current.Session["__MySession__"] = session;
            }
            return session;
        }
    }

    // **** add your session properties here, e.g like this:

    public string Username { get; set; }
    public string PasswordChange { get; set; }
    public FormResultEntity MessageResult { get; set; }
   
}



