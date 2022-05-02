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

        //CFG
        public static bool Auto_accept = true;
        public static bool Auto_ban = false;

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
                if(Auto_accept)
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
                            // Accept queue if found
                            string[] matchAccept = make_client_api_request(League_client_auth, "POST", "lol-matchmaking/v1/ready-check/accept", "");

                            if (matchAccept[0] == "204")
                            {
                                Console.WriteLine("[ACCEPT] GAME ACCEPTED!");
                                return;
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