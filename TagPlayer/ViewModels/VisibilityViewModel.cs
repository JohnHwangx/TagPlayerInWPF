using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace TagPlayer.ViewModels
{
    public class VisibilityViewModel : BindableBase
    {
        private bool _playVis;

        public bool PlayVis
        {
            get { return _playVis; }
            set
            {
                _playVis = value;
                RaisePropertyChanged("PlayVis");
            }
        }
        private bool _editVis;

        public bool EditVis
        {
            get { return _editVis; }
            set
            {
                _editVis = value;
                RaisePropertyChanged("EditVis");
            }
        }
        private bool _addVis;

        public bool AddVis
        {
            get { return _addVis; }
            set
            {
                _addVis = value;
                RaisePropertyChanged("AddVis");
            }
        }
        public DelegateCommand<ListBox> SongListMenuItemCommand { get; set; }
        private void OnSongListMenuItem(ListBox obj)
        {
            if (obj.SelectedItems.Count > 1)
            {
                EditVis = false;
                AddVis = true;
                PlayVis = true;
            }
            else if (obj.SelectedItems.Count == 1)
            {
                EditVis = true;
                AddVis = true;
                PlayVis = true;
            }
            else
            {
                EditVis = false;
                AddVis = false;
                PlayVis = false;
            }
        }
        public VisibilityViewModel()
        {
            SongListMenuItemCommand = new DelegateCommand<ListBox>(OnSongListMenuItem);
        }
    }
}
