using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace SML.Models {
    public class Replays {

        public class ReplayData {
            public string spy_username { get; set; }
            public string sniper_username { get; set; }
            public string result { get; set; }
            public string level { get; set; }
            public string[] selected_missions { get; set; }
            public string[] picked_missions { get; set; }
            public string[] completed_missions { get; set; }
            public int sequence_number { get; set; }
            public DateTime start_time { get; set; }
            public int duration { get; set; }
            public string game_type { get; set; }
            public string uuid { get; set; }
            public string map_variant { get; set; }
            public string spy_displayname { get; set; }
            public string sniper_displayname { get; set; }
            public int guest_count { get; set; }
            public int clock_seconds { get; set; }
            public string file_path { get; set; }
        }

        public static string GetData(string filePath) {
            string pythonExe = HttpContext.Current.Server.MapPath("~/Python313/python.exe");
            string script = HttpContext.Current.Server.MapPath("~/SpyPartyParse-master/SpyPartyParser.py");

            var psi = new ProcessStartInfo {
                FileName = pythonExe,
                Arguments = $"\"{script}\" \"{filePath}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = Path.GetDirectoryName(script)
            };

            var errors = "";
            var jsonResults = "";

            Debug.WriteLine("Running SpyPartyParser.py...");
            Debug.WriteLine($"Arguments: {psi.Arguments}");

            using (var process = Process.Start(psi)) {
                errors = process.StandardError.ReadToEnd();
                jsonResults = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }

            if (!string.IsNullOrEmpty(errors)) {
                Debug.WriteLine("Python stderr:");
                Debug.WriteLine(errors);
            }

            Debug.WriteLine($"Python stdout: {jsonResults}");

            return errors + jsonResults;
        }


        public static ReplayData ReadFile(string filePath) {
            //ParserServiceReference.ParserServiceClient parserDataConnection = new ParserServiceReference.ParserServiceClient();

            //string response = parserDataConnection.GetData(filePath);
            string response = GetData(filePath);
            response = response.Replace("\'", "\"");

            try {
                ReplayData jsonObject = JsonConvert.DeserializeObject<ReplayData>(response);
                jsonObject.file_path = filePath;
                return jsonObject;
            }
            catch (Exception e) {
                Debug.WriteLine("Error in ReadFile of SML.Models.Replays");
                response = response + e.Message + e.StackTrace;
                return null;
            }
        }


        public static string StringFormatJSON(ReplayData jsonObject) {

            var formattedInfo = 
            $"Spy Username: {jsonObject.spy_username}\n" +
            $"Sniper Username: {jsonObject.sniper_username}\n" +
            $"Result: {jsonObject.result}\n" +
            $"Venue: {jsonObject.level}\n" +
            $"Selected Missions: {string.Join(", ", jsonObject.selected_missions)}\n" +
            $"Picked Missions: {string.Join(", ", jsonObject.picked_missions)}\n" +
            $"Completed Missions: {string.Join(", ", jsonObject.completed_missions)}\n" +
            $"Sequence Number: {jsonObject.sequence_number}\n" +
            $"Start Time: {jsonObject.start_time.ToString("yyyy-MM-dd HH:mm:ss")}\n" +
            $"Duration: {jsonObject.duration}\n" +
            $"Game Type: {jsonObject.game_type}\n" +
            $"UUID: {jsonObject.uuid}\n" +
            $"Map Variant: {jsonObject.map_variant}\n" +
            $"Spy Name: {jsonObject.spy_displayname}\n" +
            $"Sniper Name: {jsonObject.sniper_displayname}\n" +
            $"Guest Count: {jsonObject.guest_count}\n" +
            $"Start Clock Seconds: {jsonObject.clock_seconds}";

            return formattedInfo;
        }

        //var replay = new Replays.ReplayData {
        //    spy_username = "TestSpy",
        //    sniper_username = "TestSniper",
        //    result = "Win",
        //    level = "TestLevel",
        //    selected_missions = new[] { "Mission1", "Mission2" },
        //    picked_missions = new[] { "Mission1" },
        //    completed_missions = new[] { "Mission1" },
        //    sequence_number = 1,
        //    start_time = DateTime.Now,
        //    duration = 300,
        //    game_type = "TestGame",
        //    uuid = Guid.NewGuid().ToString(),
        //    map_variant = "Variant1",
        //    spy_displayname = "SpyTestName",
        //    sniper_displayname = "SniperTestName",
        //    guest_count = 2,
        //    clock_seconds = 90
        //};
    }
}
