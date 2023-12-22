using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkCommunication : MonoBehaviour
{
    public static NetworkCommunication Instance;
    private string apiUrl = "https://www.random.org/integers/?num=1&min=1&max=6&col=1&base=10&format=plain&rnd=new";
    public int randomNumber;


    private void Awake()
    {
        Instance = this;
    }

    public async Task GetNumber()
    {
        randomNumber = await GetValue();
    }

    public async Task<int> GetValue()
    {
        HttpClient client = new HttpClient();
        var response = await client.GetAsync("https://www.random.org/integers/?num=1&min=1&max=6&col=1&base=10&format=plain&rnd=new");
    
        if (response.IsSuccessStatusCode)
        {
            string numberString = await response.Content.ReadAsStringAsync();
            if (int.TryParse(numberString, out int randomNumber))
            {
                return randomNumber;
            }
        }

        return -1; // Or any other default/error value
    }
}
