using System.Diagnostics;

/// <summary>
/// Community game launcher library namespace
/// </summary>
namespace CGLL
{
    /// <summary>
    /// Game class
    /// </summary>
    /// <typeparam name="T">Session log user data type</typeparam>
    public class Game<T>
    {
        /// <summary>
        /// Game process
        /// </summary>
        public Process GameProcess { get; private set; }

        /// <summary>
        /// Game launch options
        /// </summary>
        public GameLaunchOptionsDataContract GameLaunchOptions { get; private set; }

        /// <summary>
        /// Last resources state
        /// </summary>
        public ResourcesState LastResourcesState { get; private set; }

        /// <summary>
        /// Last session log data
        /// </summary>
        public SessionLogDataContract<T> LastSessionLogData { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameProcess">Game process</param>
        /// <param name="gameLaunchOptions">Game launch options</param>
        /// <param name="lastResourcesState">Last resources state</param>
        /// <param name="lastSessionLogData">Last session log data</param>
        protected Game(Process gameProcess, GameLaunchOptionsDataContract gameLaunchOptions, ResourcesState lastResourcesState, SessionLogDataContract<T> lastSessionLogData)
        {
            GameProcess = gameProcess;
            GameLaunchOptions = gameLaunchOptions;
            LastResourcesState = lastResourcesState;
            LastSessionLogData = lastSessionLogData;
        }
    }
}
