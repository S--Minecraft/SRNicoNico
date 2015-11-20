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
using System.Threading.Tasks;
using SRNicoNico.Models.NicoNicoWrapper;
using System.Windows.Input;

namespace SRNicoNico.ViewModels
{
    public class ReservationViewModel : TabItemViewModel
    {
        
        #region ReservationResult変更通知プロパティ
        private ReservationResultViewModel _ReservationResult;

        public ReservationResultViewModel ReservationResult
        {
            get { return _ReservationResult; }
            set
            {
                if (_ReservationResult == value)
                    return;
                _ReservationResult = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        public ReservationViewModel() : base("予約リスト") {

        }

        public void OpenReservation()
        {
            ReservationResultViewModel Reservation = new ReservationResultViewModel();

            ReservationResult = Reservation;
            Reservation.IsActive = true;

            Task.Run(() => {

                foreach (NicoNicoLiveReservationData data in new NicoNicoLiveReservation(this).GetReservationList())
                {

                    ReservationResultEntryViewModel entry = new ReservationResultEntryViewModel()
                    {

                        Data = data
                    };

                    Reservation.List.Add(entry);
                }
                Reservation.IsActive = false;
            });
        }
        public override void KeyDown(KeyEventArgs e)
        {

            if (e.Key == Key.F5)
            {

                OpenReservation();
            }
        }

    }
}