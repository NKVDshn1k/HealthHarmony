using HealthHarmony.DataAcces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace HealthHarmony.Model;


public class DbDomainUnionUsersModel : Base.ModelBase
{
    public ObservableCollection<User> Collection { get; private set; }

    public async Task Reload()
    {
        IEnumerable<User> collection = 
            await HealthHarmonyApi.GetUsers();

        if (collection == null)
            Collection = new ObservableCollection<User>();
        else
            Collection = new ObservableCollection<User>(collection);
    }

    public async Task Add(UserDataPackage user)
    {
        var result = await HealthHarmonyApi.AddUser(user);
        if (result != null)
            Collection.Add(result);
        else
            MessageBox.Show("failed to add object from database");
    }

    public async Task Edite(User original, UserDataPackage dataPackage)
    {
        var result = await HealthHarmonyApi.Edite(dataPackage);
        if (result == null)
            MessageBox.Show("failed to edite object from database");
        else
            original.Map(result);
    }

    public async Task Delete(User user)
    {
        var responce = await HealthHarmonyApi.DeleteUser(user.Id);
        if (responce)
            Collection.Remove(user);
        else
            MessageBox.Show("failed to delete object from database");
    }

    public void Swap(int index1, int index2)
    {
        var container = Collection[index1];
        Collection[index1] = Collection[index2];
        Collection[index2] = container;
    }
}
