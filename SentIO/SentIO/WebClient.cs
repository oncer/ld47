using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace SentIO
{
    class WebClient
    {
        private const string ADDR = "https://sentio.ddns.net";
        private static readonly HttpClient client = new HttpClient();
        private string macAddress;
        public WebClient()
        {
            macAddress = 
                (
                    from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where nic.OperationalStatus == OperationalStatus.Up
                    select nic.GetPhysicalAddress().ToString()
                ).FirstOrDefault();
        }

        private static WebClient _instance;
        public static WebClient Instance { 
            get 
            {
                if (_instance == null)
                {
                    _instance = new WebClient();
                }
                return _instance;
            }
        }

        public bool GetPlayerFinished()
        {
            string requestAddr = ADDR + "/api/player";
            var response = client.GetStringAsync(requestAddr);
            try {
                response.Wait(1000);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Could not connect to {requestAddr}: {e}");
                return false;
            }
            //{"ip":"84.113.55.163","finished":0}

            var json = JObject.Parse(response.Result);
            Debug.WriteLine(json["ip"].Value<string>());
            return json["finished"].Value<bool>();
        }
    }
}
