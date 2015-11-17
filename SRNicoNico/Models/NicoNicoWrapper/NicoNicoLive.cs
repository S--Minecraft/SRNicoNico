using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using Codeplex.Data;

namespace SRNicoNico.Models.NicoNicoWrapper
{
    public class NicoNicoLive : NotificationObject
    {
        //ニコ生取得API
        public const string LiveApiURL = "http://watch.live.nicovideo.jp/api/getplayerstatus?v=";

        public static NicoNicoLiveData GetLive(string liveId)
        {
            string uri = LiveApiURL + liveId;

            var result = NicoNicoUtil.XmlToJson(NicoNicoWrapperMain.GetSession().GetAsync(uri).Result);
            var json = DynamicJson.Parse(result);
            var response = json.nicolive_video_response;

            NicoNicoLiveData ret = new NicoNicoLiveData();

            ret.Status = response.status;
            if (ret.Status == "ok")
            {
                ret.Stream = ParseStreamData(response.stream);

                ret.User = ParseUserData(response.user);

                ret.RTMPIsFms = response.rtmp.is_fms;
                ret.RTMPPort = response.rtmp.rtmpt_port;
                ret.RTMPUrl = response.rtmp.url;
                ret.RTMPTicket = response.rtmp.ticket;

                ret.MSAddr = response.ms.addr;
                ret.MSPort = response.ms.port;
                ret.MSThread = response.ms.thread;

                ret.TidList = response.tid_list;

                ret.TwitterIsLiveEnabled = response.twitter.live_enabled;
                ret.TwitterVipModeCount = response.twitter.vip_mode_count;
                ret.TwitterLiveApiUrl = response.twitter.live_api_url;

                ret.PlayerQOSAnalytics = response.player.qos_analytics;
                ret.PlayerDialogOidashiUrl = response.player.dialog_image.oidashi;
                ret.PlayerIsNoticeViewerBalloonEnbled = response.player.is_notice_viewer_ballon_enabled;
                ret.PlayerErrorReport = response.player.error_report;

                ret.MarqueeCategory = response.marquee.category;
                ret.MarqueeGameKey = response.marquee.game_key;
                ret.MarqueeGameTime = response.marquee.game_time;
                ret.MarqueeIsForceNicowariOff = response.marquee.force_nicowari_off;
            }
            else
            {
                ret.Error = response.error.code;
            }

            return ret;
        }

        public static NicoNicoLiveStreamData ParseStreamData(dynamic res)
        {
            NicoNicoLiveStreamData ret = new NicoNicoLiveStreamData();

            ret.Id = res.id;
            ret.Title = res.title;
            ret.Description = res.description;
            ret.ProviderType = res.provider_type;
            ret.DefaultCommunity = res.default_community;
            ret.International = res.international;
            ret.IsOwner = res.is_owner;
            ret.OwnerId = res.owner_id;
            ret.OwnerName = res.owner_name;
            ret.IsReserved = res.is_reserved;
            ret.IsNicoNicoEnqueteEnbled = res.is_niconico_enquete_enabled;
            ret.WatchCount = res.watch_count;
            ret.CommentCount = res.comment_count;
            ret.BaseTime = res.base_time;
            ret.OpenTime = res.open_time;
            ret.StartTime = res.start_time;
            ret.EndTime = res.end_time;
            //ret.HqStream = res.title;
            ret.IsRerunStream = res.is_rerun_stream;
            //ret.IsArchivePlayServer = res.title;
            ret.BourbonUrl = res.bourbon_url;
            ret.FullVideo = res.full_video;
            ret.AfterVideo = res.after_video;
            ret.BeforeVideo = res.before_video;
            ret.KickoutVideo = res.kickout_video;
            ret.TwitterTag = res.twitter_tag;
            ret.DanjoCommentMode = res.danjo_comment_mode;
            //ret.Aspect  = res.title;
            ret.InfinityMode = res.infinity_mode;
            ret.Archive = res.archive;
            ret.PressDisplayLines = res.press.display_lines;
            ret.PressDisplayTime = res.press.display_time;
            ret.PressStyleConf = res.press.style_conf;
            ret.PluginDelay = res.plugin_delay;
            ret.PluginUrl = res.plugin_url;
            ret.PluginUrls = res.plugin_urls;
            ret.AllowNetduetto = res.allow_netduetto;
            ret.NdToken = res.nd_token;
            ret.NgScoring = res.ng_scoring;
            ret.IsNonArchiveTimeShiftEnbled = res.is_nonarchive_timeshift_enabled;
            ret.IsTimeShiftReserved = res.is_timeshift_reserved;
            ret.HeaderComment = res.header_comment;
            ret.FooterComment = res.footer_comment;
            ret.SplitBottom = res.split_bottom;
            ret.SplitTop = res.split_top;
            ret.BackgroundComment = res.background_comment;
            ret.FontScale = res.font_scale;
            ret.CommentLock = res.comment_lock;
            ret.Telop = res.telop.enable;

            List<NicoNicoLiveContent> contents = new List<NicoNicoLiveContent>();
            foreach (var content in res.contents_list)
            {
                NicoNicoLiveContent r = new NicoNicoLiveContent();

                r.Id = content.id;
                r.DisableAudio = content.disableAudio;
                r.DisableVideo = content.disableVideo;
                r.StartTime = content.start_time;
                r.Url = content;

                contents.Add(r);
            }
            ret.ContentsList = contents;
            /*
            List<NicoNicoLiveQueData> que = new List<NicoNicoLiveQueData>();
            foreach(var q in res.que)
            {

            }
            ret.QueList = que;
            */
            ret.PictureUrl = res.picture_url;
            ret.ThumbUrl = res.thumb_url;
            ret.IsPriotityPrefecture = res.is_priority_prefecture;

            return ret;
        }

