﻿using Livet;
using Livet.Messaging;
using SRNicoNico.Models.NicoNicoViewer;
using SRNicoNico.Models.NicoNicoWrapper;
using SRNicoNico.ViewModels;
using SRNicoNico.Views.Contents.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SRNicoNico.LiveModels
{

    public class LiveViewModel : TabItemViewModel
    {

        //API結果
        #region VideoData変更通知プロパティ
        private LiveData _LiveData;

        public LiveData LiveData
        {
            get { return _LiveData; }
            set
            {
                if (_LiveData == value)
                    return;
                _LiveData = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        //各種シークバー関連の時間管理
        #region Time変更通知プロパティ
        private VideoTime _Time;

        public VideoTime Time
        {
            get { return _Time; }
            set
            {
                if (_Time == value)
                    return;
                _Time = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region BPS変更通知プロパティ
        private string _BPS;

        public string BPS
        {
            get { return _BPS; }
            set
            {
                if (_BPS == value)
                    return;
                _BPS = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsInitialized変更通知プロパティ
        private bool _IsInitialized;

        public bool IsInitialized
        {
            get { return _IsInitialized; }
            set
            {
                if (_IsInitialized == value)
                    return;
                _IsInitialized = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsPlaying変更通知プロパティ
        private bool _IsPlaying;

        public bool IsPlaying
        {
            get { return _IsPlaying; }
            set
            {
                if (_IsPlaying == value)
                    return;
                _IsPlaying = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region WebBrowser変更通知プロパティ
        private WebBrowser _WebBrowser;

        public WebBrowser WebBrowser
        {
            get { return _WebBrowser; }
            set
            {
                if (_WebBrowser == value)
                    return;
                _WebBrowser = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region CommentVisibility変更通知プロパティ
        private bool _CommentVisibility = false;

        public bool CommentVisibility
        {
            get { return _CommentVisibility; }
            set
            {
                if (_CommentVisibility == value)
                    return;
                _CommentVisibility = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        //フルスクリーンかどうか
        #region IsFullScreen変更通知プロパティ
        private bool _IsFullScreen;

        public bool IsFullScreen
        {
            get { return _IsFullScreen; }
            set
            {
                if (_IsFullScreen == value)
                    return;
                _IsFullScreen = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        //リピートするかどうか
        #region IsRepeat変更通知プロパティ
        private bool _IsRepeat;

        public bool IsRepeat
        {
            get { return _IsRepeat; }
            set
            {
                if (_IsRepeat == value)
                    return;
                _IsRepeat = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Volume変更通知プロパティ
        private int _Volume = -1;

        public int Volume
        {
            get { return _Volume; }
            set
            {
                if (_Volume == value)
                    return;
                _Volume = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region VideoFlash変更通知プロパティ UI要素だけどこればっかりは仕方ない
        private VideoFlash _VideoFlash;

        public VideoFlash VideoFlash
        {
            get { return _VideoFlash; }
            set
            {
                if (_VideoFlash == value)
                    return;
                _VideoFlash = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Video変更通知プロパティ UI要素 仕方ないんや・・・
        private VideoContent _Video;

        public VideoContent Video
        {
            get { return _Video; }
            set
            {
                if (_Video == value)
                    return;
                _Video = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region FullScreenWindow変更通知プロパティ UI要素 仕方ないんです
        private FullScreenWindow _FullScreenWindow;

        public FullScreenWindow FullScreenWindow
        {
            get { return _FullScreenWindow; }
            set
            {
                if (_FullScreenWindow == value)
                    return;
                _FullScreenWindow = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public string VideoInfoPlacement
        {
            get
            {

                return App.ViewModelRoot.Config.Video.VideoPlacement;
            }
            set { }
        }

        #region VideoUrl変更通知プロパティ
        private string _VideoUrl;

        public string VideoUrl
        {
            get { return _VideoUrl; }
            set
            {
                if (_VideoUrl == value)
                    return;
                _VideoUrl = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Liveid変更通知プロパティ
        private string _Liveid;

        public string Liveid
        {
            get { return _Liveid; }
            set
            {
                if (_Liveid == value)
                    return;
                _Liveid = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        public LiveViewModel(string liveUrl) : base(liveUrl.Substring(0/*30*/))
        {

            VideoUrl = liveUrl;
            Liveid = Name;
            App.ViewModelRoot.TabItems.Add(this);
            App.ViewModelRoot.SelectedTab = this;
            Initialize(liveUrl);
        }

        private void Initialize(string liveUrl)
        {

            IsActive = true;
            Task.Run(() => {
                
                LiveData = new LiveData();


                Status = "生放送情報取得中";
                //動画情報取得
                LiveData.ApiData = NicoNicoLive.GetLive(liveUrl);

                if (LiveData.ApiData.Status != "ok")
                {

                    //App.ViewModelRoot.Messenger.Raise(new TransitionMessage(typeof(Views.Contents.Video.PaidVideoDialog), this, TransitionMode.Modal));
                    return;
                }

                DispatcherHelper.UIDispatcher.BeginInvoke(new Action(() => {

                    while (VideoFlash == null)
                    {

                        Thread.Sleep(1);
                    }
                    
                    LiveData.VideoType = NicoNicoVideoType.RTMP;
                    WebBrowser.Source = new Uri(GetRTMPPlayerPath());

                }));
                IsActive = false;

                Time = new VideoTime();

                //動画時間
                //Time.VideoTimeString = NicoNicoUtil.GetTimeFromLong(LiveData.ApiData.Stream);

                NicoNicoComment comment = new NicoNicoComment(/*LiveData.ApiData*/null, /*this*/null);

                List<NicoNicoCommentEntry> list = comment.GetComment();

                /*
                if (list != null)
                {

                    foreach (NicoNicoCommentEntry entry in list)
                    {

                        VideoData.CommentData.Add(new CommentEntryViewModel(entry));
                    }

                    dynamic json = new DynamicJson();
                    json.array = list;

                    DispatcherHelper.UIDispatcher.BeginInvoke(new Action(() => InjectComment(json.ToString())));
                }

                if (!Properties.Settings.Default.CommentVisibility)
                {

                    DispatcherHelper.UIDispatcher.BeginInvoke(new Action(() => InvokeScript("JsToggleComment")));
                }
                else
                {

                    CommentVisibility = true;
                }*/
                //App.ViewModelRoot.StatusBar.Status = "動画取得完了";
            });
        }
        /*
        public void OpenBrowser()
        {

            System.Diagnostics.Process.Start(VideoUrl);
        }

        //リンクを開く
        public void OpenLink(string cmsid)
        {

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || VideoData.ApiData.IsPaidVideo)
            {

                System.Diagnostics.Process.Start(cmsid);
                return;
            }

            if (cmsid.StartsWith("http://www.nicovideo.jp/watch/"))
            {

                new VideoViewModel(cmsid);
                DisposeViewModel();
                return;
            }

            if (cmsid.StartsWith("http"))
            {

                System.Diagnostics.Process.Start(cmsid);
                return;
            }

        }

        //ツイートダイアログ表示
        public void OpenTweetDialog()
        {

            TweetDialogViewModel vm = new TweetDialogViewModel();
            string url = "https://twitter.com/intent/tweet?hashtags=" + VideoData.ApiData.Cmsid
                            + "&text=" + HttpUtility.UrlEncode(VideoData.ApiData.Title) + "(" + Time.VideoTimeString + ")&url=" + VideoUrl;
            Console.WriteLine(url);
            url = url.Replace(" ", "%20");
            System.Diagnostics.Process.Start(url);
            vm.OriginalUri = new Uri(url);
            //App.ViewModelRoot.Messenger.Raise(new TransitionMessage(typeof(Views.Contents.Misc.TweetDialog), vm, TransitionMode.Modal));
        }

        //リピート
        public void ToggleRepeat()
        {

            IsRepeat ^= true;
            Properties.Settings.Default.IsRepeat = IsRepeat;
            Properties.Settings.Default.Save();
        }

        public void ToggleComment()
        {

            CommentVisibility ^= true;
            Properties.Settings.Default.CommentVisibility = CommentVisibility;
            Properties.Settings.Default.Save();
            InvokeScript("JsToggleComment");
        }

        public void ToggleMute()
        {

            if (Volume == 0)
            {

                Volume = 100;
            }
            else
            {

                Volume = 0;
            }
            ChangeVolume(Volume);
        }

        //フルスクリーンにする
        public void GoToFullScreen()
        {

            if (IsFullScreen)
            {

                return;
            }
            IsFullScreen = true;

            //リソースに登録
            Application.Current.Resources["VideoFlashKey"] = VideoFlash;
            var message = new TransitionMessage(typeof(FullScreenWindow), this, TransitionMode.Modal);

            //ウィンドウからFlash部分を消去
            Video.Grid.Children.Remove(VideoFlash);

            //フルスクリーンウィンドウ表示
            Messenger.Raise(message);
        }

        //ウィンドウモードに戻す
        public void ReturnFromFullScreen()
        {

            if (!IsFullScreen)
            {

                return;
            }

            IsFullScreen = false;

            //Flash部分をフルスクリーンウィンドウから消去
            FullScreenWindow.ContentControl.Content = null;

            //ウィンドウを閉じる
            FullScreenWindow.Close();

            //ウィンドウにFlash部分を追加
            Video.Grid.Children.Add(VideoFlash);

        }

        //フルスクリーン切り替え
        public void ToggleFullScreen()
        {

            if (IsFullScreen)
            {

                ReturnFromFullScreen();
            }
            else
            {

                GoToFullScreen();
            }
        }

        //一時停止切り替え
        public void PlayOrPauseOrResume()
        {

            if (IsPlaying)
            {

                IsPlaying = false;
                Pause();
            }
            else
            {

                IsPlaying = true;
                Resume();
            }

        }

        //最初から
        public void Restart()
        {

            Seek(0);
        }

        //1フレーム毎に呼ばれる
        public void CsFrame(float time, float buffer, long bps)
        {


            double vpos = time * 100;
            vpos = Math.Floor(vpos);

            double comp = bps / 1024;

            //大きいから単位を変えましょう
            if (comp > 1024)
            {

                BPS = Math.Floor((comp / 1024) * 100) / 100 + "MiB/秒";
            }
            else
            {

                BPS = Math.Floor(comp * 100) / 100 + "KiB/秒";
            }


            Time.BufferedTime = buffer;

            //Console.WriteLine(VideoData.ApiData.Cmsid + " time:" + time + " buffer:" + Time.BufferedTime + " bps:" + bps);

            SetSeekCursor(time);

            if ((int)time == VideoData.ApiData.Length - 1)
            {

                if (IsRepeat)
                {

                    Seek(0);
                }
            }

            if ((int)time == VideoData.ApiData.Length)
            {

                if (IsFullScreen)
                {

                    ReturnFromFullScreen();
                }
            }
        }

        //指定した時間でシークバーを移動する
        private void SetSeekCursor(float time)
        {

            Time.CurrentTime = (int)time;
            Time.CurrentTimeString = NicoNicoUtil.GetTimeFromLong(Time.CurrentTime);
        }

        //このメソッド以降はWebBrowserプロパティはnullではない
        public void OpenVideo()
        {

            while (VideoData.ApiData == null)
            {

                Thread.Sleep(50);
            }

            //ここからInvoke可能
            WebBrowser.ObjectForScripting = new ObjectForScriptingHelper(this);
            IsPlaying = true;
            IsInitialized = true;
            Mylist.EnableButtons();

            Console.WriteLine("VideoUrl:" + VideoData.ApiData.GetFlv.VideoUrl);

            if (VideoData.VideoType == NicoNicoVideoType.RTMP)
            {

                InvokeScript("JsOpenVideo", VideoData.ApiData.GetFlv.VideoUrl + "^" + VideoData.ApiData.GetFlv.FmsToken);

            }
            else
            {

                InvokeScript("JsOpenVideo", VideoData.ApiData.GetFlv.VideoUrl);
            }


            Volume = Properties.Settings.Default.Volume;
            ChangeVolume(Volume);

            IsRepeat = Properties.Settings.Default.IsRepeat;

        }
        */

        //Flashに一時停止命令を送る
        public void Pause()
        {

            InvokeScript("JsPause");
        }

        //Flashに再生再開命令を送る
        public void Resume()
        {

            InvokeScript("JsResume");
        }

        //Flashにシーク命令を送る
        public void Seek(float pos)
        {

            InvokeScript("JsSeek", pos.ToString());
        }

        //Flashにコメントリストを送る
        public void InjectComment(string json)
        {

            InvokeScript("JsInjectComment", json);
        }

        public void ChangeVolume(int vol)
        {

            Properties.Settings.Default.Volume = vol;
            Properties.Settings.Default.Save();
            InvokeScript("JsChangeVolume", (vol / 100.0).ToString());
        }

        //JSを呼ぶ
        private void InvokeScript(string func, params string[] args)
        {


            //読み込み前にボタンを押しても大丈夫なように
            if (WebBrowser != null && WebBrowser.IsEnabled && WebBrowser.Source != null)
            {

                try
                {

                    WebBrowser.InvokeScript(func, args);

                }
                catch (COMException)
                {

                    Console.WriteLine("COMException：" + func);
                }
            }
        }
        
        private string GetRTMPPlayerPath()
        {

            var cur = System.IO.Directory.GetCurrentDirectory();
            return cur + "./Flash/NicoNicoRTMPPlayer.html";
        }
        
        //RTMP動画でタイムアウトになった時又は予期せぬ理由でエラーになった時
        public void RTMPTimeOut()
        {

            App.ViewModelRoot.Messenger.Raise(new TransitionMessage(typeof(Views.Contents.Video.VideoTimeOutDialog), this, TransitionMode.Modal));
        }

        /*
        public void Reflesh()
        {

            DisposeViewModel();
            new VideoViewModel(VideoUrl);
        }

        public void DisposeViewModel()
        {

            //ウェブブラウザ開放
            WebBrowser.Dispose();
            WebBrowser.IsEnabled = false;

            App.ViewModelRoot.TabItems.Remove(this);

            Dispose();
        }

        public override void KeyDown(KeyEventArgs e)
        {

            if (IsFullScreen)
            {

                switch (e.Key)
                {
                    case Key.Space:
                        PlayOrPauseOrResume();
                        break;
                    case Key.Escape:
                        ToggleFullScreen();
                        break;
                    case Key.Back:
                        Restart();
                        break;
                    case Key.C:
                        ToggleComment();
                        break;
                    case Key.R:
                        ToggleRepeat();
                        break;
                    case Key.M:
                        ToggleMute();
                        break;
                }
            }
            else
            {

                switch (e.Key)
                {
                    case Key.Space:
                        PlayOrPauseOrResume();
                        break;
                    case Key.F:
                        ToggleFullScreen();
                        break;
                    case Key.Back:
                        Restart();
                        break;
                    case Key.C:
                        ToggleComment();
                        break;
                    case Key.R:
                        ToggleRepeat();
                        break;
                    case Key.M:
                        ToggleMute();
                        break;
                    case Key.F5:
                        Reflesh();
                        break;
                }
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                {

                    if (e.Key == Key.W)
                    {

                        DisposeViewModel();
                    }
                }
            }
        }
        */

    }
}
