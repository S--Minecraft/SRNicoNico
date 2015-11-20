using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using SRNicoNico.Models;

namespace SRNicoNico.ViewModels
{
    public class ReservationResultViewModel : ViewModel
    {
        
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

        #region List変更通知プロパティ
        private DispatcherCollection<ReservationResultEntryViewModel> _List = new DispatcherCollection<ReservationResultEntryViewModel>(DispatcherHelper.UIDispatcher);

        public DispatcherCollection<ReservationResultEntryViewModel> List
        {
            get { return _List; }
            set
            {
                if (_List == value)
                    return;
                _List = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region SelectedItem変更通知プロパティ
        private ReservationResultEntryViewModel _SelectedItem;

        public ReservationResultEntryViewModel SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                if (_SelectedItem == value)
                    return;
                _SelectedItem = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        //選択された動画を開く
        public void Open()
        {

            if (SelectedItem != null)
            {

                new VideoViewModel("http://live.nicovideo.jp/watch/" + SelectedItem.Data.Id);
                SelectedItem = null;
            }
        }

    }
}
