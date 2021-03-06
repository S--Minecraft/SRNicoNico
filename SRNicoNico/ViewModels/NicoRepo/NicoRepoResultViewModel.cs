﻿using System;
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

using SRNicoNico.Models.NicoNicoWrapper;
using SRNicoNico.Models.NicoNicoViewer;

namespace SRNicoNico.ViewModels {
    public class NicoRepoResultViewModel : ViewModel {

        #region IsActive変更通知プロパティ
        private bool _IsActive;

        public bool IsActive {
            get { return _IsActive; }
            set { 
                if(_IsActive == value)
                    return;
                _IsActive = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region OwnerViewModel変更通知プロパティ
        private ViewModel _OwnerViewModel;

        public ViewModel OwnerViewModel {
            get { return _OwnerViewModel; }
            set { 
                if(_OwnerViewModel == value)
                    return;
                _OwnerViewModel = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region NicoRepo変更通知プロパティ
        private DispatcherCollection<NicoRepoResultEntryViewModel> _NicoRepo = new DispatcherCollection<NicoRepoResultEntryViewModel>(DispatcherHelper.UIDispatcher);

        public DispatcherCollection<NicoRepoResultEntryViewModel> NicoRepo {
            get { return _NicoRepo; }
            set { 
                if(_NicoRepo == value)
                    return;
                _NicoRepo = value;
                RaisePropertyChanged();
            }
        }
		#endregion

		#region SelectedItem変更通知プロパティ
		private NicoRepoResultEntryViewModel _SelectedItem;

		public NicoRepoResultEntryViewModel SelectedItem {
			get { return _SelectedItem; }
			set { 
				if(_SelectedItem == value)
					return;
				_SelectedItem = value;
				RaisePropertyChanged();
			}
		}
        #endregion

        #region Title変更通知プロパティ
        private string _Title;

        public string Title {
            get { return _Title; }
            set { 
                if(_Title == value)
                    return;
                _Title = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public NicoRepoResultViewModel(string title) {

            Title = title;
        }

        public void Open() {

            //not existsの時など
            if(SelectedItem == null || SelectedItem.Entry.VideoUrl == null) {

                SelectedItem = null;
                return;
            }

            //URLに応じて開くものを変える
            NicoNicoOpener.Open(SelectedItem.Entry.VideoUrl);

            SelectedItem = null;
        }


        
    }
}
