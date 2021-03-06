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

namespace TagPlayer.controls
{
    /// <summary>
    /// TagsViewControl.xaml 的交互逻辑
    /// </summary>
    public partial class TagsViewControl : UserControl
    {
        public TagsViewControl()
        {
            InitializeComponent();
            ChangeWrapPanel(AddButton);
        }

        private void ChangeWrapPanel(Action<WrapPanel> action)
        {
            foreach (var child in StackPanelTags.Children)
            {
                if (child is GroupBox groupBox)
                {
                    var groupBoxChildren = groupBox.Content as UserControl;
                    if (groupBoxChildren != null)
                    {
                        var wrapPanel = groupBoxChildren.Content as WrapPanel;
                        if (wrapPanel != null)
                        {
                            action(wrapPanel);
                        }
                    }
                }
            }
        }

        private void AddButton(WrapPanel wrapPanel)
        {
            foreach (var button in TagButtonModel.Instance.GetButtonContent(wrapPanel.Name))
            {
                wrapPanel.Children.Add(button);
            }
        }

        private void ClearTags(WrapPanel wrapPanel)
        {
            foreach (var item in wrapPanel.Children)
            {
                var button = item as Button;
                if (button!=null)
                {
                    button.FontWeight = FontWeights.Normal;
                    button.Foreground = FindResource("ForegroundBrush") as SolidColorBrush;
                }
            }
        }

        public void ClearTags()
        {
            ChangeWrapPanel(ClearTags);
        }
    }
}
