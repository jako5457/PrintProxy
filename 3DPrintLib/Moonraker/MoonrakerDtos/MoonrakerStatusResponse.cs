using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrintLib.Moonraker.MoonrakerDtos
{

    public class MoonRakerStatusResponse
    {
        public MoonRakerResult result { get; set; }
    }

    public class MoonRakerResult
    {
        public float eventtime { get; set; }
        public MoonRakerStatus status { get; set; }
    }

    public class MoonRakerStatus
    {
        public MoonRakerDisplay_Status display_status { get; set; }
        public MoonRakerPrint_Stats print_stats { get; set; }
    }

    public class MoonRakerDisplay_Status
    {
        public float progress { get; set; }
        public string message { get; set; }
    }

    public class MoonRakerPrint_Stats
    {
        public string filename { get; set; }
        public float total_duration { get; set; }
        public float print_duration { get; set; }
        public float filament_used { get; set; }
        public string state { get; set; }
        public string message { get; set; }
        public MoonRakerInfo info { get; set; }
    }

    public class MoonRakerInfo
    {
        public object total_layer { get; set; }
        public object current_layer { get; set; }
    }


}
