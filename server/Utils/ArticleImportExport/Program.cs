using System;
using System.Collections.Generic;
using Npgsql;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using Dapper;
using NpgsqlTypes;
using Console = Colorful.Console;

var runner = new Runner();
runner.Run();

public class Runner
{
    private bool quit;

    private string remoteConnectionString;
    private string localConnectionString;
    private string exportsDirectory;

    public void Run()
    {
        remoteConnectionString = ConfigurationManager.AppSettings.Get("RemoteConnectionString");
        localConnectionString = ConfigurationManager.AppSettings.Get("LocalConnectionString");
        exportsDirectory = ConfigurationManager.AppSettings.Get("ExportsDirectory");
        
        Console.WriteAscii("Blog Import Export", Color.Green);
        
        while (!quit)
        {
            Console.WriteLine("(E)xport, (I)mport, (Q)uit.", Color.Green);
            var option =  Console.ReadLine().ToLower().Trim();

            switch (option)
            {
                case "e":   
                    Export(); 
                    break;
                case "i":
                    Import();
                    break;
                case "q":
                    quit = true;
                    break;
            }
        }
    }

    private void Export()
    {
        Console.WriteLine("Export (L)ocal or (R)emote?", Color.Green);

        var env = Console.ReadLine().ToLower().Trim();

        var connectionString = env == "r" ? remoteConnectionString : localConnectionString;
        
        using var connection = new NpgsqlConnection(connectionString);
        var results = connection.Query<Post>("SELECT \"Id\", \"Title\", \"Slug\", \"Body\", \"HeaderImage\", \"PublishDate\", \"ReadMinutes\" FROM public.\"Posts\";").ToList();

        if (!results.Any())
        {
            Console.WriteLine("No records found", Color.Green);
            return;
        }


        File.AppendAllTextAsync($"{exportsDirectory}export_{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}-{env}.json", JsonSerializer.Serialize(results));
    }

    private void Import()
    {
        Console.WriteLine("Select import file - enter number and hit enter", Color.Green);
        var files = Directory.EnumerateFiles(exportsDirectory).ToList();

        var count = 1;
        foreach (var file in files)
        {
            Console.WriteLine($"{count++}. {file}", Color.Green);
        }

        Console.WriteLine("");
        var response = Console.ReadLine();

        if (!int.TryParse(response, out var number))
        {
            if (number < 1 || number > files.Count)
            {
                Console.WriteLine("Invalid answer", Color.Green);
                return;
            }
        }

        var posts = JsonSerializer.Deserialize<List<Post>>(File.ReadAllText(files[number-1]));

        Console.WriteLine("Import to (L)ocal or (R)emote?", Color.Green);

        var env = Console.ReadLine().ToLower().Trim();
        var connectionString = env == "r" ? remoteConnectionString : localConnectionString;
        using var connection = new NpgsqlConnection(connectionString);
        connection.Execute("DELETE FROM public.\"Posts\"");
        
        foreach (var post in posts)
        {
            connection.Execute(
                @"INSERT INTO public.""Posts""(""Title"", ""Slug"", ""Body"", ""HeaderImage"", ""PublishDate"", ""ReadMinutes"")

            VALUES('" + post.Title + "', '"+ post.Slug +"', '"+ post.Body+"', '"+ post.HeaderImage +"', '"+ new NpgsqlDateTime(post.PublishDate.DateTime) + "', '"+post.ReadMinutes +"'); ");
        }

    }
}

public record Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Body { get; set; }
    public string HeaderImage { get; set; }
    public DateTimeOffset PublishDate { get; set; }
    public int ReadMinutes { get; set; }
}
