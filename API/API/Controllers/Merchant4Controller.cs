using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Merchant")]
    public class Merchant4Controller : ApiController
    {
        public string service_command { get; set; }
        public string language { get; set; }
        public string merchant_identifier { get; set; }
        public string access_code { get; set; }
        public string merchant_reference { get; set; }
        public string card_security_code { get; set; }
        public string card_number { get; set; }
        public string expiry_date { get; set; }
        public string remember_me { get; set; }
        public string card_holder_name { get; set; }



        public class Reqobj
        {

             public string service_command { get; set; }
            //public string language { get; set; }
            //public string merchant_identifier { get; set; }
            //public string access_code { get; set; }
            //public string merchant_reference { get; set; }
            public string card_security_code { get; set; }
            public string card_number { get; set; }
            public string expiry_date { get; set; }
            public string token_name { get; set; }
            public string return_url { get; set; }
            public string currency { get; set; }
            public string remember_me { get; set; }
            public string card_holder_name { get; set; }
            public string amount { get; set; }
            public string order_description { get; set; }
            //public string Signature { get; set; }
            public string customer_email { get; set; }
        }
        [HttpPost]
        //[Authorize]
        [Route("SendMerchantResponseGetWebRequestAsync")]
        public async System.Threading.Tasks.Task<string> SendMerchantResponseGetWebRequestAsync(Reqobj TKreq)
        {
            access_code = System.Configuration.ConfigurationManager.AppSettings["access_code"].ToString();
            language = System.Configuration.ConfigurationManager.AppSettings["language"].ToString();
            merchant_identifier = System.Configuration.ConfigurationManager.AppSettings["merchant_identifier"].ToString();
            string SHA_RequestPhase = System.Configuration.ConfigurationManager.AppSettings["sha_request"].ToString();
            string SHA_ResponsePhase = System.Configuration.ConfigurationManager.AppSettings["sha_response"].ToString();

            int sand = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["sandbox"].ToString());
            var SandBoxMode = Convert.ToBoolean(sand);
            // service_command = "TOKENIZATION";
            string ApiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"].ToString();
            string str_returned = "";
            var Resultdata = "";
            try
            {
                //string concatenatedString = SHA_RequestPhase +
                //    "access_code" + "=" + access_code +
                //    "language" + "=" + language +
                //    "merchant_identifier" + "=" + merchant_identifier +
                //    "merchant_reference" + "=" + Guid.NewGuid().ToString() +
                //    "service_command" + "=" + service_command +
                //    SHA_ResponsePhase;
                string sigString = string.Empty;
                string PgResponseMsg = string.Empty;
                IDictionary<string, string> ResponseArgs1 = new Dictionary<string, string>();
                ResponseArgs1.Add(new KeyValuePair<String, string>("service_command", "TOKENIZATION"));
                ResponseArgs1.Add(new KeyValuePair<String, string>("language", language));
                ResponseArgs1.Add(new KeyValuePair<String, string>("merchant_identifier", merchant_identifier));
                ResponseArgs1.Add(new KeyValuePair<String, string>("access_code", access_code));
                ResponseArgs1.Add(new KeyValuePair<String, string>("merchant_reference", Guid.NewGuid().ToString()));
                //ResponseArgs1.Add(new KeyValuePair<String, string>("card_security_code", TKreq.card_security_code));
                //ResponseArgs1.Add(new KeyValuePair<String, string>("card_number", TKreq.card_number));
                //ResponseArgs1.Add(new KeyValuePair<String, string>("expiry_date", TKreq.expiry_date));
                var dataObj = new
                {
                    Data = ResponseArgs1
                }
                ;

                PgResponseMsg = JsonConvert.SerializeObject(dataObj);


                ResponseArgs1 = ResponseArgs1.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

                sigString += "LIVESHAOUT";
                foreach (var respargs in ResponseArgs1)
                {
                    if (respargs.Key != "signature")
                    {
                        if (!string.IsNullOrEmpty(respargs.Value))
                        {
                            sigString = (sigString
                                        + (respargs.Key + ("=" + System.Web.HttpUtility.HtmlDecode(respargs.Value).Replace(" ", ""))));
                        }
                    }
                }

                StringBuilder hashBuilder = new StringBuilder();
                Byte[] data = null;
                UTF8Encoding encoder = new UTF8Encoding();

                data = HashAlgorithm.Create("SHA256").ComputeHash(encoder.GetBytes(sigString));


                for (int i = 0; (i
                            <= (data.Length - 1)); i++)
                {
                    hashBuilder.Append(data[i].ToString("x2"));
                }

                var Signature = hashBuilder.ToString();

                ApiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"].ToString();



                var ResponseArgs = new List<KeyValuePair<string, string>>();
                //        'command' => 'PURCHASE',
                //'access_code' => 'enter_your_acccess_code_here',
                //'merchant_identifier' => 'enter_your_merchant_identifier_here',
                //'merchant_reference' => enter_your_merchant_reference_here,
                //'amount' => enter_your_amount_here,
                //'currency' => 'AED',
                //'language' => enter_your_language_here,
                //'customer_email' => 'customer@gmail.com',
                //'token_name' => enter_your_token_name_here,
                //'return_url' => return_url,
                //'card_security_code' => enter_your_cvv_here,
                 
                ResponseArgs.Add(new KeyValuePair<String, string>("card_security_code", "350"));
                ResponseArgs.Add(new KeyValuePair<String, string>("card_number", "5297412542005689"));
                ResponseArgs.Add(new KeyValuePair<String, string>("card_holder_name", "MADA"));
                ResponseArgs.Add(new KeyValuePair<String, string>("merchant_identifier", merchant_identifier));
                ResponseArgs.Add(new KeyValuePair<String, string>("access_code", access_code));
                ResponseArgs.Add(new KeyValuePair<String, string>("expiry_date", "2105"));
                ResponseArgs.Add(new KeyValuePair<String, string>("order_description", "Testing"));
                ResponseArgs.Add(new KeyValuePair<String, string>("language", language));
                ResponseArgs.Add(new KeyValuePair<String, string>("service_command", "TOKENIZATION"));
                ResponseArgs.Add(new KeyValuePair<String, string>("merchant_reference", Guid.NewGuid().ToString()));
                ResponseArgs.Add(new KeyValuePair<String, string>("Signature", Signature));
                ResponseArgs.Add(new KeyValuePair<String, string>("customer_email", "nqraa.it@gmail.com"));
                ResponseArgs.Add(new KeyValuePair<String, string>("return_url", "http://183.82.111.111/ALSADHAN/Web/login"));
                ResponseArgs.Add(new KeyValuePair<String, string>("amount", "10"));
                ResponseArgs.Add(new KeyValuePair<String, string>("currency", "SAR"));


                //ResponseArgs.Add(new KeyValuePair<String, string>("card_security_code", TKreq.card_security_code));
                //ResponseArgs.Add(new KeyValuePair<String, string>("service_command", TKreq.service_command));
                //ResponseArgs.Add(new KeyValuePair<String, string>("card_number", TKreq.card_number));
                //ResponseArgs.Add(new KeyValuePair<String, string>("card_holder_name", TKreq.card_holder_name));
                //ResponseArgs.Add(new KeyValuePair<String, string>("Signature", Signature));
                //ResponseArgs.Add(new KeyValuePair<String, string>("merchant_identifier", merchant_identifier));
                //ResponseArgs.Add(new KeyValuePair<String, string>("merchant_reference", Guid.NewGuid().ToString()));
                //ResponseArgs.Add(new KeyValuePair<String, string>("access_code", access_code));
                //ResponseArgs.Add(new KeyValuePair<String, string>("expiry_date", TKreq.expiry_date));
                //ResponseArgs.Add(new KeyValuePair<String, string>("return_url", TKreq.return_url));
                //ResponseArgs.Add(new KeyValuePair<String, string>("language", language));
                //ResponseArgs.Add(new KeyValuePair<String, string>("customer_email", TKreq.customer_email));
                //ResponseArgs.Add(new KeyValuePair<String, string>("order_description", TKreq.order_description));
                ////ResponseArgs.Add(new KeyValuePair<String, string>("sha_request", SHA_RequestPhase));
                ////ResponseArgs.Add(new KeyValuePair<String, string>("sha_response", SHA_ResponsePhase));
                ////ResponseArgs.Add(new KeyValuePair<String, string>("token_name", TKreq.token_name));
                ////ResponseArgs.Add(new KeyValuePair<String, string>("currency", TKreq.currency));
                ////ResponseArgs.Add(new KeyValuePair<String, string>("remember_me", TKreq.remember_me));
                ////ResponseArgs.Add(new KeyValuePair<String, string>("amount", TKreq.amount));

                ////{ "card_security_code":"***","service_command":"TOKENIZATION","card_number":"529741******5689","card_holder_name":"***","signature":"58dc5ff534db7407eada7d45a215518bed3c5beb26415791141c62e9e3b3bf16","merchant_identifier":"5aaa037c","merchant_reference":"aeb781bf-6b6c-46de-9d88-9216cefc4221","access_code":"kKXle2tHFV4P1O3dA2iu","expiry_date":"***","return_url":"http://localhost:65379/FortResponse.aspx","language":"en"}
                ////  { "card_security_code":"***","service_command":"TOKENIZATION","card_number":"529741******5689","card_holder_name":"***","signature":"8fbfe8e9b5c400e1cc4b079e4a52b0ff1cb9947c642c92f12badaca8866047d7","merchant_identifier":"5aaa037c","merchant_reference":"e11cf3f5-d687-488b-8075-4273e0a0ad1f","access_code":"kKXle2tHFV4P1O3dA2iu","expiry_date":"***","return_url":"http://localhost:65379/FortResponse.aspx","language":"en"}
                string Reqmsg = "";
                using (var client = new HttpClient())
                {
                    var byteArray = Encoding.ASCII.GetBytes("nqraa.it@gmail.com:Nqraa123@123");
                    var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    client.DefaultRequestHeaders.Authorization = header;
                    var request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);
                    request.Content = new FormUrlEncodedContent(ResponseArgs);
                    var response = await client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                        Reqmsg = null;

                    var result = await response.Content.ReadAsStringAsync();
                    Reqmsg = result;




                }
                return Reqmsg;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
                return (ex.Message);  
            }

        }
        [HttpGet] 
        //[Authorize]
        [Route("ValidateCard")]
        public async Task<dynamic> ValidateCard()
        {
            try {
                access_code = System.Configuration.ConfigurationManager.AppSettings["access_code"].ToString();
                language = System.Configuration.ConfigurationManager.AppSettings["language"].ToString();
                merchant_identifier = System.Configuration.ConfigurationManager.AppSettings["merchant_identifier"].ToString();
                string SHA_RequestPhase = System.Configuration.ConfigurationManager.AppSettings["sha_request"].ToString();
                string SHA_ResponsePhase = System.Configuration.ConfigurationManager.AppSettings["sha_response"].ToString();
                string merchant_referance = Guid.NewGuid().ToString();
                int sand = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["sandbox"].ToString());
                var SandBoxMode = Convert.ToBoolean(sand);

                string ApiUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrl"].ToString();
                var Reqmsg = "";
                string return_url = "http://localhost:65379/FortResponse.aspx";

                ArrayList PayfortParameters = new ArrayList
            {
                "service_command=TOKENIZATION",
                "merchant_reference="+merchant_referance,
                "return_url="+return_url
            };
                string signature = PAYFORT.Security.GenerateSignature(PayfortParameters);
                var ResponseArgs = new List<KeyValuePair<string, string>>();
                ResponseArgs.Add(new KeyValuePair<String, string>("signature", signature));
                ResponseArgs.Add(new KeyValuePair<String, string>("command_service", "TOKENIZATION"));
                ResponseArgs.Add(new KeyValuePair<String, string>("access_code", access_code));
                ResponseArgs.Add(new KeyValuePair<String, string>("merchant_identifier", merchant_identifier));
                ResponseArgs.Add(new KeyValuePair<String, string>("merchant_referance", merchant_referance));
                ResponseArgs.Add(new KeyValuePair<String, string>("language", language));
                ResponseArgs.Add(new KeyValuePair<String, string>("return_url", return_url));
                ResponseArgs.Add(new KeyValuePair<String, string>("expiry_date", "2105"));
                ResponseArgs.Add(new KeyValuePair<String, string>("card_number", "5297412542005689"));
                ResponseArgs.Add(new KeyValuePair<String, string>("card_security_code", "350"));
                ResponseArgs.Add(new KeyValuePair<String, string>("card_holder_name", "MADA"));
                var str_returned = "";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(ApiUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    streamWriter.Write(ResponseArgs);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    str_returned = result;
                }
                return str_returned;

                return Reqmsg;
            }
             
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
                return (ex.Message);
            }
        }
    }
}
