using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TagPlayer.controls
{
    /// <summary>
    /// TagsEditingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TagsEditingWindow : Window
    {
        public TagsEditingWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //Songs.SongTags = new List<string>();
            var tag = (Button)e.OriginalSource;
            if (tag.FontWeight == FontWeights.Normal)
            {
                tag.FontWeight = FontWeights.Bold;
                tag.Foreground = FindResource("MouseOverBrush") as SolidColorBrush;
                //Songs.SongTags.Add(tag.Content.ToString());
            }
            else if (tag.FontWeight == FontWeights.Bold)
            {
                tag.FontWeight = FontWeights.Normal;
                tag.Foreground = FindResource("ForegroundBrush") as SolidColorBrush;
                //Songs.SongTags.RemoveAt(Songs.SongTags.IndexOf(tag.Content.ToString()));
            }
        }
    }
}
