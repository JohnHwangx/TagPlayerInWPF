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
    public class TagButtonModel:Window
    {
        public static List<Button> GetButtonContent(string categoryName)
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
        private static Button CreateButton(string content, int count)
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
            button.SetBinding(ButtonBase.CommandProperty, new Binding("SongListService.SelectTagCommand"));
            button.SetBinding(ButtonBase.CommandParameterProperty, new Binding { Source = button });
            return button;
        }
    }
}
