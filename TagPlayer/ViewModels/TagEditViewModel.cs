using Prism.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TagPlayer.Model;

namespace TagPlayer.ViewModels
{
    public class TagEditViewModel : Window
    {
        public List<string> SongTags { get; set; }

        public DelegateCommand<Window> CloseWindowCommand { get; set; }

        private void CloseWindowExecute(Window window)
        {
            window.Close();
        }
        public DelegateCommand<Window> ClearTagsCommand { get; set; }
        private void ClearTagsExecute(Window window)
        {
            CheckTag(GetStackPanel(window).Children, false);
            SongTags.Clear();
            //TagsList.Clear();
        }
        public DelegateCommand<Window> EditTagsCommand { get; set; }
        private void EditTagsExecute(Window window)
        {
            window.DialogResult = true;
            window.Close();
        }
        public DelegateCommand<Window> WindowLoadedCommand { get; set; }
        private void WindowLoadedExecute(Window window)
        {
            CheckTag(GetStackPanel(window).Children, true);
        }
        private StackPanel GetStackPanel(Window window)
        {
            var grid = window.Content as Grid;
            if (grid == null) return null;
            var children = grid.Children;
            foreach (var child in children)
            {
                var userControl = (child as ScrollViewer)?.Content as UserControl;
                var stackPanel = userControl?.Content as StackPanel;
                if (stackPanel != null)
                    return stackPanel;
            }
            return null;
        }
        private void CheckTag(IEnumerable uiControls, bool flag)
        {
            foreach (UIElement element in uiControls)
            {
                if (element is Button)
                {
                    Button temp = element as Button;
                    if (SongTags == null) continue;
                    foreach (var tag in SongTags)
                    {
                        if (tag != temp.Content.ToString()) continue;
                        if (flag)
                        {
                            temp.FontWeight = FontWeights.Bold;
                            var resourceDictionary = new ResourceDictionary
                            {
                                Source = new Uri(@"controls/Resource/TagViewsResource.xaml",
                                    UriKind.RelativeOrAbsolute)
                            };
                            temp.Foreground = resourceDictionary["MouseOverBrush"] as SolidColorBrush;
                        }
                        else
                        {
                            temp.FontWeight = FontWeights.Normal;
                            var resourceDictionary = new ResourceDictionary
                            {
                                Source = new Uri(@"controls/Resource/TagViewsResource.xaml",
                                    UriKind.RelativeOrAbsolute)
                            };
                            temp.Foreground = resourceDictionary["ForegroundBrush"] as SolidColorBrush;
                        }
                    }
                }
                else if (element is GroupBox)
                {
                    var userControl = (element as GroupBox).Content as UserControl;
                    var wrapPanel = userControl?.Content as WrapPanel;
                    if (wrapPanel != null)
                        CheckTag(wrapPanel.Children, flag);
                }
            }
        }

        public TagEditViewModel(Song song)
        {
            SongTags = song.Tags;
            CloseWindowCommand = new DelegateCommand<Window>(CloseWindowExecute);
            ClearTagsCommand = new DelegateCommand<Window>(ClearTagsExecute);
            EditTagsCommand = new DelegateCommand<Window>(EditTagsExecute);
            WindowLoadedCommand = new DelegateCommand<Window>(WindowLoadedExecute);
        }
    }
}
