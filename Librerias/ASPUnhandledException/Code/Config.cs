using System;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;

namespace ASPUnhandledException
{
    // --
    // -- processes custom .config section as follows:
    // --
    // -- <configSections>    
    // --        <section name="UnhandledException" type="System.Configuration.NameValueSectionHandler, System, 
    // --        Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
    // -- </configSections>
    // --
    // -- <!-- settings for UnhandledExceptionManager -->
    // --    <UnhandledException>
    // --        <add key="ContactInfo" value="Ima Testguy at 123-555-1212" />
    // --        <add key="IgnoreDebug" value="False" />
    // --        <add key="IgnoreRegex" value="get_aspx_ver\.aspx" />
    // --        <add key="EmailTo" value="me@mydomain.com" />
    // --    </UnhandledException>   
    // --
    // -- Complete configuration options are..
    // --  
    // --  <!-- Handler defaults -->
    // --  <add key="AppName" value="" />
    // --  <add key="ContactInfo" value="" />
    // --  <add key="EmailTo" value="" />
    // --  <add key="LogToEventLog" value="False" />
    // --  <add key="LogToFile" value="True" />
    // --  <add key="LogToEmail" value="True" />
    // --  <add key="LogToUI" value="True" />
    // --     <add key="PathLogFile" value="" />
    // --  <add key="IgnoreRegex" value="" />
    // --  <add key="IgnoreDebug" value="True" />
    // --  <add key="WhatHappened" value="There was an unexpected error in this website. This may be due to a programming bug." />
    // --  <add key="HowUserAffected" value="The current page will not load." />
    // --  <add key="WhatUserCanDo" value="Close your browser, navigate back to this website, and try repeating your last action. Try alternative methods of performing the same action. If problems persist, contact (contact)." />
    // --
    // --  <!-- SMTP email configuration defaults -->
    // --  <add key="SmtpDefaultDomain" value="mydomain.com" />
    // --  <add key="SmtpServer" value="mail.mydomain.com" />
    // --  <add key="SmtpPort" value="25" />
    // --  <add key="SmtpAuthUser" value="" />
    // --  <add key="SmtpAuthPassword" value="" />
    // --
    // '' <summary>
    // '' Minimal class for retrieving typed values from the 
    // '' <UnhandledException> custom NameValueCollection .config section
    // '' </summary>
    public class Config 
    {
        private const string _strSectionName = "UnhandledException";
    
        private static NameValueCollection _nvc;
    
        private static void Load() {
            if (_nvc != null) 
            {
                return;
            }
        
            object o = null;

            try 
            {
                o = ConfigurationSettings.GetConfig(_strSectionName);

            }
            catch (Exception ex) 
            {
                // -- we are in an unhandled exception handler
            }

            if ((o == null)) 
            {
                // -- we can work without any configuration at all (all defaults)
                _nvc = new NameValueCollection();
                return;
            }
        
            try {
                _nvc = ((NameValueCollection)(o));

            }
            catch (Exception ex) {
                throw new ConfigurationException(
                    ("The <" + (_strSectionName + "> section is present in the .config file, but it does not appear to be a name value collection.")), 
                    ex);
            }
        }
    
        // '' <summary>
        // '' retrieves integer value from the 
        // '' <UnhandledException> custom NameValueCollection .config section
        // '' </summary>
        public static int GetInteger(string Key, 
                                     int Default) 
        {
            Load();

            var strTemp = _nvc[Key];

            if (string.IsNullOrWhiteSpace(strTemp) )
            {
                return Default;
            }
        
            try 
            {
                return Convert.ToInt32(strTemp);

            }
            catch (Exception ex) 
            {
                return Default;
            }
        }
    
        // '' <summary>
        // '' retrieves boolean value from the 
        // '' <UnhandledException> custom NameValueCollection .config section
        // '' </summary>
        public static bool GetBoolean(string Key, 
                                      bool? Default = null) {
            Load();

            // Warning!!! Optional parameters not supported
            var strTemp = _nvc[Key];

            if (string.IsNullOrWhiteSpace(strTemp)) 
            {
                return (Default.HasValue && Default.Value);
            }
        
            switch (strTemp.ToLower()) {
                case "1":
                case "true":
                    return true;

                default:
                    return false;
            }
        }
    
        // '' <summary>
        // '' retrieves string value from the 
        // '' <UnhandledException> custom NameValueCollection .config section
        // '' </summary>
        public static string GetString(string Key, 
                                       string Default = null) {
            Load();

            // Warning!!! Optional parameters not supported
            var strTemp = _nvc[Key];

            return (string.IsNullOrWhiteSpace(strTemp) ? Default : strTemp);
        }
    
        // '' <summary>
        // '' retrieves relative or absolute path string from the 
        // '' <UnhandledException> custom NameValueCollection .config section
        // '' </summary>
        public static string GetPath(string Key) {
            Load();

            var strPath = GetString(Key, string.Empty);

            // -- users might think we're using Server.MapPath, but we're not
            // -- strip this because it's unnecessary (we assume website root, if path isn't rooted)
            if (strPath.StartsWith("~/")) 
            {
                strPath = strPath.Replace("~/", string.Empty);
            }
        
            return (Path.IsPathRooted(strPath) ? strPath : Path.Combine(AppBase, strPath));
        }
    
        private static string AppBase 
        {
            get 
            {
                return Convert.ToString(AppDomain.CurrentDomain.GetData("APPBASE"));
            }
        }
    }
}