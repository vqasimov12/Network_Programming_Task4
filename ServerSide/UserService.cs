using ServerSide;

namespace Server;
public class UserService
{
    public void AddUser(User? user)
    {
        Console.WriteLine("added");
    }

    public void UpdateUser (User? user)
    {
        Console.WriteLine("uodated");

    }

    public void DeleteUser(User? user)
    {
        Console.WriteLine("deleted");

    }

    public List<User> GetUsers(object? obj)
    {
        return null;
    }

}