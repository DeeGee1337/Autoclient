using System;
using System.Diagnostics;
using System.Net;
using System.Text;

//auth https://hextechdocs.dev/getting-started-with-the-lcu-api/

namespace Autoleague
{
    //Classes
    class Program
    {
        //Variables
        public static bool League_client_is_open = false;
        public static string[] League_client_auth;
        public static bool buffer = false;
        public static int LUC_Process_id = 0;

        //Picking
        public static bool picked_buffer = false;
        public static bool locked_buffer = false;
        public static bool banned_buffer = false;
        public static bool locked_ban_buffer = false;

        //CFG
        public static bool Auto_accept = true;
        public static bool Auto_ban = true;
        public static bool Auto_pick = true;
        public static int currentChamp = (int)Champions.Brand;
        public static int currentBan = (int)Champions.Annie;

        //champselect
        public static string Chatroom_last = "";
        public static string act_ID_last = "";
        public static long act_ID_last_StartTime;

        //ChampsEnum
        enum Champions
        {
            Unknown,
            Aatrox = 266,
            Ahri = 103,
            Akali = 84,
            Alistar = 12,
            Amumu = 32,
            Anivia = 34,
            Annie = 1,
            Ashe = 22,
            AurelionSol = 136,
            Azir = 268,
            Bard = 432,
            Blitzcrank = 53,
            Brand = 63,
            Braum = 201,
            Caitlyn = 51,
            Camille = 164,
            Cassiopeia = 69,
            Chogath = 31,
            Corki = 42,
            Darius = 122,
            Diana = 131,
            Draven = 119,
            DrMundo = 36,
            Ekko = 245,
            Elise = 60,
            Evelynn = 28,
            Ezreal = 81,
            Fiddlesticks = 9,
            Fiora = 114,
            Fizz = 105,
            Galio = 3,
            Gangplank = 41,
            Garen = 86,
            Gnar = 150,
            Gragas = 79,
            Graves = 104,
            Hecarim = 120,
            Heimerdinger = 74,
            Illaoi = 420,
            Irelia = 39,
            Ivern = 427,
            Janna = 40,
            JarvanIV = 59,
            Jax = 24,
            Jayce = 126,
            Jhin = 202,
            Jinx = 222,
            Kaisa = 145,
            Kalista = 429,
            Karma = 43,
            Karthus = 30,
            Kassadin = 38,
            Katarina = 55,
            Kayle = 10,
            Kayn = 141,
            Kennen = 85,
            Khazix = 121,
            Kindred = 203,
            Kled = 240,
            KogMaw = 96,
            Leblanc = 7,
            LeeSin = 64,
            Leona = 89,
            Lissandra = 127,
            Lucian = 236,
            Lulu = 117,
            Lux = 99,
            Malphite = 54,
            Malzahar = 90,
            Maokai = 57,
            MasterYi = 11,
            MissFortune = 21,
            MonkeyKing = 62,
            Mordekaiser = 82,
            Morgana = 25,
            Nami = 267,
            Nasus = 75,
            Nautilus = 111,
            Neeko = 518,
            Nidalee = 76,
            Nocturne = 56,
            Nunu = 20,
            Olaf = 2,
            Orianna = 61,
            Ornn = 516,
            Pantheon = 80,
            Poppy = 78,
            Pyke = 555,
            Qiyana = 246,
            Quinn = 133,
            Rakan = 497,
            Rammus = 33,
            RekSai = 421,
            Renekton = 58,
            Rengar = 107,
            Riven = 92,
            Rumble = 68,
            Ryze = 13,
            Sejuani = 113,
            Shaco = 35,
            Shen = 98,
            Shyvana = 102,
            Singed = 27,
            Sion = 14,
            Sivir = 15,
            Skarner = 72,
            Sona = 37,
            Soraka = 16,
            Swain = 50,
            Sylas = 517,
            Syndra = 134,
            TahmKench = 223,
            Taliyah = 163,
            Talon = 91,
            Taric = 44,
            Teemo = 17,
            Thresh = 412,
            Tristana = 18,
            Trundle = 48,
            Tryndamere = 23,
            TwistedFate = 4,
            Twitch = 29,
            Udyr = 77,
            Urgot = 6,
            Varus = 110,
            Vayne = 67,
            Veigar = 45,
            Velkoz = 161,
            Vi = 254,
            Viktor = 112,
            Vladimir = 8,
            Volibear = 106,
            Warwick = 19,
            Xayah = 498,
            Xerath = 101,
            XinZhao = 5,
            Yasuo = 157,
            Yorick = 83,
            Yuumi = 350,
            Zac = 154,
            Zed = 238,
            Ziggs = 115,
            Zilean = 26,
            Zoe = 142,
            Zyra = 143,
            Senna = 235,
            Aphelios = 523,
            Sett = 875,
            Lillia = 876,
            Yone = 777,
            Samira = 360,
            Seraphine = 147,
            Gwen = 887,
            Rell = 526,
            Viego = 234,
            Akshan = 166,
            Vex = 711,
            Zeri = 221,
            Max
        };

