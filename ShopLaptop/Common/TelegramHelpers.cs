using ShopLaptop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ShopLaptop.Common
{
    public class TelegramHelpers
    {

        private string botToken = "7791363433:AAHvW1ost2wDXzsdj3PLubBoJpXHrUaK8lM";
        private string chatId = "1364926983";

        private static TelegramHelpers instance;

        public static TelegramHelpers Instance
        {
            get
            {
                if (instance == null)
                    instance = new TelegramHelpers();
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        public TelegramHelpers()
        {

        }

        public void SendMessage(string message)
        {
            string url = string.Format("https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}", botToken, chatId, Uri.EscapeDataString(message));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.ContentLength = 0;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Lỗi khi gửi tin nhắn.");
                }
            }
        }

        public async Task SendMessageAsync(string message)
        {
            string url = $"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={chatId}&text={Uri.EscapeDataString(message)}";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Lỗi khi gửi tin nhắn: {response.StatusCode} - {error}");
                }
            }
        }

        public void SendOrderToTelegram(string message)
        {
            try
            {
                SendMessage(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gửi đơn hàng tới Telegram: {ex.Message}");
            }
        }
    }
}
