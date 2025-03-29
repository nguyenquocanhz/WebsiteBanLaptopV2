using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace ShopLaptop.EF
{
    public class TelegramConfig
    {
        public Table<TelegramConfig> TelegramConfigs { get; set; }

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string BotToken { get; set; }

        [Required]
        [StringLength(255)]
        public string ChatId { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
    }

}