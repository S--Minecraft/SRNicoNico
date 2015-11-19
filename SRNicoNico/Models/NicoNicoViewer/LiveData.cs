using Livet;
using SRNicoNico.Models.NicoNicoWrapper;
using SRNicoNico.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRNicoNico.Models.NicoNicoViewer
{
    class LiveData : NotificationObject
    {

        //LiveAPIデータ
        #region ApiData変更通知プロパティ
        private NicoNicoLiveData _ApiData;

        public NicoNicoLiveData ApiData
        {
            get { return _ApiData; }
            set
            {
                if (_ApiData == value)
                    return;
                _ApiData = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        //ストーリーボードデータ
        //Todo:
        public NicoNicoStoryBoardData StoryBoardData { get; set; }

        //コメントデータ
        //Todo:
        #region CommentData変更通知プロパティ
        private ObservableCollection<CommentEntryViewModel> _CommentData = new ObservableCollection<CommentEntryViewModel>();

        public ObservableCollection<CommentEntryViewModel> CommentData
        {
            get { return _CommentData; }
            set
            {
                if (_CommentData == value)
                    return;
                _CommentData = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        //動画タイプ mp4とかswfとかrtmpとか
        //Todo:
        #region VideoType変更通知プロパティ
        private NicoNicoVideoType? _VideoType;

        public NicoNicoVideoType? VideoType
        {
            get { return _VideoType; }
            set
            {
                if (_VideoType == value)
                    return;
                _VideoType = value;
                RaisePropertyChanged();
            }
        }
        #endregion



    }
}
