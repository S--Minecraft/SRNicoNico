using Codeplex.Data;
using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRNicoNico.Models.NicoNicoWrapper
{
    class NicoNicoVitaLiveApi : NotificationObject
    {
        //生放送情報取得API
        private const string LiveDataApiUrl = "http://api.ce.nicovideo.jp/liveapi/v1/video.info?__format=json&v=";
        private const string LiveDataArrayApiUrl = "http://api.ce.nicovideo.jp/liveapi/v1/video.array?__format=json&v=";
        
        public static NicoNicoVitaApiLiveData GetLiveData(string liveId)
        {
            string result = NicoNicoWrapperMain.GetSession().GetAsync(LiveDataApiUrl + liveId).Result;

            var json = DynamicJson.Parse(result);
            var response = json.nicolive_video_response;

            NicoNicoVitaApiLiveData ret = new NicoNicoVitaApiLiveData();

            ret = ParseLiveData(response.video_info);
            ret.Status = response.@status;

            return ret;
        }

        public static Dictionary<string, NicoNicoVitaApiLiveData> GetLiveArrayData(List<string> liveIds)
        {
            string result = NicoNicoWrapperMain.GetSession().GetAsync(LiveDataArrayApiUrl + string.Join(",", liveIds)).Result;

            var json = DynamicJson.Parse(result);
            var response = json.nicolive_video_response;

            Dictionary<string, NicoNicoVitaApiLiveData> ret = new Dictionary<string, NicoNicoVitaApiLiveData>();
            
            foreach (var live in response.video_info)
            {
                ret[live.video.id] = ParseLiveData(live);
            }

            return ret;
        }

        public static NicoNicoVitaApiLiveData ParseLiveData (dynamic response)
        {
            NicoNicoVitaApiLiveData r = new NicoNicoVitaApiLiveData();
            if(response.community != "")
            {
                r.ChannelId = response.community.channel_id;
                r.GlobalId = response.community.global_id;
                r.ChannelName = response.community.name;
                r.ChannelThumbnailUrl = response.community.thumbnail;
                r.ChannelThumbnailSmallUrl = response.community.thumbnail_small;
            }
            //r.LockedTags = response.livetags.locked.livetag;
            //r.CurrentStatus = response.video._currentstatus;
            r.PictureUrl = response.video._picture_url;
            r.ThumbnailUrl = response.video._thumbnail_url;
            //r.TimeshiftLimit = int.Parse(response.video._timeshift_limit);
            //r.TsArchiveEndTime = NicoNicoUtil.DateFromVitaFormatDate(response.video._ts_archive_end_time);
            //r.TsArchiveReleasedTime = NicoNicoUtil.DateFromVitaFormatDate(response.video._ts_archive_released_time);
            //r.TsArchiveStartTime = NicoNicoUtil.DateFromVitaFormatDate(response.video._ts_archive_start_time);
            //r.TsIsEndless = response.video._ts_is_endless == "1" ? true : false;
            //r.TsReservedCount = int.Parse(response.video._ts_reserved_count);
            //r.TsViewLimitNum = int.Parse(response.video._ts_view_limit_num);
            //r.UseTsArchive = response.video._use_tsarchive == "1" ? true : false;
            r.ChannelOnly = response.video.channel_only == "1" ? true : false;
            r.CommentCount = int.Parse(response.video.comment_count);
            r.CommunityOnly = response.video.community_only == "1" ? true : false;
            r.Description = response.video.description() ? response.video.description : "";
            r.EndTime = NicoNicoUtil.DateFromVitaFormatDate(response.video.end_time);
            r.HidescoreComment = response.video.hidescore_comment == "1" ? true : false;
            r.HidescoreOnline = response.video.hidescore_online == "1" ? true : false;
            r.Id = response.video.id;
            r.IsHq = response.video.is_hq == "1" ? true : false;
            r.OpenTime = NicoNicoUtil.DateFromVitaFormatDate(response.video.open_time);
            r.ProviderType = response.video.provider_type;
            r.RelatedChannelId = response.video.related_channel_id;
            r.StartTime = NicoNicoUtil.DateFromVitaFormatDate(response.video.start_time);
            r.TimeshiftEnbled = response.video.timeshift_enabled == "1" ? true : false;
            r.Title = response.video.title;
            //r.UserId = response.video.user_id;
            r.ViewCount = int.Parse(response.video.view_counter);
            return r;
        }
    }

    public class NicoNicoVitaApiLiveData : NotificationObject
    {
        //受信成功かどうか
        public string Status { get; set; }

        //チャンネルId
        public string ChannelId { get; set; }

        //チャンネル/コミュニティId
        public string GlobalId { get; set; }

        //チャンネル/コミュニティ名
        public string ChannelName { get; set; }

        //チャンネルサムネ(大)
        public string ChannelThumbnailUrl { get; set; }

        //チャンネルサムネ(小)
        public string ChannelThumbnailSmallUrl { get; set; }

        //タグ
        public string[] LockedTags { get; set; }

        //放送中かどうかなど
        public string CurrentStatus { get; set; }

        //サムネイル(大)
        public string PictureUrl { get; set; }

        //サムネイル(小)
        public string ThumbnailUrl { get; set; }

        //
        public int TimeshiftLimit { get; set; }

        //
        public string TsArchiveEndTime { get; set; }

        //
        public string TsArchiveReleasedTime { get; set; }

        //
        public string TsArchiveStartTime { get; set; }

        //
        public bool TsIsEndless { get; set; }

        //
        public int TsReservedCount { get; set; }

        //
        public int TsViewLimitNum { get; set; }

        //
        public bool UseTsArchive { get; set; }

        //チャンネル限定
        public bool ChannelOnly { get; set; }

        //コメント数
        public int CommentCount { get; set; }

        //コミュニティ限定
        public bool CommunityOnly { get; set; }

        //説明
        public string Description { get; set; }

        //終了予定時刻
        public string EndTime { get; set; }

        //
        public bool HidescoreComment { get; set; }

        //
        public bool HidescoreOnline { get; set; }

        //生放送Id
        public string Id { get; set; }

        //高画質対応
        public bool IsHq { get; set; }

        //開演時刻
        public string OpenTime { get; set; }

        //生放送主種別(公式はofficial、コミュニティはcommunity)
        public string ProviderType { get; set; }

        //関連チャンネルId
        public string RelatedChannelId { get; set; }

        //放送開始時刻
        public string StartTime { get; set; }

        //タイムシフトの有無効
        public bool TimeshiftEnbled { get; set; }

        //タイトル
        public string Title { get; set; }

        //放送主ユーザーId
        public string UserId { get; set; }

        //再生数
        public int ViewCount { get; set; }
    }
}
