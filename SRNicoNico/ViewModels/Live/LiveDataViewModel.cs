using Livet;
using SRNicoNico.Models.NicoNicoWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRNicoNico.ViewModels
{
    class LiveDataViewModel : ViewModel {

        #region Data変更通知プロパティ
        private NicoNicoLiveData _Data;

        public NicoNicoLiveData Data
        {
            get { return _Data; }
            set
            {
                if (_Data == value)
                    return;
                _Data = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsActive変更通知プロパティ
        private bool _IsActive;

        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                if (_IsActive == value)
                    return;
                _IsActive = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        public  LiveDataViewModel(string liveId)
        {

            Task.Run(() => {

                IsActive = true;

                Data = NicoNicoLive.GetLive(liveId);
                IsActive = false;
            });
        }

    }
}
