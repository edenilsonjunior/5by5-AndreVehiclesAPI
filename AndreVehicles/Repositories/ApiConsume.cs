using Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ApiConsume<T>
    {



        public static async Task<T?> Get(string uri, string requestUri)
        {
            T? generics;

            try
            {
                using HttpClient client = new();
                client.BaseAddress = new Uri(uri);

                HttpResponseMessage response = await client.GetAsync(requestUri);

                response.EnsureSuccessStatusCode();
                generics = await response.Content.ReadFromJsonAsync<T>();
            }
            catch (Exception)
            {
                return default;
            }

            if (generics == null)
                return default;

            return generics;
        }
    }
}