        //Functions
        private static void Check_League_Client_started()
        {
            Console.WriteLine("[CLIENT] Check_League_Client_started called");
            Console.WriteLine("[CLIENT] Entering while true... ");

            while (true)
            {
                //get client process
                Console.WriteLine("[CLIENT] Waiting for League client...");
                Process client = Process.GetProcessesByName("LeagueClientUx").FirstOrDefault();

                //nullptr check
                if (client != null)
                {
                    Console.WriteLine("[CLIENT] ProcessID: " + client.Id);
                    League_client_auth = Auth_with_LUC_api(client);
                    buffer = true;
                    League_client_is_open = true;

                    if (LUC_Process_id != client.Id)
                    {
                        Console.WriteLine("[CLIENT] ProcessID check");
                        LUC_Process_id = client.Id;

                        if (Auto_accept)
                        {
                            Console.WriteLine("[CLIENT] Auto_accept = true");
                            Auto_accept = true;
                        }

                        if (Auto_ban)
                        {

                        }
                    }
                    //else
                    //{
                     //   League_client_is_open = false;
                      //  Auto_accept = false;

                        //Console.WriteLine("[CLIENT] League client waiting...");
                    //}
                    Thread.Sleep(2000);
                }
            }
        }

        private static string[] Auth_with_LUC_api(Process process_input)
        {
            Console.WriteLine("[AUTH] Auth_with_LUC_api called");

            //cmdmeme
            Process cmd = new Process();
            Console.WriteLine("[AUTH] Starting CMD.exe..");
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            //riot api codenz
            //On Windows this command looks like this: wmic PROCESS WHERE name='LeagueClientUx.exe' GET commandline and here's the Mac counterpart: ps -A | grep LeagueClientUx

            cmd.StandardInput.WriteLine("wmic process where 'Processid=" + process_input.Id + "' get Commandline");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();

            //get Port
            //bsp: "D:/Riot Games/League of Legends/LeagueClientUx.exe" "--riotclient-auth-token=osq5jjvbzioaIm62dURmpw" "--riotclient-app-port=63296" "--no-rads" "--disable-self-update" "--region=EUW" "--locale=en_GB" "--remoting-auth-token=C4DHVvrVUsKqvLrYJFZIgg" "--respawn-command=LeagueClient.exe" "--respawn-display-name=League of Legends" "--app-port=63359" "--install-directory=D:\Riot Games\League of Legends" "--app-name=LeagueClient" "--ux-name=LeagueClientUx" "--ux-helper-name=LeagueClientUxHelper" "--log-dir=LeagueClient Logs" "--crash-reporting=crashpad" "--crash-environment=EUW1" "--crash-pipe=\\.\pipe\crashpad_5432_HZRJBWVRTIBKLLYP" "--app-log-file-path=D:/Riot Games/League of Legends/Logs/LeagueClient Logs/2022-05-02T18-51-26_5432_LeagueClient.log" "--app-pid=5432" "--output-base-dir=D:\Riot Games\League of Legends" "--no-proxy-server"
            string CMD_output = cmd.StandardOutput.ReadToEnd();
            //Console.WriteLine("[AUTH] API CALL:" + CMD_output);
            Console.WriteLine("[AUTH] Parsing Output..");

            string Client_port = CMD_output.Split("--app-port=")[1].Split('"')[0];
            Console.WriteLine("[AUTH] Client Port:" + Client_port);

            string Client_key = CMD_output.Split("--remoting-auth-token=")[1].Split('"')[0];
            Console.WriteLine("[AUTH] Client Port:" + Client_key);

            //Get_key
            string Auth_for_rito = "riot:" + Client_key;

            var Auth_plain_text = System.Text.Encoding.UTF8.GetBytes(Auth_for_rito);
            Console.WriteLine("[AUTH] Encoding in Plaintext..");

            string Auth_Base64 = System.Convert.ToBase64String(Auth_plain_text);
            Console.WriteLine("[AUTH] Auth_Base64:" + Auth_Base64);

            //Return
            string[] output = { Auth_Base64, Client_port };
            return output;
        }

