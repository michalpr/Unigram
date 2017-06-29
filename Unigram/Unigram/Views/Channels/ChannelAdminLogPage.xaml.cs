﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unigram.ViewModels.Channels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Unigram.Views.Channels
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChannelAdminLogPage : Page
    {
        public ChannelAdminLogViewModel ViewModel => DataContext as ChannelAdminLogViewModel;

        public ChannelAdminLogPage()
        {
            InitializeComponent();
            DataContext = UnigramContainer.Current.ResolveType<ChannelAdminLogViewModel>();
        }

        private void ProfileBubble_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Download_Click(object sender, Controls.TransferCompletedEventArgs e)
        {

        }

        private void Reply_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StickerSet_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
