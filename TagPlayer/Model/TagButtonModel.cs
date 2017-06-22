using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Linq;

namespace TagPlayer.Model
{
    public class TagButtonModel : Window
    {
        private static TagButtonModel _instance;

        public static TagButtonModel Instance
        {
            get { return _instance??(_instance=new TagButtonModel()); }
        }

        private TagButtonModel()
        {
            SongTags = new List<string>();
        }
        public List<Button> GetButtonContent(string categoryName)
        {
            var path = @"..\..\Image\SongTags.xml";
            var xDoc = XDocument.Load(path.ToString());
            var tags = xDoc.Descendants(categoryName);
            var buttonList = new List<Button>();
            var count = 0;
            foreach (var category in tags)
            {
                //tagList.AddRange(category.Elements().Select(tag => tag.Value));
                foreach (var xElement in category.Elements())
                {
                    buttonList.Add(CreateButton(xElement.Value, count));
                    count++;
                }
            }
            return buttonList;
        }
        private Button CreateButton(string content, int count)
        {
            var window = new Window();
            var button = new Button
            {
                Name = "btn" + count,
                Content = content,
                Width = 70,
                Height = 20,
                Background = new SolidColorBrush(Colors.Transparent),
                BorderThickness = new Thickness(0),
                Foreground = window.FindResource("ForegroundBrush") as SolidColorBrush,
                Template = window.FindResource("TagsButtonTemplate") as ControlTemplate,
                //TODO 添加Command和CommandParameter
            };
            button.SetBinding(ButtonBase.CommandProperty, new Binding("TagsPanelViewModel.SelectTagCommand"));
            button.SetBinding(ButtonBase.CommandParameterProperty, new Binding { Source = button });
            return button;
        }

        public List<string> SongTags { get; set; }

        public void SetTagModel(ref Button button)
        {
            if (button.FontWeight == FontWeights.Normal)
            {
                button.FontWeight = FontWeights.Bold;
                button.Foreground = FindResource("MouseOverBrush") as SolidColorBrush;
                SongTags.Add(button.Content.ToString());
            }
            else if (button.FontWeight == FontWeights.Bold)
            {
                button.FontWeight = FontWeights.Normal;
                button.Foreground = FindResource("ForegroundBrush") as SolidColorBrush;
                SongTags.RemoveAt(SongTags.IndexOf(button.Content.ToString()));
            }
        }
    }
}
