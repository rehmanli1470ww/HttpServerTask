
using System.ComponentModel.DataAnnotations;
using System.Net;


using HttpListener listener = new HttpListener();

listener.Prefixes.Add("http://127.0.0.1:27001/");
listener.Prefixes.Add("http://127.0.0.1:27002/");
listener.Prefixes.Add("http://localhost:27003/");

listener.Start();


while (true)
{


    string folderPath = "MyWebSite";


    string[] files = Directory.GetFiles(folderPath);




    var context = listener.GetContext();

    _ = Task.Run(() =>
    {
        HttpListenerRequest? request = context.Request;

        HttpListenerResponse? response = context.Response;
        response.ContentType = "text/html";
        response.Headers.Add("Content-Type", "text/html");
        response.Headers.Add("Server", "Step");
        response.Headers.Add("Date", DateTime.Now.ToString());

        var url = request.RawUrl;

        if (url == "/")
        {
            response.StatusCode = 200;


            // Data Content
            using var writer = new StreamWriter(response.OutputStream);

            var index = File.ReadAllText("MyWebSite/index.html");
            writer.Write(index);
        }
        else
        {

            var urls = url?.Split('/').ToList();


            if (urls[1] == "MyWebSite")
            {

                foreach (string file in files)
                {
                    var temp = file.Split('\\').ToList();
                    var temp1 = $"{temp}.html";
                    var temp3 = $"{urls[2]}.html";

                    if (temp[1] == urls[2] || temp1 == temp3)
                    {
                        response.StatusCode = 200;
                        using var writer = new StreamWriter(response.OutputStream);

                        var index = File.ReadAllText($"MyWebSite/{temp[1]}");
                        writer.Write(index);

                    }

                }


            }
            else
            {
                response.StatusCode = 404;

                using var writer = new StreamWriter(response.OutputStream);

                var index = File.ReadAllText("MyWebSite/404.html");
                writer.Write(index);
            }
        }
    });

}






