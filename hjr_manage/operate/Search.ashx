<%@ WebHandler Language="C#" Class="Search" %>

using System;
using System.Web;

public class Search : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("<script>alert('sdadsasda');</script>");
        operate();
    }

    private void operate()
    {
        try
        {
            string searchStr = HttpContext.Current.Request["searchStr"];

            string state = string.Empty;
            state = "success";

            HttpContext.Current.Response.Write(state);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);
        }

    }




    public bool IsReusable {
        get {
            return false;
        }
    }

}