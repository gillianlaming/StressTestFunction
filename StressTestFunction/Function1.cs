using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace StressTestFunction
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
			log.LogInformation("starting the stress test");
			Guid instanceGuid = Guid.NewGuid();
			string rootDir = @"D:\home\site\wwwroot";
			string pathToReadFile = Path.Combine(rootDir, instanceGuid.ToString(), "TestDirectory0", "TestFile0.bin");
			string folderPath = Path.Combine(rootDir, instanceGuid.ToString());
			Directory.CreateDirectory(folderPath);

			string applicationFilePath = @"D:\home\site\wwwroot\IOTests.exe";
			log.LogInformation($"Path to write files is {folderPath}");
			log.LogInformation($"application path is {applicationFilePath}");

			Process.Start(applicationFilePath, $"writeperftest -p {folderPath} -c 3000 -cs 1000 -t 5 -cc 1000 -s 1 -v 1 -dc 1 -wt 1 -fc 1");
			Process.Start(applicationFilePath, $"testfileopenclose {pathToReadFile} 100000");
		}
	}
}
