﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using Tralus.Framework.Data;
using Tralus.Framework.Module;

namespace Tralus.Shell.Module.Utility
{
    public class ReflectionHelper
    {
        public static void GetImportedModules(out IEnumerable<Type> importedModules, out IEnumerable<Type> dbContexts)
        {
            var moduleInfosStrings = ConfigurationManager.AppSettings["ImportedModules"] ?? "";

            var tempImprtedModule = new List<Type>();
            var tempDbContext = new List<Type>();


            LoadModule("Tralus.Framework", tempImprtedModule, tempDbContext);

            foreach (var moduleInfosStr in moduleInfosStrings.Split(','))
            {
                if (string.IsNullOrWhiteSpace(moduleInfosStr))
                    continue;

                LoadModule(moduleInfosStr, tempImprtedModule, tempDbContext);
            }

            importedModules = tempImprtedModule;
            dbContexts = tempDbContext;
        }

        private static void LoadModule(string moduleInfosStr, List<Type> tempImprtedModule, List<Type> tempDbContext)
        {
            var moduleFullPath = moduleInfosStr + ".Module";
            var moduleAssembly = Assembly.Load(moduleFullPath);
            var dataFullPath = moduleInfosStr + ".Data";
            var dataAssembly = Assembly.Load(dataFullPath);

            var modules = moduleAssembly.GetTypes().Where(t => t.IsSubclassOf(typeof(TralusModule)));

            // ToDo: This should remove from here and move to a web related place. This project should not have any reference to web.

            //if (moduleInfosStr != "Tralus.Framework")
            //{
                Assembly moduleCustomAssembly;
                if (System.Web.HttpRuntime.AppDomainAppId != null)
                {
                    var moduleWwebFullPath = moduleInfosStr + ".Module.Web";
                    moduleCustomAssembly = Assembly.Load(moduleWwebFullPath);
                }
                else
                {
                    var moduleWinFullPath = moduleInfosStr + ".Module.Win";
                    moduleCustomAssembly = Assembly.Load(moduleWinFullPath);
                }

                var modulesSpecific = moduleCustomAssembly.GetTypes().Where(
                    t => t.IsSubclassOf(typeof(TralusModule)) && !t.FullName.Contains("TralusWinModule") && !t.FullName.Contains("TralusWebModule"));

                tempImprtedModule.AddRange(modules);
                tempImprtedModule.AddRange(modulesSpecific);
            //}

            tempDbContext.AddRange(dataAssembly.GetTypes().Where(
                t => t.IsSubclassOf(typeof(DbContextBase)) && !t.IsAbstract && !t.FullName.Contains("DbContextBase"))

                );
        }
    }
}
