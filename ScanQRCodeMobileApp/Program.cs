using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace ScanQRCodeMobileApp
{
    class Program
    {
        private static HttpClient httpClient = new HttpClient();
        private static string token;
        static void Main(string[] args)
        {
            {
                //模拟登陆获取token
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/Authorize/Token");
                var requestJson = JsonConvert.SerializeObject(new { Email = "1234567@qq.com", Password = "123" });
                httpRequestMessage.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                var resultJson = httpClient.SendAsync(httpRequestMessage).Result.Content.ReadAsStringAsync().Result;
                 token = JsonConvert.DeserializeObject<MyToken>(resultJson)?.Token;
            }
            {
                Console.WriteLine("请输入二维码");
                var qrCode=Console.ReadLine();
                if(!string.IsNullOrWhiteSpace(qrCode)&& !string.IsNullOrWhiteSpace(token))
                {//扫码模拟
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/api/SignalR/Send2FontRequest");
                    httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var requestJson = JsonConvert.SerializeObject(new ScanQRCodeDTO { ConnectionID = qrCode, Name = "1234567@qq.com" });
                    httpRequestMessage.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                    var result = httpClient.SendAsync(httpRequestMessage).Result;
                    var result1= result.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(result+",,,"+ result1);
                }
                else
                {
                    Console.WriteLine("token 或二维码不正确");
                }
            

            }
            Console.ReadLine();
        }
    }
}
