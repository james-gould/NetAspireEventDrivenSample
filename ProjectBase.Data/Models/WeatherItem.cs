﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBase.Data.Models
{
    public class WeatherItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WeatherItemId { get; set; }

        public string Reading { get; set; } = string.Empty;

        public DateTimeOffset GeneratedAt => DateTimeOffset.UtcNow;

        public int CityId { get; set; }

        public virtual City City { get; set; } = new();
    }
}