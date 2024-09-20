using System.Net;
using System.Reflection;
using System.Text.Json;
using Server;
using ServerSide;
var listener = new HttpListener();
int port = 27001;
listener.Prefixes.Add($"http://localhost:{port}/");
listener.Start();

try
{

    while (true)
    {
        var context = listener.GetContext();
        Console.WriteLine("Client Connected");
        _ = Task.Run(() =>

        {
            try
            {
                var url = context.Request.RawUrl;
                var splitedUrl = url.Split('/', StringSplitOptions.RemoveEmptyEntries);
                var _serviceName = $"Server.{splitedUrl[0]}Service";
                Assembly assembly = Assembly.GetExecutingAssembly();
                Type? type = assembly.GetType(_serviceName);
                if (type is null)
                    return;
                var service = Activator.CreateInstance(type) as UserService;
                var methodName = splitedUrl[1];
                var mi = type.GetMethod(methodName);
                if (splitedUrl.Length > 2)
                {
                    //User? user = JsonSerializer.Deserialize<User>(splitedUrl[2])!;
                    //User[] u = [user];
                    mi?.Invoke(service, null);
                }
                else
                {
                    mi?.Invoke(service,null);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        });
        // ,asdasd,dasd
    }

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

