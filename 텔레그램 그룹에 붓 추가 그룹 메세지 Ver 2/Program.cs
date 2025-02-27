using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static Telegram.Bot.TelegramBotClient;


namespace 텔레그램_그룹에_붓_추가_그룹_메세지_Ver_2
{
    internal static class Program
    {


        // 안대표
        private static readonly string Token = "8114311955:AAGjac5TGgmb8BDQaS1sBXFITOv4qOEe76s"; // 텔레그램 봇 토큰
        private static readonly long GroupChatId = -1002295706262; // "5597895053"; // 메시지를 보낼 대상의 채팅 ID .            


        //private static readonly string Token = "7656905376:AAGYB_pa0s_brGJ-yyZiP0zzm3Onpy1LQzg";  // 봇 토큰 입력
        //private static readonly long GroupChatId = -1002275129371; // 그룹 채팅 ID

        private static readonly TelegramBotClient botClient = new TelegramBotClient(Token);
        [STAThread]
        static async Task Main()
        {
            // await SendMessageToGroupAsync("안녕하세요, 이 메시지는 그룹에 전송됩니다.");
            string imageUrl = "https://centercoin.kr/1.jpg"; // 이미지 URL

            var messageText = "🐰 *새로운 게임 111 Welcome to BadBunny Tap2Earn!* 🥕\n\n" +
                    "Hop into the paws of BadBunny, unlock treasures, and piece together forgotten tales!\n\n" +
                    "👉 *Tap Into the Adventure* \n" +
                    "Ready your fingers, slice those carrots, and earn your Airdrop XP!\n\n" +
                    "*Shop 🛠️* – Boost your power and customize your adventure!\n" +
                    "*Friends 🔪* – Recruit and earn 5,000 tokens per friend!\n" +
                    "*Special Tasks 🐾* – Complete tasks to earn big rewards!\n\n" +
                    "_Stay alert for Airdrop Alerts 🍄!_";
            long ChatId = -1002275129371; // 그룹 채팅 ID

            await SendPhotoAsync(GroupChatId.ToString(), imageUrl, messageText);
            //// PlaySendPhotoAsync(GroupChatId.ToString());

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }


        static async Task PlaySendPhotoAsync(string chatId)
        {
            // 인라인 키보드 버튼 생성
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithUrl("🔥 Play", "t.me/OkGameRoom_bot/ugame") // 버튼 생성
                }
            });

            // 메시지 전송
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "클릭하면 CenterCoin 웹사이트로 이동합니다!",
                replyMarkup: inlineKeyboard
            );

            Console.WriteLine("메시지 전송 완료!");
        }


        static async Task SendPhotoAsync(string chatId, string imageUrl, string captionText)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                   // InlineKeyboardButton.WithUrl("🚀 Start Game", $"https://centercoin.kr/?chat_id={GroupChatId}")
                   // InlineKeyboardButton.WithWebApp("🚀 Start Game", "t.me/OkGameRoom_bot/ugame")
                    InlineKeyboardButton.WithUrl("🚀 Start Game", $"t.me/OkGameRoom_bot/ugame/?chat_id={GroupChatId}")
                },
                new []
                {
                  //  InlineKeyboardButton.WithUrl("🚀 Start Game", $"https://centercoin.kr/?chat_id={GroupChatId}")
                   // InlineKeyboardButton.WithUrl("📢 Join Our Community", "t.me/OkGameRoom_bot/ugame")
                    InlineKeyboardButton.WithUrl("🚀 Start Game", $"t.me/OkGameRoom_bot/ugame/?chat_id={GroupChatId}")
                }
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

        //var keyboard = new InlineKeyboardMarkup(buttons.Select(b => new[] { InlineKeyboardButton.WithUrl(b, $"https://centercoin.kr/?chat_id={GroupChatId}") }).ToArray());

        private static async Task SendMessageToGroupAsync(string message)
        {
            try
            {
                await botClient.SendTextMessageAsync(
                    chatId: GroupChatId,
                    text: message
                );
                Console.WriteLine("메시지가 성공적으로 전송되었습니다.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"메시지 전송 중 오류 발생: {ex.Message}");
            }
        }

    }
}