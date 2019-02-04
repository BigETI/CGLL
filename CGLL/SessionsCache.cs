using System.Collections.Generic;

/// <summary>
/// Community game launcher library namespace
/// </summary>
namespace CGLL
{
    /// <summary>
    /// Sessions cache class
    /// </summary>
    public static class SessionsCache<T>
    {
        /// <summary>
        /// Session logs
        /// </summary>
        private static Dictionary<string, SessionLog<T>> sessionLogs = new Dictionary<string, SessionLog<T>>();

        /// <summary>
        /// Get session log
        /// </summary>
        /// <param name="path">Session log path</param>
        /// <returns>Session log</returns>
        public static SessionLog<T> GetSessionLog(string path)
        {
            SessionLog<T> ret = null;
            if (path != null)
            {
                string p = path.ToLower();
                lock (sessionLogs)
                {
                    if (sessionLogs.ContainsKey(p))
                    {
                        ret = sessionLogs[p];
                    }
                    else
                    {
                        ret = SessionLog<T>.Load(path);
                        if (ret != null)
                        {
                            sessionLogs.Add(p, ret);
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Clear session logs cache
        /// </summary>
        public static void Clear()
        {
            lock (sessionLogs)
            {
                sessionLogs.Clear();
            }
        }

        /// <summary>
        /// Remove session log from cache
        /// </summary>
        /// <param name="sessionLog">Session log</param>
        public static void Remove(SessionLog<T> sessionLog)
        {
            if (sessionLog != null)
            {
                string path = sessionLog.Path.ToLower();
                if (sessionLogs.ContainsKey(path))
                {
                    sessionLogs.Remove(path);
                    sessionLog.Dispose();
                }
            }
        }
    }
}
