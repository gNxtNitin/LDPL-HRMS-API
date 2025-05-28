using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorAndExceptionHandling.Library
{
    public class ErrorLoggingService: IErrorLoggingService
    {
        //private readonly FileManagementDbContext _context;

        //public ErrorLoggingService(FileManagementDbContext context)
        //{
        //    _context = context;
        //}

        private readonly string _logFilePath;

      
        public ErrorLoggingService(string logFilePath)
        {
            //_logFilePath = logFilePath;
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), logFilePath);
            _logFilePath = rootPath;
        }

        public async Task LogError(Exception exception, string controller, string action, string userId = null)
        {
            var errorLog = new ErrorLogModel
            {
                ExceptionMessage = exception.Message,
                StackTrace = exception.StackTrace,
                LogLevel = "Error", // You can implement more levels if needed
                Controller = controller,
                Action = action,
                UserId = userId
            };

            // _context.ErrorLogs.Add(errorLog);
            // await _context.SaveChangesAsync();

            // Log to file (or database, depending on your setup)
            await LogToFileAsync(errorLog);
        }

        private async Task LogToFileAsync(ErrorLogModel errorLog)
        {
            try
            {
                // Ensure the directory exists
                var directoryPath = Path.GetDirectoryName(_logFilePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Format the log message
                var logMessage = $"{errorLog.Timestamp:yyyy-MM-dd HH:mm:ss} | {errorLog.LogLevel} | {errorLog.Controller} | {errorLog.Action} | {errorLog.ExceptionMessage} | {errorLog.StackTrace}";

                // Append the log message to the log file
                await File.AppendAllTextAsync(_logFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during logging
                Console.WriteLine($"Failed to log error: {ex.Message}");
            }
        }
    }
}
