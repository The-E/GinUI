using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Updater
{
    public class UpdateChecker
    {
        private static readonly DirectoryInfo UpdaterBaseDirectory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\g-node\GinWindowsClient\Updates\");

        private static readonly string UpdatedMsi =
            "https://web.gin.g-node.org/achilleas/gin-ui-installers/raw/master/Setup.msi";
        
        public static void DoUpdate()
        {         
            if (!UninstallProgram("Gin Windows Client")) return;
            var wb = new WebClient();
            wb.DownloadFile(new Uri(UpdatedMsi), UpdaterBaseDirectory + @"\setup.msi");
            var procstartinfo = new ProcessStartInfo();
            procstartinfo.FileName = "msiexec.exe";
            procstartinfo.Arguments = "/i \"" + UpdaterBaseDirectory.FullName + "\\setup.msi\"";
            procstartinfo.CreateNoWindow = true;
            var process = Process.Start(procstartinfo);
            process.WaitForExit();
        }

        private static bool UninstallProgram(string ProgramName)
        {
            try
            {
                var installedPrograms =
                    GetSubkeysValue(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                        RegistryHive.LocalMachine);

                foreach (var key in installedPrograms)
                {
                    if ((from value in key.Values where value.Key == "DisplayName" select value.Value as string).Any(s => s != null &&
                                                                                                                          s == "Gin Windows Client"))
                    {
                        var uninstallString = (from value in key.Values
                                               where value.Key == "UninstallString"
                                               select value.Value as string).First();

                        uninstallString = uninstallString.Replace("/I", "/x");
                        var psInfo = new ProcessStartInfo();
                        psInfo.FileName = "cmd.exe";
                        psInfo.Arguments = "/C " + uninstallString + " /q";
                        psInfo.CreateNoWindow = true;
                        var process = Process.Start(psInfo);
                        process.WaitForExit();
                        return process.ExitCode == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        class Key
        {
            public string KeyName { get; set; }
            public List<KeyValuePair<string, object>> Values { get; set; }
        }

        private static List<Key> GetSubkeysValue(string path, RegistryHive hive)
        {
            var result = new List<Key>();
            using (var hiveKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default))
            using (var key = hiveKey.OpenSubKey(path))
            {
                var subkeys = key.GetSubKeyNames();

                foreach (var subkey in subkeys)
                {
                    var values = GetKeyValue(key, subkey);
                    result.Add(values);
                }
            }
            return result;
        }

        private static Key GetKeyValue(RegistryKey hive, string keyName)
        {
            var result = new Key() { KeyName = keyName, Values = new List<KeyValuePair<string, object>>() };
            var key = hive.OpenSubKey(keyName);
            if (key != null)
            {
                foreach (var valueName in key.GetValueNames())
                {
                    var val = key.GetValue(valueName);
                    var pair = new KeyValuePair<string, object>(valueName, val);
                    result.Values.Add(pair);
                }
            }

            return result;
        }
    }
}


public class NuGetFeed
{
    public string id { get; set; }
    public string name { get; set; }
    public int accountId { get; set; }
    public int projectId { get; set; }
    public bool isPrivateProject { get; set; }
    public bool publishingEnabled { get; set; }
    public DateTime created { get; set; }
}

public class AccessRightDefinition
{
    public string name { get; set; }
    public string description { get; set; }
}

public class AccessRight
{
    public string name { get; set; }
    public bool allowed { get; set; }
}

public class RoleAce
{
    public int roleId { get; set; }
    public string name { get; set; }
    public bool isAdmin { get; set; }
    public List<AccessRight> accessRights { get; set; }
}

public class SecurityDescriptor
{
    public List<AccessRightDefinition> accessRightDefinitions { get; set; }
    public List<RoleAce> roleAces { get; set; }
}

public class Project
{
    public int projectId { get; set; }
    public int accountId { get; set; }
    public string accountName { get; set; }
    public List<object> builds { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
    public string repositoryType { get; set; }
    public string repositoryScm { get; set; }
    public string repositoryName { get; set; }
    public string repositoryBranch { get; set; }
    public bool isPrivate { get; set; }
    public bool skipBranchesWithoutAppveyorYml { get; set; }
    public bool enableSecureVariablesInPullRequests { get; set; }
    public bool enableSecureVariablesInPullRequestsFromSameRepo { get; set; }
    public bool enableDeploymentInPullRequests { get; set; }
    public bool saveBuildCacheInPullRequests { get; set; }
    public bool rollingBuilds { get; set; }
    public bool rollingBuildsDoNotCancelRunningBuilds { get; set; }
    public bool rollingBuildsOnlyForPullRequests { get; set; }
    public bool alwaysBuildClosedPullRequests { get; set; }
    public string tags { get; set; }
    public NuGetFeed nuGetFeed { get; set; }
    public SecurityDescriptor securityDescriptor { get; set; }
    public DateTime created { get; set; }
    public DateTime updated { get; set; }
}

public class Job
{
    public string jobId { get; set; }
    public string name { get; set; }
    public string osType { get; set; }
    public bool allowFailure { get; set; }
    public int messagesCount { get; set; }
    public int compilationMessagesCount { get; set; }
    public int compilationErrorsCount { get; set; }
    public int compilationWarningsCount { get; set; }
    public int testsCount { get; set; }
    public int passedTestsCount { get; set; }
    public int failedTestsCount { get; set; }
    public int artifactsCount { get; set; }
    public string status { get; set; }
    public DateTime started { get; set; }
    public DateTime finished { get; set; }
    public DateTime created { get; set; }
    public DateTime updated { get; set; }
}

public class Build
{
    public int buildId { get; set; }
    public List<Job> jobs { get; set; }
    public int buildNumber { get; set; }
    public string version { get; set; }
    public string message { get; set; }
    public string messageExtended { get; set; }
    public string branch { get; set; }
    public bool isTag { get; set; }
    public string commitId { get; set; }
    public string authorName { get; set; }
    public string authorUsername { get; set; }
    public string committerName { get; set; }
    public string committerUsername { get; set; }
    public DateTime committed { get; set; }
    public List<object> messages { get; set; }
    public string status { get; set; }
    public DateTime started { get; set; }
    public DateTime finished { get; set; }
    public DateTime created { get; set; }
    public DateTime updated { get; set; }
}

public class RootObject
{
    public Project project { get; set; }
    public Build build { get; set; }
}
