﻿#pragma checksum "D:\Projects\GitHub\SkyDriveExample\TaskListDemo\TaskListDemo\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9B5645B700174AFF1E3F2D9E81506900"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Live.Controls;
using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace TaskListDemo {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.ListBox Tasks;
        
        internal System.Windows.Controls.Button NewTask;
        
        internal System.Windows.Controls.Button ClearTasks;
        
        internal System.Windows.Controls.Button ToSkyDrive;
        
        internal System.Windows.Controls.Button FromSkyDrive;
        
        internal Microsoft.Live.Controls.SignInButton signInButton;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/TaskListDemo;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.Tasks = ((System.Windows.Controls.ListBox)(this.FindName("Tasks")));
            this.NewTask = ((System.Windows.Controls.Button)(this.FindName("NewTask")));
            this.ClearTasks = ((System.Windows.Controls.Button)(this.FindName("ClearTasks")));
            this.ToSkyDrive = ((System.Windows.Controls.Button)(this.FindName("ToSkyDrive")));
            this.FromSkyDrive = ((System.Windows.Controls.Button)(this.FindName("FromSkyDrive")));
            this.signInButton = ((Microsoft.Live.Controls.SignInButton)(this.FindName("signInButton")));
        }
    }
}

