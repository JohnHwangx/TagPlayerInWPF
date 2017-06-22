using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TagPlayer.Model;

namespace TagPlayer.ViewModels
{
    public class TagsEditViewModel
    {
        public List<string> SongTags { get; set; }
        /// <summary>
        /// 取消
        /// </summary>
        public DelegateCommand<Window> CancelCommand { get; set; }

        private void CancelExecute(Window window)
        {
            window.Close();
        }
        /// <summary>
        /// 清空
        /// </summary>
        public DelegateCommand<Window> ClearTagsCommand { get; set; }
        private void ClearTagsExecute(Window window)
        {
            //CheckTag(GetStackPanel(window).Children, false);
            SongTags.Clear();
        }
        /// <summary>
        /// 确定
        /// </summary>
        public DelegateCommand<Window> SureCommand { get; set; }
        private void SureExecute(Window window)
        {
            window.DialogResult = true;
            window.Close();
        }

        public DelegateCommand<Button> SelectTagsCommand { get; set; }

        private void OnSelectTags(Button button)
        {
            //TagButtonModel.Instance.SongTags = SongTags;
            TagButtonModel.Instance.SetTagModel(ref button);
        }
        public TagsEditViewModel(Song song)
        {
            TagButtonModel.Instance.SongTags = song.Tags;
            SelectTagsCommand = new DelegateCommand<Button>(OnSelectTags);
            ClearTagsCommand = new DelegateCommand<Window>(ClearTagsExecute);
            SureCommand = new DelegateCommand<Window>(SureExecute);
            CancelCommand = new DelegateCommand<Window>(CancelExecute);
        }
    }
}
