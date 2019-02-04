using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Community game launcher library namespace
/// </summary>
namespace CGLL
{
    /// <summary>
    /// Session log provider class
    /// </summary>
    /// <typeparam name="T">User data type</typeparam>
    public static class SessionLogProvider<T>
    {
        /// <summary>
        /// Get session logs
        /// </summary>
        /// <param name="path">Session logs path</param>
        /// <returns></returns>
        public static SessionLog<T>[] GetSessionLogs(string path)
        {
            List<SessionLog<T>> list = new List<SessionLog<T>>();
            try
            {
                if (Directory.Exists(path))
                {
                    string[] files = Directory.GetFiles(path, "*.game-session", SearchOption.AllDirectories);
                    if (files != null)
                    {
                        foreach (string file in files)
                        {
                            SessionLog<T> session = SessionsCache<T>.GetSessionLog(file);
                            if (session != null)
                            {
                                list.Add(session);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            SessionLog<T>[] ret = list.ToArray();
            list.Clear();
            return ret;
        }
    }
}
