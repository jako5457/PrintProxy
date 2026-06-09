using System;
using System.Collections.Generic;
using System.Text;

namespace PrintLib
{
    public class PrinterStatus : JobStatus
    {
        /// <summary>
        /// The printer name
        /// </summary>
        public string PrinterName { get; set; } = string.Empty;

        /// <summary>
        /// the filename of the file printing
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// A link to a image of the file printing
        /// </summary>
        public string? FileThumbnail { get; set; } = null!;

        /// <summary>
        /// A link to a video stream of the printers camera
        /// </summary>
        public string? PrinterCam { get; set; }

        public string Identifier { get; set; } = string.Empty;

        /// <summary>
        /// Additional information about the printer
        /// </summary>
        public Dictionary<string, string> info { get; set; } = new();

        public void addJobStatus(JobStatus status)
        {
            Status = status.Status;
            Progress = status.Progress;
        }
    }
}
