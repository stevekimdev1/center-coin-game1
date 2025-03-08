using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json; // System.Text.Json 사용

namespace 텔레그램_붓_기능
{
    class GetCharInfo
    {




        private string chatId = ""; // 그룹 ID 입력
        private string botToken = ""; // 봇 토큰 입력
        public async Task<string> GetCharIdAsync(string inbotToken)
        {
            botToken = inbotToken;
            await Get_Chard().ConfigureAwait(false);
            return chatId;
        }
        // https://api.telegram.org/bot77566701UAMixh_o9uqASTU8b4buQO80ktvap6K0/getUpdates


        async Task Get_Chard()
        {
            HttpClient client = new HttpClient();
            string chText = "/start";
            string url = $"https://api.telegram.org/bot{botToken}/getUpdates";
            HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false); // 데드락 방지
            string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            // JsonDocument로 응답 처리
            using (JsonDocument json = JsonDocument.Parse(responseBody))
            {
                foreach (var update in json.RootElement.GetProperty("result").EnumerateArray())
                {
                    if (update.TryGetProperty("message", out var message) && message.TryGetProperty("from", out var newMember))
                    {
                        message.TryGetProperty("text", out var text);
                        string chatMessage = text.GetString() ?? "";
                        if (chatMessage == chText)
                        {
                            chatId = newMember.GetProperty("id").ToString();
                        }
                    }
                }
            }
        }
    }
}
