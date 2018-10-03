using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace App1.Model
{
    public class CallLogModel
    {
        public string CallName { get; set; }
        public string CallNumber { get; set; }
        public string CallDuration { get; set; }
        public string CallDurationFormat {
            get
            {
                var intDuration = Convert.ToInt32(CallDuration);
                TimeSpan time = TimeSpan.FromSeconds(intDuration);

                //here backslash is must to tell that colon is
                //not the part of format, it just a character that we want in output
                return time.ToString(@"hh\:mm\:ss\:fff");
            }
        }
        public long CallDateTick { get; set; }
        public DateTime CallDate { get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(this.CallDateTick);
            }
        }

        public string CallType { get; set; }

        public string CallTitle { get => $"{CallNumber} - {CallName}"; }
        public string CallDescription { get => $"{CallDate.ToString("g", CultureInfo.CreateSpecificCulture("es-mx"))} - {CallType} | Duración: {CallDurationFormat}"; }
    }
}
