﻿// FFXIVAPP
// ThemeHelper.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Linq;
using MahApps.Metro;
using NLog;

namespace FFXIVAPP.Classes.Helpers
{
    internal static class ThemeHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// </summary>
        /// <param name="t"> </param>
        public static void ChangeTheme(string t)
        {
            try
            {
                var split = t.Split('|');
                var accent = split[0];
                var theme = split[1];
                switch (theme)
                {
                    case "Dark":
                        ThemeManager.ChangeTheme(MainWindow.View, ThemeManager.DefaultAccents.First(a => a.Name == accent), Theme.Dark);
                        break;
                    case "Light":
                        ThemeManager.ChangeTheme(MainWindow.View, ThemeManager.DefaultAccents.First(a => a.Name == accent), Theme.Light);
                        break;
                }
            }
            catch
            {
            }
        }
    }
}