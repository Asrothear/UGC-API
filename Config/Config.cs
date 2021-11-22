﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using UGC_API.Models;

namespace UGC_API.Config
{
    internal static class Configs
    {
        public static string[] Systems { get; set; }
        public static string[] Events { get; set; }
        public static int UpdateSystems { get; set; }

        internal static ConfigModel Values = new();
        public static void ReadConfig()
        {
            string cfgFile = @".\Config.json";
            string path = "";
            if (File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), cfgFile)))
            {
                path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), cfgFile);
            }
            else
            {
                File.WriteAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), cfgFile), JsonSerializer.Serialize(new ConfigModel()));
                path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), cfgFile);
                Console.WriteLine($"Neue Konfig in {path} erstellt. Bitte diese anpassen!!");
                Thread.Sleep(5000);
                System.Environment.Exit(0);
            }
            Values = JsonSerializer.Deserialize<ConfigModel>(System.IO.File.ReadAllText(path));
        }
    }
}
