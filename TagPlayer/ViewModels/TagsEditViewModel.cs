using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TagPlayer.controls;
using TagPlayer.Model;

namespace TagPlayer.ViewModels
{
    public class TagsEditViewModel
    {
        public Song SelectedSong { get; set; }
        //public List<string> SongTags { get; set; }
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
            TagButtonModel.Instance.ClearTags(window);
        }
        /// <summary>
        /// 确定
        /// </summary>
        public DelegateCommand<Window> SureCommand { get; set; }
        private void SureExecute(Window window)
        {
            window.DialogResult = true;
            SongListModel.Instance.ClearSongTags(SelectedSong);//将数据库中该歌曲的标签清空
            SelectedSong.Tags = new List<string>(TagButtonModel.Instance.SongTags);
            SongListModel.Instance.SaveSongTags(SelectedSong);
            window.Close();
        }

        public DelegateCommand<Button> SelectTagsCommand { get; set; }

        private void OnSelectTags(Button button)
        {
            TagButtonModel.Instance.SetTagModel(ref button, TagsType.SongTags);
        }
        public TagsEditViewModel(Song song)
        {
            SelectedSong = song;
            TagButtonModel.Instance.SongTags = new List<string>(song.Tags);
            SelectTagsCommand = new DelegateCommand<Button>(OnSelectTags);
            ClearTagsCommand = new DelegateCommand<Window>(ClearTagsExecute);
            SureCommand = new DelegateCommand<Window>(SureExecute);
            CancelCommand = new DelegateCommand<Window>(CancelExecute);
        }
    }
}
