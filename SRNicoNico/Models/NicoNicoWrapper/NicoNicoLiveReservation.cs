using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using Codeplex.Data;
using SRNicoNico.ViewModels;

namespace SRNicoNico.Models.NicoNicoWrapper
{
    public class NicoNicoLiveReservation : NotificationObject
    {
        //ニコ生予約リスト取得API
        public const string ReservationListApiURL = "http://live.nicovideo.jp/api/watchingreservation?mode=";
        //listが簡易、detaillistが詳細
        private const string ApiType = "detaillist";
        
        private ReservationViewModel Reservation;

        public NicoNicoLiveReservation(ReservationViewModel vm)
        {

            Reservation = vm;
        }

        public List<NicoNicoLiveReservationData> GetReservationList()
        {
            string uri = ReservationListApiURL + ApiType;

            Reservation.Status = "生放送予約リスト取得中";

            var result = NicoNicoUtil.XmlToJson(NicoNicoWrapperMain.GetSession().GetAsync(uri).Result);
            var json = DynamicJson.Parse(result);
            var response = json.nicolive_video_response.timeshift_reserved_detail_list;

            List<NicoNicoLiveReservationData> ret = new List<NicoNicoLiveReservationData>();

            List<string> lives = new List<string>();
            foreach(var entry in response.reserved_item)
            {
                lives.Add("lv" + entry.vid);
            }
            Dictionary<string, NicoNicoVitaApiLiveData> vitaLives = NicoNicoVitaLiveApi.GetLiveArrayData(lives);
            
            foreach(var entry in response.reserved_item)
            {
                NicoNicoLiveReservationData data = new NicoNicoLiveReservationData();
                data.Id = "lv" + entry.vid;
                data.Title = entry.title;
                if (entry.status == "FIRST_WATCH") {
                    data.State = "視聴権を利用する";
                } else if (entry.status == "WATCH") {
                    data.State = "視聴する";
                } else if (entry.status == "USE_LIMIT_DATE_OUT") {
                    data.State = "視聴権利用期限が切れています";
                } else if (entry.status == "RESERVED") {
                    data.State = "予約中";
                } else {
                    data.State = entry.status;
                }

                if (entry.unwatch == "1") {
                    data.Unwatch = true;
                } else
                {
                    data.Unwatch = false;
                }

                if (entry.expire != "0")
                {
                    data.ExpireDate = NicoNicoUtil.GetTimeFromLong(long.Parse(entry.expire));
                }

                if (vitaLives.ContainsKey(data.Id))
                {
                    data.Data = vitaLives[data.Id];
                }

                ret.Add(data);
            }

            

            Reservation.Status = "生放送予約リスト取得完了";

            return ret;
        }
    }

    public class NicoNicoLiveReservationData : NotificationObject
    {
        //生放送id
        public string Id { get; set; }

        //タイトル
        public string Title { get; set; }

        //状態
        public string State { get; set; }

        //一度でも見たかどうか
        public bool Unwatch { get; set; }

        //期限切れ日時 UnixTime
        public string ExpireDate { get; set; }

        //詳細情報
        public NicoNicoVitaApiLiveData Data { get; set; }
    }
}
