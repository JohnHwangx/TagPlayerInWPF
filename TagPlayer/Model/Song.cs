/**
 * 歌曲类
 * **/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TagPlayer.Model
{
    public class Song
    {
        private bool _hasAlbumCover;

        [NonSerialized]
        private BitmapImage _albumCover;
        private int _star;
        public List<string> Tags { get; set; }

        public Song(string path,string title)
        {
            Path = path;
            Title = title;
        }

        public Song(string path)
        {
            var songModel = new SongModel();
            Path = path;
            Title = SongModel.GetInfo(21, path);
            if (Title.Equals(""))
            {
                Title = SongModel.GetFileName(path);
            }
            Artist = SongModel.GetInfo(20, path);
            if (Artist.Equals(""))
            {
                Artist = "未知作者";
            }
            Album = SongModel.GetInfo(14, path);
            if (Album.Equals(""))
            {
                Album = "未知专辑";
            }
            Duration = SongModel.GetInfo(27, path);
            AlbumCover = null;
            //_hasAlbumCover = false;
            Star = 0;
            Tags = null;
        }

        public Song()
        {
            Path = null;
            Title = null;
            Artist = null;
            Album = null;
            Duration = null;
            AlbumCover = null;
            //_hasAlbumCover = false;
            Star = 0;
            Tags = null;
        }

        public Song(Song song)
        {
            Path = song.Path;
            Title = song.Title;
            Artist = song.Artist;
            Album = song.Album;
            Duration = song.Duration;
            AlbumCover = song.AlbumCover;
            //_hasAlbumCover = song._hasAlbumCover;
            Star = song.Star;
            Tags = song.Tags;
        }

        public Song(string path, string title, string artist, string album, string duration)
        {
            Path = path;
            Title = title;
            Artist = artist;
            Album = album;
            Duration = duration;
            _albumCover = null;
            //_hasAlbumCover = false;
            _star = 0;
            Tags = null;
        }

        public string Path { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string Album { get; set; }

        public string Duration { get; set; }

        public int Star
        {
            get { return _star; }
            set
            {
                if (value <= 5 && value >= 0)
                {
                    _star = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"Star", value, @"Star must be 0-5");
                }
            }
        }

        public BitmapImage AlbumCover
        {
            get
            {
                //var songModel = new SongModel();
                //if (_hasAlbumCover == false)
                //{
                //    _albumCover = SongModel.GetAlbumCover(Path);
                //    _hasAlbumCover = true;
                //    return _albumCover;
                //}
                return _albumCover;
            }
            set { _albumCover = value; }
        }

        public bool Equals(Song song)
        {
            if (Path.Equals(song.Path) &&
                Title.Equals(song.Title) &&
                Artist.Equals(song.Artist) &&
                Album.Equals(song.Album) &&
                Duration.Equals(song.Duration))
            {
                return true;
            }
            return false;
        }

        public void LoadAlbum()
        {
            if (AlbumCover == null)
            {
                AlbumCover = SongModel.GetAlbumCover(Path);
            }
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Song;
            if (other == null)
            {
                return false;
            }
            if (string.Equals(Path, other.Path))
            {
                return true;
            }
            return false;
        }
    }
}
