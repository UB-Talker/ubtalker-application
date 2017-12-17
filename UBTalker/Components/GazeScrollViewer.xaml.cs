﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UBTalker.Components
{
    /// <summary>
    /// Interaction logic for GazeScrollViewer.xaml
    /// </summary>
    public partial class GazeScrollViewer : ScrollViewer
    {
        static GazeScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GazeScrollViewer), new FrameworkPropertyMetadata(typeof(GazeScrollViewer)));
        }

        public GazeScrollViewer()
        {
            InitializeComponent();
        }
    }
}