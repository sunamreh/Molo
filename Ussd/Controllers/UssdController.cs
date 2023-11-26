using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Ussd.Models;

namespace Ussd.Controllers
{
    [Route("ussd")]
    [ApiController]
    public class UssdController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache;
        public HttpClient _httpClient { get; set; }

        public UssdController(ILogger<UssdController> logger, IMemoryCache cache, HttpClient httpClient)
        {
            _logger = logger;
            _cache = cache;
            _httpClient = httpClient;
        }

        [HttpPost]
        [Route("callback")]
        public async Task<ActionResult<string>> Callback([FromForm] CallbackRequestModel model)
        {
            _logger.LogInformation($"sessionId: {model.SessionId}; phoneNumber: {model.PhoneNumber}; networkCode: {model.NetworkCode}; serviceCode: {model.ServiceCode}; text: {model.Text}");

            model.PhoneNumber = model.PhoneNumber.Replace("+27", "0");

            var response = string.Empty;

            var existingSubsciber = await SendSubscriberExistsRequest(model.PhoneNumber, model.SessionId);

            if (!existingSubsciber)
                response = await NavigateSubscribe(model);
            else 
                response = await NavigateTransact(model);

            return await Task.FromResult(Content(response, "text/plain"));
        }

        private async Task<string> NavigateSubscribe(CallbackRequestModel model)
        {
            string response = string.Empty;

            var subscribeModel = new SubscribeRequestModel
            {
                Msisdn = model.PhoneNumber
            };

            if (string.IsNullOrEmpty(model.Text))
            {
                response = SubscribeStart();
            }
            else if (model.Text == "1")
            {
                response = response = "CON Loan mobile money to family and friends, agree on a interest rate and collection date. MoLo will collect the money for you on collection date with interest. \n\n";
                response += "0. Back to previous menu";

                return response;
            }
            else if (model.Text == "1*0")
            {
                return SubscribeStart();
            }
            else if (model.Text == "2")
            {
                response = "CON Please enter Name:";
            }
            else if (model.Text.Count(c => c == '*') == 1)
            {
                response = "CON Create a 5 digit Molo PIN to continue:";
            }
            else if (model.Text.Count(c => c == '*') == 2)
            {
                var values = model.Text.Split('*');
                subscribeModel.Name = values[1];
                subscribeModel.Pin = values[2];
                
                response = await SendSubscribeRequest(subscribeModel);

                if (string.IsNullOrEmpty(response))
                    response = "END Your request has been submitted for processing. You will receive an SMS on successful subscription. Thank you!";
            }

            _cache.Remove($"{model.SessionId}-SubscriberProfile");
            _cache.Remove($"{model.SessionId}-SubscriberExists");

            return response;
        }

        private static string SubscribeStart()
        {
            var response = "CON Welcome to MoLo, Mobile Loans! \n";
            response += "Please choose an option to continue: \n";
            response += "1. What is Molo? \n";
            response += "2. Subscribe";

            return response;
        }

        private async Task<string> NavigateTransact(CallbackRequestModel model)
        {
            string response = string.Empty;
            SubscriberProfileResponseModel subscriberProfile = null;

            if (string.IsNullOrEmpty(model.Text))
            {
                response = "CON Enter your 5 digit MoLo PIN to continue";
            }
            else 
            {
                string[] text = model.Text.Split('*');

                subscriberProfile = await SendSubscriberProfileRequest(model.SessionId, model.PhoneNumber, text[0]);

                if (subscriberProfile == null)
                    return "END Invalid PIN Number";

                if (text.Length == 1)
                {
                    response = $"CON Welcome {subscriberProfile.Name} to MoLo, Mobile Loans! \n\n";
                    response += "Please choose an option to continue: \n";
                    response += "1. Transact \n";
                    response += "2. Balances \n";
                    response += "3. Manage Active Clients \n";
                }
                else if (text[1] == "1")
                {
                    if (text.Length == 1)
                    {
                        response = "CON Please enter Client's Name:";
                    }
                    if (text.Length == 2)
                    {
                        response = "CON Enter Client's Name:";
                    }
                    if (text.Length == 3)
                    {
                        response = "CON Enter Client's Phone Number:";
                    }
                    else if (text.Length == 4)
                    {
                        response = "CON Enter Loan Amount";
                    }
                    else if (text.Length == 5)
                    {
                        response = "CON Enter Collection Day";
                    }
                    else if (text.Length == 6)
                    {
                        //Change to select
                        response = "CON Enter Collection Month";
                    }
                    else if (text.Length == 7)
                    {
                        //Change to select
                        response = "CON Enter Collection Year";
                    }
                    else if (text.Length == 8)
                    {
                        //Change to select
                        response = "CON Select Interest Rate: \n\n";
                        response += "1. 5% \n";
                        response += "2. 10% \n";
                        response += "3. 15% \n";
                        response += "4. 20% \n";
                        response += "5. 30% \n";
                    }
                    else if (text.Length == 9)
                    {
                        var id = subscriberProfile.SubscriberId;
                        var clientName = text[2];
                        var clientMsisdn = text[3];
                        var loanAmount = text[4];

                        int year = Int32.Parse(text[7]);
                        int month = Int32.Parse(text[6]);
                        int day = Int32.Parse(text[5]);
                        var collectionDate = new DateTime(year, month, day);
                        var interestRate = Byte.Parse(text[8]);

                        var transactRequest = new TransactRequestModel
                        {
                            SubscriberId = id,
                            ClientMsisdn = clientMsisdn,
                            ClientName = clientName,
                            CollectionDate = collectionDate.ToString(),
                            InterestRateId = interestRate,
                            LoanAmount = loanAmount
                        };

                        decimal interest = 1;

                        switch (text[8])
                        {
                            case "1": interest = 1.05m;
                                break;
                            case "2":
                                interest = 1.10m;
                                break;
                            case "3":
                                interest = 1.15m;
                                break;
                            case "4":
                                interest = 1.20m;
                                break;
                            case "5":
                                interest = 1.30m;
                                break;
                            default:
                                interest = 1m;
                                break;
                        }

                        response = $"END A payment will be made to {clientName} - {clientMsisdn}, " +
                            $"for an amount of {loanAmount} to be collected on {collectionDate}. Total to be collected {decimal.Parse(loanAmount) * interest}";

                        await SendTransactRequest(transactRequest);
                    }
                }
                else if (text[1] == "2")
                {
                    response = $"END You have a total outstanding balance owed to you of {subscriberProfile.OutstandingBalance} \n\n";
                    //response += "0. Return";
                }
                else if (text[1] == "3")
                {
                    if (text.Length == 2)
                    {
                        response = "CON Search for Client by Msisdn: \n\n";
                    }else if (text.Length == 3)
                    {
                        var clientMsisdn = text[2];

                        var client = subscriberProfile.ActiveClients.FirstOrDefault(c => c.Msisdn == clientMsisdn);

                        if (client == null)
                            return $"END The Phonenumber {clientMsisdn} is not an active client";

                        response = $"CON {client.Name} ({client.Msisdn}) - Balance {client.Balance} \n\n";
                        response += "1. Settle \n";
                        response += "2. Collect \n";
                    }
                    else if (text.Length == 4 && text[3] == "1")
                    {
                        var clientMsisdn = text[2];

                        var client = subscriberProfile.ActiveClients.FirstOrDefault(c => c.Msisdn == clientMsisdn);

                        response = $"END A request to collect will be made to {client.Name} ({client.Msisdn}) for an amount of {client.Balance} \n\n";

                        await SendSettleRequest(client.Id);

                    }
                    else if (text.Length == 4 && text[3] == "2")
                    {
                        var clientMsisdn = text[2];

                        var client = subscriberProfile.ActiveClients.FirstOrDefault(c => c.Msisdn == clientMsisdn);

                        response = $"END The amount of {client.Balance} will be settled {client.Name} ({client.Msisdn}) \n\n";

                        await SendCollectRequest(client.Id);
                    }

                }
            }

            return response;
        }

