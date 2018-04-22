namespace HttpClientSample
{
  using System;
  using System.Net;
  using System.Net.Http;
  using System.Net.Http.Headers;
  using System.Threading.Tasks;

  public class User
  {
    public User(string username, string password)
    {
      this.username = username;
      this.password = password;
    }

    public string username { get; set; }

    public string password { get; set; }
  }

  class Program
  {
    static HttpClient client = new HttpClient();

    static async Task<HttpResponseMessage> LoginAsync(User user)
    {
      HttpResponseMessage response = await client.PostAsJsonAsync(
          "api-token-auth/", user);
      response.EnsureSuccessStatusCode();

      // return URI of the created resource.
      return response;
    }

    static async Task<User> GetUserAsync(string path)
    {
      User user = null;
      HttpResponseMessage response = await client.GetAsync(path);
      if (response.IsSuccessStatusCode)
      {
        user = await response.Content.ReadAsAsync<User>();
      }
      return user;
    }

    static void Main(User user)
    {
      RunAsync(user).GetAwaiter().GetResult();
    }

    public static async Task<bool> RunAsync(User user)
    {
      // Update port # in the following line.
      client.BaseAddress = new Uri("http://staging.tangent.tngnt.co/");
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(
          new MediaTypeWithQualityHeaderValue("application/json"));

      try
      {
        var login = await LoginAsync(user);
        Console.WriteLine($"Created at {login}");
        if (login.IsSuccessStatusCode)
        {
          return true;
        }
        else
        {
          return false;
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return false;
      }
    }
  }
}