using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ch5_LanguageFeatures.Models
{
    public class MyAsyncMethods
    {
        public static Task<long?> GetPageLength()
        {
            HttpClient client = new HttpClient();
            var httpTask = client.GetAsync("http://apress.com");

            return httpTask.ContinueWith((Task<HttpResponseMessage> antecedent) =>
            {
                return antecedent.Result.Content.Headers.ContentLength;
            });
        }

        public async static Task<long?> GetPageLength2()
        {
            HttpClient client = new HttpClient();
            var httpMessage = await client.GetAsync("http://apress.com");

            return httpMessage.Content.Headers.ContentLength;
        }

        public async static Task<IEnumerable<long?>>
            GetPageLength3(List<string> output, params string[] urls)
        {
            List<long?> results = new List<long?>();
            HttpClient client = new HttpClient();
            foreach (var url in urls)
            {
                output.Add($"Request started for http://{url}");
                var httpMessage = await client.GetAsync($"http://{url}");
                results.Add(httpMessage.Content.Headers.ContentLength);
                output.Add($"Completed request for {url}");
            }

            return results;
        }

        // Async Enumerable
        public static async IAsyncEnumerable<long?>
            GetPageLength4(List<string> output, params string[] urls)
        {
            HttpClient client = new HttpClient();

            foreach (string url in urls)
            {
                output.Add($"Started request for {url}");
                var httpMessage = await client.GetAsync($"http://{url}");
                output.Add($"Completed request for {url}");
                yield return httpMessage.Content.Headers.ContentLength;
            }
        }
    }
}
