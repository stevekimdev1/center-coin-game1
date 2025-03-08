using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using static Telegram.Bot.TelegramBotClient;


namespace 텔레그램_붓_기능
{
    class BotGame1
    {
        public static int GameRun = 1;

        public string domain;
        public string imageUrl;
        public string captionText;
        public string gameTest;

        public  void  Start()
        {
            GameRun = 1;
        }
        public void Stop()
        {
            GameRun = 0;
        }

        public async Task BotStart(string botToken, string chatId, string domain)
        {
            BotUpdates(botToken, chatId, domain, imageUrl, captionText, gameTest);
        }

        static async Task BotUpdates(string botToken,string chatId,string domain, string imageUrl, string captionText, string gameTest)
        {
             HttpClient client = new HttpClient();
             int offset1 = 0;
            string userId;
            string userName;
            string chText = "/start";
            while (GameRun == 1)
            {
                string url = $"https://api.telegram.org/bot{botToken}/getUpdates?offset={offset1}";
                HttpResponseMessage response = await client.GetAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();

                // JsonDocument로 응답 처리
                using (JsonDocument json = JsonDocument.Parse(responseBody))
                {
                    foreach (var update in json.RootElement.GetProperty("result").EnumerateArray())
                    {
                        if (update.TryGetProperty("message", out var message) && message.TryGetProperty("from", out var newMember))
                        {
                            if (newMember.GetProperty("id").ToString() != chatId) // 봇 자신이 아닐 경우
                            {
                                userId = newMember.GetProperty("id").ToString();
                                userName = newMember.GetProperty("username").GetString() ?? "Unknown";
                                if (message.TryGetProperty("text", out var text))
                                {
                                    string chatMessage = text.GetString() ?? "";
                                    if (chatMessage == chText)
                                    {
                                        //await SendPhotoAsync(botToken, userId, userName,domain,imageUrl,captionText,gameTest);
                                        SendMessage1(botToken, userId, userName, domain, imageUrl, captionText, gameTest);
                                    }
                                }
                            }
                        }
                        offset1 = update.GetProperty("update_id").GetInt32() + 1; // 다음 업데이트 ID로 이동
                    }
                }

                await Task.Delay(1000); // 1초 대기 후 다시 확인
            }
      
        }
        static async Task SendPhotoAsync(string botToken, string chatId, string userName,string domain, string imageUrl,string captionText,string gameTest)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://api.telegram.org/bot{botToken}/sendPhoto";

                var payload = new
                {
                    chat_id = chatId,
                    photo = imageUrl,
                    caption = captionText,
                    parse_mode = "Markdown",
                    reply_markup = new
                    {
                        inline_keyboard = new[]
                        {
                            new[]
                            {
                                new
                                {
                                    text = "{gameTest}",
                                    // web_app = new { url = "https://centercoin.kr/?chat_id={chatId}&username={{username}}" }
                                    web_app = new { url = "{domain}/?chat_id={chatId}&username={{username}}" }
                                }
                            },

                            //new[]
                            //{
                            //    new
                            //    {
                            //        text = "🚀 Start Game",
                            //        url = $"https://centercoin.kr/?chat_id={chatId}&username={{username}}"
                            //    }
                            //},

                            //new[]
                            //{
                            //    new
                            //    {
                            //        text = "📢 Join Our Community",
                            //        web_app = new { url = "https://centercoin.kr/?chat_id={chatId}&username={{username}}" }
                            //    }
                            //}
                        }
                    }
                };
                string jsonPayload = JsonSerializer.Serialize(payload);
                HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("✅ 채널 메시지 전송 성공!");
                }
                else
                {
                    Console.WriteLine($"❌ 전송 실패: {response.StatusCode}");
                }
            }
        }


        static async Task SendMessage1(string botToken, string chatId, string userName, string domain, string imageUrl, string captionText, string gameTest)
        {
            if (botClient == null)
            {
                botClient = new TelegramBotClient(botToken);
            }
            string webAppUrl = $"{domain}/?chat_id={chatId}&username={userName}";
            string gaemStart = $"{gameTest}";
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    // InlineKeyboardButton.WithUrl("🚀 Start Game", $"https://centercoin.kr/?chat_id={GroupChatId}")
                    // InlineKeyboardButton.WithWebApp("🚀 Start Game", "t.me/OkGameRoom_bot/ugame")
                    InlineKeyboardButton.WithUrl(gaemStart, webAppUrl)
                }
                //new []
                //{
                //  //  InlineKeyboardButton.WithUrl("🚀 Start Game", $"https://centercoin.kr/?chat_id={GroupChatId}")
                //   // InlineKeyboardButton.WithUrl("📢 Join Our Community", "t.me/OkGameRoom_bot/ugame")
                //    InlineKeyboardButton.WithUrl("🚀 Start Game", $"t.me/OkGameRoom_bot/ugame/?chat_id={chatId}")
                //}
            });

            try
            {
                await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: imageUrl,
                    caption: captionText,
                    replyMarkup: inlineKeyboard
                );

                Console.WriteLine("✅ Photo message sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to send photo message: {ex.Message}");
            }
        }



        private static TelegramBotClient botClient = null;
        public async Task SendMessage(string botToken, string chatId, string userName, string domain, string imageUrl, string captionText, string gameTest)
        {
            if (botClient == null)
            {
                botClient = new TelegramBotClient(botToken);
            }
            string webAppUrl = $"{domain}/?chat_id={chatId}&username={userName}";
            string gaemStart = $"{gameTest}";
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    // InlineKeyboardButton.WithUrl("🚀 Start Game", $"https://centercoin.kr/?chat_id={GroupChatId}")
                    // InlineKeyboardButton.WithWebApp("🚀 Start Game", "t.me/OkGameRoom_bot/ugame")
                    InlineKeyboardButton.WithUrl(gaemStart, webAppUrl)
                }
                //new []
                //{
                //  //  InlineKeyboardButton.WithUrl("🚀 Start Game", $"https://centercoin.kr/?chat_id={GroupChatId}")
                //   // InlineKeyboardButton.WithUrl("📢 Join Our Community", "t.me/OkGameRoom_bot/ugame")
                //    InlineKeyboardButton.WithUrl("🚀 Start Game", $"t.me/OkGameRoom_bot/ugame/?chat_id={chatId}")
                //}
            });

            try
            {
                await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: imageUrl,
                    caption: captionText,
                    replyMarkup: inlineKeyboard
                );

                Console.WriteLine("✅ Photo message sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to send photo message: {ex.Message}");
            }
        }



    }
}
