using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopLaptop.Models
{
    public class TelegramConfig
    {
        [Key]
        public int Id { get; set; }
        public string BotToken { get; set; }
        public string ChatId { get; set; }
        public bool IsActive { get; set; }
    }
}