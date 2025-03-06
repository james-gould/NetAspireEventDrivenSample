using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBase.Data.Models
{
    public sealed class WeatherItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Reading { get; set; } = string.Empty;
        public DateTimeOffset GeneratedAt => DateTimeOffset.UtcNow;
    }
}