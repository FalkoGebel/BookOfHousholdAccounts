﻿using System.IO;
using System.Windows;

namespace BohaWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            Properties["PathFiles"] = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "BOHA");
        }
    }
}