        public static NicoNicoLiveUserData ParseUserData(dynamic res)
        {
            NicoNicoLiveUserData ret = new NicoNicoLiveUserData();

            ret.Id = res.user_id;
            ret.Name = res.nickname;
            ret.IsPremium = res.is_premium;
            ret.Age = res.userAge;
            ret.Sex = res.userSex;
            ret.Domain = res.userDomain;
            ret.Prefecture = res.userPrefecture;
            ret.Language = res.userLanguage;
            ret.RoomLabel = res.room_label;
            ret.SeetNo = res.room_seetno;
            ret.IsJoin = res.is_join;
            ret.TwitterStatus = res.twitter.status;
            ret.TwitterScreenName = res.twitter.screen_name;
            ret.TwitterFollowersCount = res.twitter.followers_count;
            ret.TwitterIsVip = res.twitter.is_vip;
            ret.TwitterProfileImgUrl = res.twitter.profile_image_url;
            ret.TwitterAfterAuth = res.twitter.after_auth;
            ret.TwitterToken = res.twitter.tweet_token;

            return ret;
        }
    }

    public class NicoNicoLiveData : NotificationObject
    {
        //取得できたかどうか　できたらok　失敗ならerror
        public string Status { get; set; }
        //エラー文字列
        public string Error { get; set; }

        public NicoNicoLiveStreamData Stream { get; set; }

        public NicoNicoLiveUserData User { get; set; }

        public bool RTMPIsFms { get; set; }
        public int RTMPPort { get; set; }
        public string RTMPUrl { get; set; }
        public string RTMPTicket { get; set; }

        //コメントサーバURL/ポート/スレッドID
        public string MSAddr { get; set; }
        public int MSPort { get; set; }
        public int MSThread { get; set; }

        public string TidList { get; set; }

        public bool TwitterIsLiveEnabled { get; set; }
        public int TwitterVipModeCount { get; set; }
        public string TwitterLiveApiUrl { get; set; }

        public int PlayerQOSAnalytics { get; set; }
        public string PlayerDialogOidashiUrl { get; set; }
        public bool PlayerIsNoticeViewerBalloonEnbled { get; set; }
        public string PlayerErrorReport { get; set; }

        public string MarqueeCategory { get; set; }
        public string MarqueeGameKey { get; set; }
        public string MarqueeGameTime { get; set; }
        public bool MarqueeIsForceNicowariOff { get; set; }
    }
    public class NicoNicoLiveStreamData : NotificationObject
    {
        //生放送Id
        public string Id { get; set; }

        //生放送タイトル
        public string Title { get; set; }

        //生放送説明
        public string Description { get; set; }

        //生放送主種別(公式はofficial、コミュニティはcommunity)
        public string ProviderType { get; set; }