        private static string[] make_client_api_request(string[] League_auth, string method, string url, string body)
        {
            //Ignore invalid https pasta thanks C#
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            //sadly needed cuz crashes from webrequest
            if (buffer == true)
            {
                 WebRequest request = WebRequest.Create("https://127.0.0.1:" + League_auth[1] + "/" + url);
                 //Console.WriteLine("[REQUEST] Webrequest URL... ");

                // Set headers
                request.Method = method;
                request.Headers.Add("Authorization", "Basic " + League_auth[0]);
                request.ContentType = "application/json";

                // Send POST data when doing a post request
                Stream dataStream;
                if (method == "POST" || method == "PUT" || method == "PATCH")
                {
                    string postData = body;
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = byteArray.Length;
                    dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }

                // Get the response
                WebResponse response;
                try
                {
                    response = request.GetResponse();
                }
                catch (WebException e)
                {
                    response = e.Response;
                }

                // If the response is null (League client closed?)
                if (((HttpWebResponse)response) == null)
                {
                    string[] outputDef = { "999", "" };
                    return outputDef;
                }

                // Get the HTTP status code
                int statusCode = (int)((HttpWebResponse)response).StatusCode;
                string statusString = statusCode.ToString();

                // Get the body
                string responseFromServer = new StreamReader(response.GetResponseStream()).ReadToEnd();

                // Clean up the stream
                response.Close();

                // Return content
                string[] output = { statusString, responseFromServer };
                return output;
            }
            else
            {
                //URL is invalid
                string[] output = { "999", "" };
                return output;
            }

        }

