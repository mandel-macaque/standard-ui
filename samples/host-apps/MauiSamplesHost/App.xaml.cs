﻿using Microsoft.Maui.Controls;

namespace MauiSamplesHost
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}