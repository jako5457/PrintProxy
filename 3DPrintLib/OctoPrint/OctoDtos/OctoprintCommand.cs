using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace _3DPrintLib.OctoPrint.OctoDtos
{
    internal class OctoprintCommand
    {

        public OctoprintCommand(string command)
        {
            Command = command;
        }

        [JsonPropertyName("command")]
        public string Command { get; set; } = string.Empty;
    }

    internal class OctoprintFileCommand : OctoprintCommand
    {

        public OctoprintFileCommand(string command,string file,bool print = false) : base(command)
        {
            Print = print;
            File = file;
        }

        public bool Print { get; set; }

        [JsonIgnore]
        public string File { get; set; } = string.Empty;
    }

    internal class OctoprintActionCommand : OctoprintCommand
    {

        public OctoprintActionCommand(string command, string action) : base(command)
        {
            Action = action;
        }

        [JsonPropertyName("action")]
        public string Action { get; set; }
    }

}
