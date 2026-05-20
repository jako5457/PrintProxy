using PrintLib;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrintLib
{
    public interface IPrinter
    {
        /// <summary>
        /// Prints the file. File must have been uploaded before calling this.
        /// </summary>
        /// <param name="fileName">The file to be printed</param>
        /// <returns></returns>
        public Task StartAsync(string fileName);

        /// <summary>
        /// Continues a paused print
        /// </summary>
        /// <returns></returns>
        public Task ContinueAsync();
        
        /// <summary>
        /// Pauses the print
        /// </summary>
        /// <returns></returns>
        public Task PauseAsync();

        /// <summary>
        /// Stops The Print
        /// </summary>
        /// <returns></returns>
        public Task StopAsync();

        /// <summary>
        /// Uploads the file to the printer
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public Task UploadAsync(string FilePath);

        /// <summary>
        /// Gives your the full printer information
        /// </summary>
        /// <returns>Full printer information</returns>
        public Task<PrinterStatus> GetStatusAsync();

        /// <summary>
        /// Gives you only the status of a print.
        /// </summary>
        /// <returns></returns>
        public Task<JobStatus> GetJobStatusAsync();
    }
}
