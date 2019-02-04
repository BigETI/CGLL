using System.IO;
using System.Runtime.Serialization;

/// <summary>
/// Community game launcher library namespace
/// </summary>
namespace CGLL
{
    /// <summary>
    /// Game launch options Data contract
    /// </summary>
    [DataContract]
    public class GameLaunchOptionsDataContract
    {
        /// <summary>
        /// Game path
        /// </summary>
        [DataMember]
        private string gamePath;

        /// <summary>
        /// Working directory
        /// </summary>
        [DataMember]
        private string workingDirectory;

        /// <summary>
        /// Launch parameters
        /// </summary>
        [DataMember]
        private string launchParameters;

        /// <summary>
        /// Create session log
        /// </summary>
        [DataMember]
        private bool createSessionLog;

        /// <summary>
        /// Session log resource paths
        /// </summary>
        [DataMember]
        private string[] sessionLogResourcePaths;

        /// <summary>
        /// Plugins
        /// </summary>
        [DataMember]
        private string[] plugins;

        /// <summary>
        /// Game path
        /// </summary>
        public string GamePath
        {
            get
            {
                if (gamePath == null)
                {
                    gamePath = "";
                }
                return gamePath;
            }
        }

        /// <summary>
        /// Working directory
        /// </summary>
        public string WorkingDirectory
        {
            get
            {
                if (workingDirectory == null)
                {
                    workingDirectory = "";
                }
                if (workingDirectory.Trim().Length <= 0)
                {
                    workingDirectory = Path.GetDirectoryName(GamePath);
                }
                return workingDirectory;
            }
        }

        /// <summary>
        /// Launch parameters
        /// </summary>
        public string LaunchParameters
        {
            get
            {
                if (launchParameters == null)
                {
                    launchParameters = "";
                }
                return launchParameters;
            }
        }

        /// <summary>
        /// Create session log
        /// </summary>
        public bool CreateSessionLog => createSessionLog;

        /// <summary>
        /// Session log resource paths
        /// </summary>
        public string[] SessionLogResourcePaths
        {
            get
            {
                if (sessionLogResourcePaths == null)
                {
                    sessionLogResourcePaths = new string[0];
                }
                return sessionLogResourcePaths;
            }
        }

        /// <summary>
        /// Plugins
        /// </summary>
        public string[] Plugins
        {
            get
            {
                if (plugins == null)
                {
                    plugins = new string[0];
                }
                return plugins;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gamePath">Game path</param>
        /// <param name="workingDirectory">Working directory (optional)</param>
        /// <param name="launchParameters">Launch parameters (optional)</param>
        /// <param name="createSessionLog">Create session log</param>
        /// <param name="sessionLogResourcePaths">Session log resource paths</param>
        /// <param name="plugins">Plugins (optional)</param>
        public GameLaunchOptionsDataContract(string gamePath, string workingDirectory, string launchParameters, bool createSessionLog, string[] sessionLogResourcePaths, string[] plugins)
        {
            this.gamePath = gamePath;
            this.workingDirectory = workingDirectory;
            this.launchParameters = launchParameters;
            this.createSessionLog = createSessionLog;
            this.sessionLogResourcePaths = sessionLogResourcePaths;
            this.plugins = plugins;
        }
    }
}
