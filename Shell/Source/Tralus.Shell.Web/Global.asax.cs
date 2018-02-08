using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web.Configuration;
using System.Web;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Web;
using DevExpress.Web;
using Tralus.Framework.BusinessModel.Entities;
using Tralus.Shell.Module.Security;
using Tralus.Shell.Web.Security;
using ReflectionHelper = Tralus.Shell.Module.Utility.ReflectionHelper;

namespace Tralus.Shell.Web
{
    public class Global : System.Web.HttpApplication
    {
        public static IEnumerable<Type> LoadedModuleTypes;
        public static IEnumerable<Type> LoadedContextTypes;

        public static bool? _isRightToLeft = null;
        public static bool IsRightToLeft()
        {
            if (!_isRightToLeft.HasValue)
            {
                var layoutDirection = ConfigurationManager.AppSettings["LayoutDirection"];
                _isRightToLeft = layoutDirection?.ToLower() == "rtl";
            }
            return _isRightToLeft.Value;
        }

        static Global()
        {
            LoadMOdules();
        }

        private static void LoadMOdules()
        {
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    var location = Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath,
                        args.Name.Split(',')[0] + ".dll");
                    if (File.Exists(location))
                    {
                        var assembly = Assembly.LoadFrom(location);
                        return assembly;
                    }
                    return null;
                };

                ReflectionHelper.GetImportedModules(out LoadedModuleTypes, out LoadedContextTypes);
                //foreach (var loadedModuleType in LoadedModuleTypes)
                //{
                //    var loadedModule = (ModuleBase)Activator.CreateInstance(loadedModuleType);
                //    Modules.Insert(0, loadedModule);
                //}
            }
            catch (Exception exception)
            {
                Trace.WriteLine($"Unable to load modules: {exception}");
            }
        }

        public Global()
        {
            InitializeComponent();
        }
        protected void Application_Start(Object sender, EventArgs e)
        {
            ASPxWebControl.CallbackError += new EventHandler(Application_Error);
#if EASYTEST
            DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = true;
#endif
        }
        protected void Session_Start(Object sender, EventArgs e)
        {
            var webApplication = new ShellAspNetApplication();
            ConfigSecurity(webApplication);

            WebApplication.SetInstance(Session, webApplication);

            if (ConfigurationManager.AppSettings["ApplicationStyle"] == "New")
                WebApplication.Instance.SwitchToNewStyle();

            if (ConfigurationManager.ConnectionStrings["Default"] != null)
            {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            }
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif

            //WebApplication.Instance.Security = new TralusSecurityStrategy(new AdfsAuthentication());
            WebApplication.Instance.Setup();
            WebApplication.Instance.Start();
        }

        private static void ConfigSecurity(XafApplication application)
        {
            //if (application.Security != null)
            //    ((SecurityStrategy)winApplication.Security).CustomizeRequestProcessors +=
            //        (sender, e) => e.Processors.Add(typeof (BusinessLogicPermissionRequest<>), new BusinessLogicPermissionRequestProcessor(e.Permissions));

            var authenticationModeString = ConfigurationManager.AppSettings["AuthenticationMode"];
            var createUserAutomaticallyString = ConfigurationManager.AppSettings["CreateUserAutomatically"];
            var createUserAutomatically = createUserAutomaticallyString.ToLower() == "true";

            if (string.IsNullOrWhiteSpace(authenticationModeString))
            {
                throw new Exception("No AuthenticationMode specified at configuration. In app.config or web.config, there should be a 'AuthenticationMode' key in appSettings.");
            }

            AuthenticationBase authentication;

            switch (authenticationModeString)
            {
                case "ActiveDirectory":
                    authentication = new ActiveDirectoryAuthentication() { CreateUserAutomatically = createUserAutomatically };
                    break;

                case "ADFS":
                    authentication = new AdfsAuthentication() { CreateUserAutomatically = createUserAutomatically };
                    break;

                case "Tralus":
                    authentication = new AuthenticationStandard<User, AuthenticationStandardLogonParameters>();
                    break;

                case "None":
                    authentication = (TralusAuthenticationBase)null;
                    break;

                default:
                    throw new Exception($"AuthenticationMode is not supported: '{authenticationModeString}'");
            }

            if (authentication != null)
            {
                application.Security = new TralusSecurityStrategy(authentication);
            }

        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
        }
        protected void Application_EndRequest(Object sender, EventArgs e)
        {
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
        }
        protected void Application_Error(Object sender, EventArgs e)
        {
            ErrorHandling.Instance.ProcessApplicationError();
        }
        protected void Session_End(Object sender, EventArgs e)
        {
            WebApplication.LogOff(Session);
            WebApplication.DisposeInstance(Session);
        }
        protected void Application_End(Object sender, EventArgs e)
        {
        }
        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion
    }


}
