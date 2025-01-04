﻿using System.Diagnostics;

namespace PiGSF.Server
{
    class ServerCLI
    {
        static StreamReader cin = new StreamReader(Console.OpenStandardInput());
        static string ReadLine()
        {
            return cin!.ReadLineAsync()!.Result;
        }

        static int Main(string[] args)
        {
            Server server = null;
            ServerLogger.Log("Pi Game Server Framework by Pi-Dev");
            int port = int.Parse(ServerConfig.Get("bindPort"));
            server = new Server(port);

            var t = new Thread(() => server.Start());
            t.Name = "Server Thread";
            t.Start();

            while (!server.IsActive()) Thread.Sleep(16);
            while (server!.IsActive())
            {
                int current = 0, total = 0; 
                string prefix;
                var r = ServerLogger.currentRoomChannel;
                if (r == null)
                {
                    prefix = "[Server]";
                    server.knownPlayers.ForEach(p => { if (p.IsConnected()) current++; total++; });
                }
                else
                {
                    prefix = $"[{r.GetType().Name}:{r.Id}]";
                    prefix += $"[{r.Name}]";
                    r.players.ForEach(p => { if (p.IsConnected()) current++; total++; });
                }
                Console.Write($"{prefix}({current}/{total})> ");
                var input = ReadLine();
                server.HandleCommand(input);
            }

            cin.Dispose();
            t.Join();
            return 0;
        }
    }
}
