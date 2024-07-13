using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomNumbers.Data.Models
{
    public class Match
    {
        public int Id { get; set; }
        public TimeSpan ExpiryTimestamp { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? WinnerUserId { get; set; }
        public bool IsDone { get; set; }

        public virtual User WinnerUser { get; set; }
    }
}
