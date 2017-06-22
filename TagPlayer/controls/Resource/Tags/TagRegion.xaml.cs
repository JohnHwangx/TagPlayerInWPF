using System;
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
using TagPlayer.Model;
using System.Windows.Shapes;

namespace TagPlayer.controls.Resource.Tags
{
    /// <summary>
    /// TagRegion.xaml 的交互逻辑
    /// </summary>
    public partial class TagRegion : UserControl
    {
        public TagRegion()
        {
            InitializeComponent();
            //foreach (var button in TagButtonModel.Instance.GetButtonContent("RegionTags"))
            //{
            //    WrapPanel.Children.Add(button);
            //}
        }
    }
}
