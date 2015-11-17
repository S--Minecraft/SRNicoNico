using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using Codeplex.Data;

namespace SRNicoNico.Models.NicoNicoWrapper
{
    public class NicoNicoLiveReservation : NotificationObject
    {
        //ニコ生予約リスト取得API
        public const string ReservationListApiURL = "http://live.nicovideo.jp/api/watchingreservation?mode=";
        //listが簡易、detaillistが詳細
        private const string ApiType = "detaillist";

        public static List<NicoNicoLiveReservationData> GetReservationList()
        {
            string uri = ReservationListApiURL + ApiType;

            var result = NicoNicoUtil.XmlToJson(NicoNicoWrapperMain.GetSession().GetAsync(uri).Result);
            var json = DynamicJson.Parse(result);
            var response = json.nicolive_video_response;

            List<NicoNicoLiveReservationData> ret = new List<NicoNicoLiveReservationData>();
            
            foreach(var entry in response.timeshift_reserved_detail_list)
            {
                NicoNicoLiveReservationData data = new NicoNicoLiveReservationData();
                data.Id = "lv" + entry.vid;
                data.Title = entry.title;

                if (entry.status == "TSARCHIVE")
                {
                    data.State = NicoNicoLiveReservationState.TSARCHIVE;
                } else if (entry.status == "FIRST_WATCH")
                {
                    data.State = NicoNicoLiveReservationState.FIRST_WATCH;
                } else if (entry.status == "WATCH")
                {
                    data.State = NicoNicoLiveReservationState.WATCH;
                } else if (entry.status == "USE_LIMIT_DATE_OUT")
                {
                    data.State = NicoNicoLiveReservationState.USE_LIMIT_DATE_OUT;
                } else if (entry.status == "RESERVED")
                {
                    data.State = NicoNicoLiveReservationState.RESERVED;
                }

                data.Unwatch = int.Parse(entry.unwatch);
                data.ExpireDate = NicoNicoUtil.GetTimeFromLong(long.Parse(entry.expire));

                ret.Add(data);
            }

            return ret;
        }
    }

    public enum NicoNicoLiveReservationState
    {
        TSARCHIVE,
        FIRST_WATCH,
        WATCH,
        USE_LIMIT_DATE_OUT,
        RESERVED
    }

    public class NicoNicoLiveReservationData : NotificationObject
    {
        //生放送id
        public string Id { get; set; }

        //タイトル
        public string Title { get; set; }

        //状態
        public NicoNicoLiveReservationState State { get; set; }

        //？
        public int Unwatch { get; set; }

        //期限切れ日時 UnixTime
        public string ExpireDate { get; set; }
    }
}
