﻿using System;
using System.Collections.Generic;


using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MetroRadiance.Controls;
using SRNicoNico.ViewModels;
using System.Windows.Markup;
using System.Threading;

namespace SRNicoNico.Views {

	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : MetroWindow {
		public MainWindow() {
			InitializeComponent();
            Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
        }

        private void Root_KeyDown(object sender, KeyEventArgs e) {
            
            if(DataContext is MainWindowViewModel) {

                var vm = (MainWindowViewModel)DataContext;

                vm.KeyDown(e);

            }
        }
    }
}