        private static void Accept_Game()
        {
            Console.WriteLine("[ACCEPT] INIT 3 Sec...");
            Thread.Sleep(3000);

            while(true)
            {
                if(true)
                {
                    //https://github.com/HextechDocs/lcu-explorer/releases/tag/1.2.0 for the packets
                    //https://i.imgur.com/GAp5fXF.png my screenshot
                    string[] Game_session = make_client_api_request(League_client_auth, "GET", "lol-gameflow/v1/session", "");
                    //Console.WriteLine("[ACCEPT] Session: recieved");

                    //Session found
                    if (Game_session[0] == "200")
                    {
                        //Gamestate
                        string Game_phase = Game_session[1].Split("phase").Last().Split('"')[2];
                        Console.WriteLine("[ACCEPT] State: " + Game_phase);

                        if (Game_phase == "Lobby")
                        {
                            continue;
                        }
                        if (Game_phase == "Matchmaking")
                        {
                            continue;
                        }
                        if (Game_phase == "ReadyCheck")
                        {
                            if (Auto_accept)
                            {
                                // Accept queue if found
                                string[] match_accept = make_client_api_request(League_client_auth, "POST", "lol-matchmaking/v1/ready-check/accept", "");

                                if (match_accept[0] == "204")
                                {
                                    Console.WriteLine("[ACCEPT] GAME ACCEPTED!");
                                }
                            }
                        }
                        if (Game_phase == "ChampSelect")
                        {
                            if (Auto_ban)
                            {
                                //blubb
                            }
                            if (Auto_pick)
                            {
                                // Get data for the current ongoing champ select
                                string[] current_champ_select = make_client_api_request(League_client_auth, "GET", "lol-champ-select/v1/session", "");

                                if (current_champ_select[0] == "200")
                                {
                                    // Get needed data from the current champ select
                                    string Chatroom_current = current_champ_select[1].Split("chatRoomName\":\"")[1].Split('"')[0];

                                    if (Chatroom_last != Chatroom_current || Chatroom_last == "")
                                    {
                                        // Reset stuff in case someone dodged the champ select
                                        picked_buffer = false;
                                        locked_buffer = false;
                                        banned_buffer = false;
                                        locked_ban_buffer = false;
                                        //pickedSpell0 = false;
                                        //pickedSpell1 = false;
                                    }

                                    Chatroom_last = Chatroom_current;

                                    if (picked_buffer && locked_buffer && banned_buffer && locked_ban_buffer /* && pickedSpell0 && pickedSpell1*/)
                                        Thread.Sleep(1000);

                                    else
                                    {
                                        // Get more needed data from the current champ select
                                        string localplayer_cell_id = current_champ_select[1].Split("localPlayerCellId\":")[1].Split(',')[0];

                                        if (currentChamp == 0)
                                        {
                                            picked_buffer = true;
                                            locked_buffer = true;
                                        }
                                        if (currentBan == 0)
                                        {
                                            banned_buffer = true;
                                            locked_ban_buffer = true;
                                        }
                                        //if (currentSpell0[1] == "0")
                                        //{
                                        //    pickedSpell0 = true;
                                        //}
                                        //if (currentSpell1[1] == "0")
                                        //{
                                        //    pickedSpell1 = true;
                                        //}
                                        if (!picked_buffer || !locked_buffer || !banned_buffer || !locked_ban_buffer)
                                        {
                                            // First make sure we are actually able to pick a champion
                                            string csActs = current_champ_select[1].Split("actions\":[[{")[1].Split("}]],")[0];
                                            csActs = csActs.Replace("}],[{", "},{");
                                            string[] csActsArr = csActs.Split("},{");
                                            
                                            foreach (var act in csActsArr)
                                            {
                                                string ActCctorCellId = act.Split("actorCellId\":")[1].Split(',')[0];
                                                string championId = act.Split("championId\":")[1].Split(',')[0];
                                                string actId = act.Split(",\"id\":")[1].Split(',')[0];
                                                string ActCompleted = act.Split("completed\":")[1].Split(',')[0];
                                                string ActIsInProgress = act.Split("isInProgress\":")[1].Split(',')[0];
                                                string ActType = act.Split("type\":\"")[1].Split('"')[0];

                                                if (ActCctorCellId == localplayer_cell_id && ActCompleted == "false" && ActType == "pick")
                                                {
                                                    if (!picked_buffer)
                                                    {
                                                        // hover champion when champ select starts, no need to check for whenever it's my turn or not to pick it
                                                        string[] champSelectAction = make_client_api_request(League_client_auth, "PATCH", "lol-champ-select/v1/session/actions/" + actId, "{\"championId\":" + currentChamp + "}");
                                                        if (champSelectAction[0] == "204")
                                                        {
                                                            picked_buffer = true;
                                                        }
                                                    }
                                                    // ActIsInProgress makes sure it's my turn to pick the champion
                                                    if (ActIsInProgress == "true")
                                                    {
                                                        // Mark the start of the phase
                                                        if (actId != act_ID_last)
                                                        {
                                                            act_ID_last = actId;
                                                            act_ID_last_StartTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                                                        }
                                                        if (!locked_buffer)
                                                        {
                                                            string[] champSelectAction = make_client_api_request(League_client_auth, "PATCH", "lol-champ-select/v1/session/actions/" + actId, "{\"completed\":true,\"championId\":" + championId + "}");

                                                            if (champSelectAction[0] == "204")
                                                            {
                                                                locked_buffer = true;
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (ActCctorCellId == localplayer_cell_id && ActCompleted == "false" && ActType == "ban")
                                                {
                                                    string champSelectPhase = current_champ_select[1].Split("\"phase\":\"")[1].Split('"')[0];

                                                    // ActIsInProgress makes sure it's my turn to pick the champion
                                                    if (ActIsInProgress == "true" && champSelectPhase != "PLANNING")
                                                    {
                                                        // Mark the start of the phase
                                                        if (actId != act_ID_last)
                                                        {
                                                            act_ID_last = actId;
                                                            act_ID_last_StartTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                                                        }

                                                        if (!banned_buffer)
                                                        {
                                                            string[] champSelectAction = make_client_api_request(League_client_auth, "PATCH", "lol-champ-select/v1/session/actions/" + actId, "{\"championId\":" + currentBan + "}");
                                                            if (champSelectAction[0] == "204")
                                                            {
                                                                banned_buffer = true;
                                                                Thread.Sleep(1000);
                                                            }
                                                        }
                                                        else if (!locked_ban_buffer)
                                                        {
                                                             string[] champSelectAction = make_client_api_request(League_client_auth, "PATCH", "lol-champ-select/v1/session/actions/" + actId, "{\"completed\":true,\"championId\":" + championId + "}");

                                                             if (champSelectAction[0] == "204")
                                                             {
                                                                locked_ban_buffer = true;
                                                             }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    // Failed to get champ select data, should probably retry. maybe we did it too early? maybe the game was dodged?
                                    // state = "Failed to get champ select data";
                                }
                            }
                        }
                    }
                }
            }
        }


        static void Main(string[] args)
        {
            Console.SetWindowSize(120, 30);
            Console.Title = "DeeGee's Autoclient";
            Console.WriteLine("Hello DeeGee!");

            //Threads here ->
            var Thread_league_is_alive = new Task(Check_League_Client_started);
            Thread_league_is_alive.Start();

            var accept_game = new Task(Accept_Game);
            accept_game.Start();

            //wait
            var tasks = new[] { accept_game, Thread_league_is_alive };
            Task.WaitAll(tasks);

            Console.WriteLine("[MAIN] Tasks finished!");
            return;
        }
    }
}