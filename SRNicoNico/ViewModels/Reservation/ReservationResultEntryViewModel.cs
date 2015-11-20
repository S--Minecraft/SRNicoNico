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
using SRNicoNico.Models.NicoNicoWrapper;

namespace SRNicoNico.ViewModels
{
    public class ReservationResultEntryViewModel : ViewModel
    {
        //これをViewModelと呼んでいいのか謎だがとても重要なもの
        public NicoNicoLiveReservationData Data { get; set; }
    }
}
