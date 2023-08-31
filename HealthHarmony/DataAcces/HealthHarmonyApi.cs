using HealthHarmony.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace HealthHarmony.DataAcces;

public static class HealthHarmonyApi
{
    private const string
        get = "get/",
        add = "add",
        edite = "edit",
        delete = "delete/",
        api = "http://localhost:5028/api/users/";

    private readonly static HttpClient _httpClient = 
        new HttpClient();

    static HealthHarmonyApi() =>
        AppDomain.CurrentDomain.ProcessExit += 
        (x,y) => _httpClient.Dispose();

    public static async Task<IEnumerable<User>> GetUsers()=>
        await _httpClient.GetFromJsonAsync<IEnumerable<User>>(api + get);

    public static async Task<User> GetUser(int id) =>
        await _httpClient.GetFromJsonAsync<User>(api + get + id);

    public static async Task<User> AddUser(UserDataPackage user)
    {
        var responce = await _httpClient.PostAsJsonAsync(api + add, user);
        if (responce.StatusCode == HttpStatusCode.OK)
            return await responce.Content.ReadFromJsonAsync<User>();
        else
            return null;
    }

    public static async Task<UserDataPackage> Edite(UserDataPackage user)
    {
        var responce = await _httpClient.PutAsJsonAsync(api + edite, user);
        return await responce.Content.ReadFromJsonAsync<UserDataPackage>();
    }

    public static async Task<bool> DeleteUser(int id) =>
        (await _httpClient.DeleteAsync(api + delete + id))
        .StatusCode == HttpStatusCode.OK;
}
