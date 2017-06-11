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
using TagPlayer.Model;

namespace TagPlayer.controls.Resource.Tags
{
    /// <summary>
    /// TagStyle.xaml 的交互逻辑
    /// </summary>
    public partial class TagStyle : UserControl
    {
        public TagStyle()
        {
            InitializeComponent();
            foreach (var button in TagButtonModel.GetButtonContent("StyleTags"))
            {
                WrapPanel.Children.Add(button);
            }
        }
    }
}