        private async Task<string> SendSubscribeRequest(SubscribeRequestModel model)
        {
            var url = $"http://moloapi.azurewebsites.net/subscription/subscribe";
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var response = await MakeHttpRequestAsync(url, HttpMethod.Post, content);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return "END The Subscriber number is not a valid Momo account holder";
            }

            return null;
        }

        private async Task<SubscriberProfileResponseModel> SendSubscriberProfileRequest(string sessionId, string msisdn, string pin)
        {
            var responseData = await _cache.GetOrCreateAsync(
                $"{sessionId}-SubscriberProfile",
                async cacheEntry =>
                {
                    var url = $"http://moloapi.azurewebsites.net/transaction/subscriberprofile?pin={pin}&msisdn={msisdn}";

                    var response = await MakeHttpRequestAsync(url, HttpMethod.Get);

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return null;

                    var responseJson = await response.Content.ReadAsStringAsync();

                    var responseData = JsonSerializer.Deserialize<SubscriberProfileResponseModel>(responseJson);

                    return responseData;
                });

            if (responseData == null)
                _cache.Remove($"{sessionId}-SubscriberProfile");

            return responseData;
        }

        private async Task<string> SendTransactRequest(TransactRequestModel model)
        {
            var url = $"http://moloapi.azurewebsites.net/transaction/transact";
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var response = await MakeHttpRequestAsync(url, HttpMethod.Post, content);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return "END The Subscriber number is not a valid Momo account holder";
            }

            return null;
        }

        private async Task<string> SendCollectRequest(Guid clientId)
        {
            var body = new CollectBalanceRequestModel
            {
                ClientId = clientId
            };

            var url = $"http://moloapi.azurewebsites.net/collect/collectbalance";
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            await MakeHttpRequestAsync(url, HttpMethod.Post, content);

            return null;
        }

        private async Task SendSettleRequest(Guid clientId)
        {
            var body = new SettleRequestModel
            {
                ClientId = clientId
            };

            var url = $"http://moloapi.azurewebsites.net/collect/settle";
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            await MakeHttpRequestAsync(url, HttpMethod.Post, content);
        }

        private async Task<bool> SendSubscriberExistsRequest(string msisdn, string sesionId)
        {
            var responseData = await _cache.GetOrCreateAsync(
                $"{sesionId}-SubscriberExists",
                async cacheEntry =>
                {
                    var url = $"http://moloapi.azurewebsites.net/subscription/subscriberexists?msisdn={msisdn}";

                    var response = await MakeHttpRequestAsync(url, HttpMethod.Get);
                    var responseJson = await response.Content.ReadAsStringAsync();

                    var responseData = JsonSerializer.Deserialize<bool>(responseJson);

                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300);

                    return responseData;
                });

            return responseData;
        }

        private async Task<HttpResponseMessage> MakeHttpRequestAsync(string url, HttpMethod httpMethod, HttpContent content = null)
        {
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri(url)
            };

            string jsonContent = string.Empty;

            if (content != null)
            {
                jsonContent = await content.ReadAsStringAsync();
                request.Content = content;
            }

            _logger.LogInformation($"MakeHttpRequestAsync: Sending request to [{httpMethod.Method}] {url}. Content: \n {jsonContent}");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string message = $"HTTP request failed with status code {response.StatusCode}";

                _logger.LogError(message);

                //throw new HttpRequestException(message);
            }

            return response;
        }
    }
}
