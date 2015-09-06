﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.IO;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using SRNicoNico.Views.Contents;
using SRNicoNico.Views.Contents.Search;
using SRNicoNico.Models;
using SRNicoNico.Models.NicoNicoWrapper;
using System.Windows;
using System.Collections.ObjectModel;

namespace SRNicoNico.ViewModels {
	public class MainWindowViewModel : ViewModel {


		#region Title変更通知プロパティ
		private string _Title = "NicoNicoViewer ";

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



        #region WindowState変更通知プロパティ
        private WindowState _WindowState;

        public WindowState WindowState {
            get { return _WindowState; }
            set { 
                if(_WindowState == value)
                    return;
                _WindowState = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        public SignInDialogViewModel SignIn { get; private set; }
		public SearchViewModel Search { get; private set; }

		public Dictionary<string, VideoViewModel> VideoMap { get; private set; }

        public NicoRepoViewModel NicoRepo { get; private set; }


        public OtherViewModel Other { get; set; }

        public StatusBarViewModel StatusBar { get; set; }

        public ObservableCollection<ViewModel> TabItems { get; set; }


        #region SelectedTab変更通知プロパティ
        private ViewModel _SelectedTab;

        public ViewModel SelectedTab {
            get { return _SelectedTab; }
            set { 
                if(_SelectedTab == value)
                    return;
                _SelectedTab = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        public MainWindowViewModel() {

			SignIn = new SignInDialogViewModel();

            TabItems = new ObservableCollection<ViewModel> {

                (SelectedTab = new StartViewModel()),
                (Search = new SearchViewModel()),
                (NicoRepo = new NicoRepoViewModel()),
                (new HistoryViewModel()),
                (Other = new OtherViewModel())
            };


			VideoMap = new Dictionary<string, VideoViewModel>();

            StatusBar = new StatusBarViewModel();

            StatusBar.TimerStart();
		}


		public void Initialize() {

            
			//Modelsを初期化
			Task.Run(() => {

				NicoNicoWrapperMain.Instance = new NicoNicoWrapperMain();


				if(File.Exists(NicoNicoUtil.CurrentDirectory.DirectoryName + @"\session")) {

                    StatusBar.Status = "自動ログイン中";

					//セッション情報を取得する
					StreamReader reader = new StreamReader(NicoNicoUtil.CurrentDirectory.DirectoryName + @"\session");
							
					string key = reader.ReadLine().Split(':')[1];
					DateTimeOffset expire = DateTimeOffset.Parse(reader.ReadLine().Replace("Expire:", ""));

					reader.Close();

					//セッションが有効か比較する
					if(DateTimeOffset.Compare(expire, DateTimeOffset.Now) < 0) {

						//セッションが有効期限切れ
						this.SignIn.StateText = "有効期限が切れています。\n再度ログインしてください。";
						this.SignIn.AutoLogin = true;

						//ログインダイアログ表示
						Messenger.Raise(new TransitionMessage(typeof(Views.SignInDialog), this.SignIn, TransitionMode.Modal));
						return;
					}


					//セッションが有効だった場合
					NicoNicoWrapperMain.Instance.Session = new NicoNicoSession(key, expire);
					if(NicoNicoWrapperMain.Instance.Session.SignInInternal() != SigninStatus.Success) {

						//ログイン失敗
						this.SignIn.StateText = "ログインに失敗しました。";
						this.SignIn.AutoLogin = true;

						//ログインダイアログ表示
						Messenger.Raise(new TransitionMessage(typeof(Views.SignInDialog), this.SignIn, TransitionMode.Modal));
						return;
					}

                    StatusBar.Status = "ログイン完了";
					//ログイン成功
					NicoNicoWrapperMain.Instance.PostInit();



				//手動ログイン
				} else {

					//セッションを確立
					NicoNicoWrapperMain.Instance.Session = new NicoNicoSession();
					Messenger.Raise(new TransitionMessage(typeof(Views.SignInDialog), this.SignIn, TransitionMode.Modal));
					return;
				}

			});
            
		}

		//終了処理
		protected override void Dispose(bool disposing) {

			if(disposing) {

				NicoNicoWrapperMain.GetSession().HttpHandler.Dispose();
				NicoNicoWrapperMain.GetSession().HttpClient.Dispose();



			}



			base.Dispose(disposing);
		}

	}
}
