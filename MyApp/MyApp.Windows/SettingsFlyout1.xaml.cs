﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace MyApp
{
    public sealed partial class SettingsFlyout1 : SettingsFlyout
    {
        public SettingsFlyout1()
        {
            this.InitializeComponent();
            this.Title = MainPage.resource.GetString("001_PrivacyString");
        }

        // Button Support
        private async void BtnSupport(object sender, PointerRoutedEventArgs e)
        {
            //Support Nachricht
            await Windows.System.Launcher.LaunchUriAsync(new Uri("mailto:xtrose@hotmail.com"));
        }
    }
}
