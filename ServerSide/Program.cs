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
                foreach (var i in splitedUrl)
                    Console.WriteLine(i);
                var _serviceName = $"Server.{splitedUrl[0]}Service";
                Assembly assembly = Assembly.GetExecutingAssembly();
                Type? type = assembly.GetType(_serviceName);
                if (type is null)
                    return;
                var service = Activator.CreateInstance(type) as UserService;
                var methodName = $"{splitedUrl[1]}User";
                var mi = type.GetMethod(methodName);
                using var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding);
                var requestBody = reader.ReadToEnd();
                var result = mi?.Invoke(service, [requestBody]);
                if (result is List<User> users)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.OK;

                    using var responseStream = context.Response.OutputStream;
                    JsonSerializer.Serialize(responseStream, users);
                    responseStream.Flush();
                }
                else if (result is string responseString)
                {
                    context.Response.ContentType = "text/plain";
                    if (responseString == "Successful")
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                    else
                        context.Response.StatusCode = (int)HttpStatusCode.NotExtended;

                    using var writer = new StreamWriter(context.Response.OutputStream);
                    writer.Write(responseString);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        });
    }

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

