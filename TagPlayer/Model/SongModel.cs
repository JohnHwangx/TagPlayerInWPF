using ID3;
using ID3.ID3v2Frames.BinaryFrames;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using TagPlayer.controls;
using TagPlayer.ViewModels;

namespace TagPlayer.Model
{
    public class SongModel
    {
        /// <summary>
        /// 获取歌曲封面图片
        /// </summary>
        /// <param name="path">歌曲文件路径</param>
        /// <returns>图片文件</returns>
        public static BitmapImage GetAlbumCover(string path)
        {
            if (path != null)
            {
                try
                {
                    ID3Info info = new ID3Info(path, true);
                    var pictureFrames = info.ID3v2Info.AttachedPictureFrames.Items;
                    BitmapImage bitmapImage = new BitmapImage();
                    if (pictureFrames.Any())
                    {
                        AttachedPictureFrame ap = info.ID3v2Info.AttachedPictureFrames.Items[0];
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = ap.Data;
                        bitmapImage.EndInit();
                    }
                    else
                    {
                        bitmapImage = new BitmapImage(new Uri(@"/TagPlayerInWPF;component/Image/DefaultImage.png", UriKind.RelativeOrAbsolute));
                    }
                    return bitmapImage;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return null;
                }
            }
            return null;
        }

        //[STAThread]
        /// <summary>
        /// 获取歌曲信息
        /// </summary>
        /// <param name="flag">信息编号</param>
        /// <param name="path">歌曲文件路径</param>
        /// <returns>歌曲信息</returns>
        public static string GetInfo(int flag, string path)
        {
            Shell32.Shell shell = new Shell32.Shell();
            try
            {
                Shell32.Folder shellFolder = shell.NameSpace(Path.GetDirectoryName(path));
                Shell32.FolderItem folderItem = shellFolder.ParseName(Path.GetFileName(path));

                string info = shellFolder.GetDetailsOf(folderItem, flag);
                return info;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取歌曲名
        /// </summary>
        /// <param name="file">歌曲文件名</param>
        /// <returns>歌曲名</returns>
        public static string GetFileName(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            return fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf(".", StringComparison.Ordinal));
        }

        public static void EditSongTags(Selector listBox)
        {
            var songListItem = listBox.SelectedItem as SongListItem;
            if (songListItem == null) return;
            var selectedSong = songListItem.Song;

            var tagsEditViewModel = new TagsEditViewModel(selectedSong);
            var tagEditWindow = new TagsEditingWindow()
            {
                DataContext = tagsEditViewModel
            };
            tagEditWindow.ShowDialog();

            if (tagEditWindow.DialogResult != true) return;
        }
    }
}
