using ServerSide;
using System.Text.Json;

namespace Server;
public class UserService
{
    public string AddUser(string json)
    {
        if (string.IsNullOrEmpty(json)) return "Unsuccessful";
        User u = JsonSerializer.Deserialize<User>(json)!;
        if (u is null) return "Unsuccessful";
        using var db = new AppDataContext();
        db.Users.Add(u);
        db.SaveChanges();
        return "Successful";
    }

    public string UpdateUser(string json)
    {
        if (string.IsNullOrEmpty(json)) return "Unsuccessful";
        User u = JsonSerializer.Deserialize<User>(json)!;
        if (u is null) return "Unsuccessful";
        using var db = new AppDataContext();
        var user = db.Users.FirstOrDefault(x => x.Id == u.Id);
        if (user is null) return "Unsuccessful";
        user.Name = u.Name;
        user.Surname = u.Surname;
        db.SaveChanges();
        return "Successful";
    }

    public string DeleteUser(string json)
    {
        if (string.IsNullOrEmpty(json)) return "Unsuccessful";
        User u = JsonSerializer.Deserialize<User>(json)!;
        if (u is null) return "Unsuccessful";
        using var db = new AppDataContext();
        var user = db.Users.FirstOrDefault(x => x.Id == u.Id);
        if (user is null) return "Unsuccessful";
        db.Users.Remove(user);
        db.SaveChanges();
        return "Successful";
    }

    public List<User> GetUser(string json)
    {
        using var db = new AppDataContext();
        return db.Users.ToList(); ;
    }


}