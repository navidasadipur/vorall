using RestSharp;
using SpadStorePanel.Core.WebService.Model;
using System;
using System.IO;
using System.Xml.Serialization;

namespace SpadStorePanel.Core.Utility
{
    public static class WebServiceRequest
    {
        public static ProductDATA GetProductByCode(string code)
        {
            RestClient client = new RestClient($"http://109.162.158.170:5010/kahkeshanMarketWebService/KahkeshanMarketWebService.asmx/GetProductByCode?ProductCode={code}")
            {
                Timeout = -1
            };

            RestRequest request = new RestRequest(Method.GET);

            IRestResponse response = client.Execute(request);

            Console.WriteLine(response.Content);

            ProductDATA result = new ProductDATA();

            XmlSerializer serializer = new XmlSerializer(typeof(ProductDATA));

            using (TextReader reader = new StringReader(response.Content))
            {
                result = (ProductDATA)serializer.Deserialize(reader);
            }

            return result;
        }

        public static ArrayOfProductDATA GetListProduct()
        {
            RestClient client = new RestClient("http://109.162.158.170:5010/kahkeshanMarketWebService/KahkeshanMarketWebService.asmx/FindAllProduct?Criteria=")
            {
                Timeout = -1
            };
            RestRequest request = new RestRequest(Method.GET);

            IRestResponse response = client.Execute(request);

            ArrayOfProductDATA result = new ArrayOfProductDATA();

            XmlSerializer serializer = new XmlSerializer(typeof(ArrayOfProductDATA));

            using (TextReader reader = new StringReader(response.Content))
            {
                result = (ArrayOfProductDATA)serializer.Deserialize(reader);
            }

            return result;
        }
    }
}
