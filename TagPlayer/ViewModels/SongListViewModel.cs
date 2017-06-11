using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using TagPlayer.Model;
using System.Collections.ObjectModel;

namespace TagPlayer.ViewModels
{
    public class SongListViewModel : BindableBase
    {
        public SongListModel SongListModel { get; set; } = new SongListModel();

        private List<SongListItem> _disSongList;

        public List<SongListItem> DisSongList
        {
            get { return _disSongList; }
            set
            {
                _disSongList = value;
                RaisePropertyChanged("DisSongList");
            }
        }

        public void InitialSongList(List<Song> songList)
        {
            DisSongList = SongListModel.InitialSongList(songList);
        }

        public SongListViewModel()
        {
            DisSongList = new List<SongListItem>();
        }
    }
}
