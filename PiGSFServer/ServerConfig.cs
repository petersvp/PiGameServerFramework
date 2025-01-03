﻿using Auth;
using System.Collections.ObjectModel;

namespace PiGSF.Server
{
    // Add extra things to the Server Player object
    public partial class Player
    {
        // These are used by the examples, but you are free to use them as you like
        public string team = "default";
        public string name = "Guest";
        public string username = "guest";
        // public int elo = 0; // Sample line

        public string ToTableString()
        {
            return
                /* Id  */ id.ToString().PadRight(5) + "| " +
                // /* ELO  */ elo.ToString().PadRight(5) + "| " +  // Sample line!
                /* username */ username.PadRight(16) + " | " +
                // /* name */ name.PadRight(32) + " | " +
                /* uid */ uid.PadRight(48) + " |";
        }
    }

    public static class ServerConfig
    {
        static ServerConfig()
        {
            // Default config, hard-coded, and very limited.
            // You are supposed to build the server and implement your game types
            var defaultConfig = new Dictionary<string, string>() {
                { "DefaultRoomTimeout", "30" },
                { "bindAddress", "0.0.0.0" },
                { "bindPort", "12345" },
                { "defaultRoom", "ChatRoom,Lobby" },
            };

            // parse config file
            var configFile = "PIGSFServerConfig.cfg";
            var fp = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/" + configFile;
            try
            {
                var config = File.ReadAllLines(fp).Select(s => s.Split("=", StringSplitOptions.TrimEntries)).ToDictionary(x => x[0], x => x[1]);
                foreach (var c in config) defaultConfig[c.Key] = c.Value;
            }
            catch (Exception ex)
            {
                ServerLogger.Log(ex.Message);
            }

            // Apply to the class
            ServerConfig.config = new ReadOnlyDictionary<string, string>(defaultConfig);
        }

        // Packet size and format
        public static uint HeaderSize = 2;
        public static uint PolledBuffersSize = 1024; // by default 1k
        public static uint MaxInitialPacketSize = 4 * 1024; // by default 4k for JWT payload

        // Implementation details
        static ReadOnlyDictionary<string, string> config;
        public static string Get(string key, string defval = "") => config.GetValueOrDefault(key, defval);

        // Room configuration

        // Time to keep room if no players reconnect
        public static int DefaultRoomTimeout = 5;

        // Authentication modules by default
        public static IAuthProvider[] authProviders = [new JWTAuth(), new NoAuth()];

        public static string JWTPrivateKey => LoadFileOrDefault("PIGSF-PRIVATE-JWT.PEM");
        public static string EncryptionPublicKey => LoadFileOrDefault("PIGSF-PUBLIC-RSA.PEM");
        public static string EncryptionPrivateKey => LoadFileOrDefault("PIGSF-PRIVATE-RSA.PEM");

        public static string LoadFileOrDefault(string fn, string def = "")
        {
            try { return File.ReadAllText(fn); }
            catch { return def; }
        }
    }
}
