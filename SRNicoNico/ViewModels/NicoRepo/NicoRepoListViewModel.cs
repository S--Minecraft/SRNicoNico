﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using SRNicoNico.Models.NicoNicoWrapper;

namespace SRNicoNico.ViewModels {
    public class NicoRepoListViewModel : TabItemViewModel {


        private readonly string Id;



        #region Result変更通知プロパティ
        private NicoRepoResultViewModel _Result;

        public NicoRepoResultViewModel Result {
            get { return _Result; }
            set { 
                if(_Result == value)
                    return;
                _Result = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        private NicoNicoNicoRepo NicoRepo;

        private NicoRepoViewModel NicoRepoVM;

        public NicoRepoListViewModel(NicoRepoViewModel vm, string title, string id) : base(title) {

            NicoRepoVM = vm;
            Id = id;
            Result = new NicoRepoResultViewModel(title);
        }


        //ニコレポリストを開く
        public void OpenNicoRepoList() {

            DispatcherHelper.UIDispatcher.BeginInvoke(new Action(() => Result.IsActive = true));
           
            NicoRepoVM.Status = "ニコレポ取得中(" + Name + ")";

            Task.Run(() => {

                Result.OwnerViewModel = this;
                Result.NicoRepo.Clear();


                NicoRepo = new NicoNicoNicoRepo(Id);

                IList<NicoNicoNicoRepoDataEntry> data = NicoRepo.GetNicoRepo();

                foreach(NicoNicoNicoRepoDataEntry entry in data) {

                    Result.NicoRepo.Add(new NicoRepoResultEntryViewModel(entry, this));
                }

                NicoRepoVM.Status = "ニコレポ取得完了(" + Name + ")";

                Result.IsActive = false;
            });

        }

        public void NextNicoRepoList() {

            Result.IsActive = true;

            Task.Run(() => {

                IList<NicoNicoNicoRepoDataEntry> data = NicoRepo.NextNicoRepo();

                if(data == null) {

                    return;
                }

                foreach(NicoNicoNicoRepoDataEntry entry in data) {

                    Result.NicoRepo.Add(new NicoRepoResultEntryViewModel(entry, this));
                }

                Result.IsActive = false;
            });
        }

        public void Reflesh() {

            OpenNicoRepoList();
        }
    }
}