        //コミュニティ配信の場合はコミュニティID(でないときは空"")
        public string DefaultCommunity { get; set; }

        //
        public int International { get; set; }

        //
        public bool IsOwner { get; set; }

        //生放送主Id
        public string OwnerId { get; set; }

        //生放送主名
        public string OwnerName { get; set; }

        //is予約枠
        public bool IsReserved { get; set; }

        //
        public bool IsNicoNicoEnqueteEnbled { get; set; }

        //来場者数
        public int WatchCount { get; set; }

        //コメント数
        public int CommentCount { get; set; }

        //
        public string BaseTime { get; set; }

        //開演時刻
        public string OpenTime { get; set; }

        //放送開始時刻
        public string StartTime { get; set; }

        //放送終了予定時刻
        public string EndTime { get; set; }

        //高画質対応
        public int HqStream { get; set; }

        //
        public bool IsRerunStream { get; set; }

        //
        public bool IsArchivePlayServer { get; set; }

        //
        public string BourbonUrl { get; set; }

        //
        public string FullVideo { get; set; }

        //
        public string AfterVideo { get; set; }

        //
        public string BeforeVideo { get; set; }

        //
        public string KickoutVideo { get; set; }

        //Twitterタグ
        public string TwitterTag { get; set; }

        //
        public int DanjoCommentMode { get; set; }

        //アスペクト比
        public string Aspect { get; set; }

        //
        public int InfinityMode { get; set; }

        //生放送:0 タイムシフト:1
        public bool Archive { get; set; }

        //
        public int PressDisplayLines { get; set; }

        //
        public int PressDisplayTime { get; set; }

        //
        public string PressStyleConf { get; set; }

        //
        public string PluginDelay { get; set; }

        //
        public string PluginUrl { get; set; }

        //
        public string PluginUrls { get; set; }

        //
        public bool AllowNetduetto { get; set; }

        //
        public string NdToken { get; set; }

        //
        public int NgScoring { get; set; }

        //
        public bool IsNonArchiveTimeShiftEnbled { get; set; }

        //
        public bool IsTimeShiftReserved { get; set; }

        //
        public int HeaderComment { get; set; }

        //
        public int FooterComment { get; set; }

        //
        public int SplitBottom { get; set; }

        //
        public int SplitTop { get; set; }

        //
        public int BackgroundComment { get; set; }

        //
        public int FontScale { get; set; }

        //
        public bool CommentLock { get; set; }

        //
        public bool Telop { get; set; }

        //
        public List<NicoNicoLiveContent> ContentsList { get; set; }

        //
        public List<NicoNicoLiveQueData> QueList { get; set; }

        //サムネイル画像url(大)？
        public string PictureUrl { get; set; }

        //サムネイル画像url(小)？
        public string ThumbUrl { get; set; }

        //
        public bool IsPriotityPrefecture { get; set; }
    }

    public class NicoNicoLiveContent : NotificationObject
    {
        //
        public int Id { get; set; }
        
        //
        public bool DisableAudio { get; set; }

        //
        public bool DisableVideo { get; set; }

        //
        public string StartTime { get; set; }

        //
        public string Url { get; set; }
    }

    public class NicoNicoLiveQueData : NotificationObject
    {
        //縦位置
        public int Vpos { get; set; }

        //メール
        public string Mail { get; set; }

        //名前
        public string Name { get; set; }

        //中身
        public string Text { get; set; }
    }

    public class NicoNicoLiveUserData : NotificationObject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsPremium { get; set; }
        public int Age { get; set; }
        public int Sex { get; set; }
        public string Domain { get; set; }
        //都道府県id
        public int Prefecture { get; set; }
        public string Language { get; set; }
        //いる席の部屋の名前(ex.アリーナ最前列)/コミュニティid
        public string RoomLabel { get; set; }
        //座席番号
        public string SeetNo { get; set; }
        //コミュメンバーかどうか
        public bool IsJoin { get; set; }
        //Twitter連携をしているか
        public string TwitterStatus { get; set; }
        public string TwitterScreenName { get; set; }
        public int TwitterFollowersCount { get; set; }
        public bool TwitterIsVip { get; set; }
        public string TwitterProfileImgUrl { get; set; }
        public int TwitterAfterAuth { get; set; }
        public string TwitterToken { get; set; }
    }
}
