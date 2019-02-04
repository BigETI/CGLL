using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Json;

/// <summary>
/// Community game launcher library namespace
/// </summary>
namespace CGLL
{
    /// <summary>
    /// Session log class
    /// </summary>
    /// <typeparam name="T">User data type</typeparam>
    public class SessionLog<T> : IDisposable
    {
        /// <summary>
        /// Session log data
        /// </summary>
        private SessionLogDataContract<T> sessionLogData;

        /// <summary>
        /// Path
        /// </summary>
        private string path;

        /// <summary>
        /// Screenshots
        /// </summary>
        private Dictionary<string, SessionLogResource> resources;

        /// <summary>
        /// Archive file stream
        /// </summary>
        private FileStream archiveFileStream;

        /// <summary>
        /// Archive
        /// </summary>
        private ZipArchive archive;

        /// <summary>
        /// Serializer
        /// </summary>
        private static readonly DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SessionLogDataContract<T>));

        /// <summary>
        /// Path
        /// </summary>
        public string Path
        {
            get
            {
                return path;
            }
        }

        /// <summary>
        /// Session data
        /// </summary>
        public SessionLogDataContract<T> SessionLogData
        {
            get
            {
                if (sessionLogData == null)
                {
                    sessionLogData = new SessionLogDataContract<T>();
                }
                return sessionLogData;
            }
        }

        /// <summary>
        /// Date and time
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                return SessionLogData.DateTime;
            }
        }

        /// <summary>
        /// Time span
        /// </summary>
        public TimeSpan TimeSpan
        {
            get
            {
                return SessionLogData.TimeSpan;
            }
        }

        /// <summary>
        /// User data
        /// </summary>
        public T UserData
        {
            get
            {
                return SessionLogData.UserData;
            }
        }

        /// <summary>
        /// Resources
        /// </summary>
        public SessionLogResource[] Resources
        {
            get
            {
                if (resources == null)
                {
                    resources = new Dictionary<string, SessionLogResource>();
                    try
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (entry.FullName.Trim() != "meta.json")
                            {
                                try
                                {
                                    if (!(resources.ContainsKey(entry.FullName)))
                                    {
                                        SessionLogResource session_log_resource = new SessionLogResource(entry.FullName, entry.Open(), true);
                                        resources.Add(session_log_resource.Name, session_log_resource);
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.Error.WriteLine(e);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e);
                    }
                }
                SessionLogResource[] ret = new SessionLogResource[resources.Count];
                resources.Values.CopyTo(ret, 0);
                return ret;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="sessionLogData">Session log data</param>
        /// <param name="archiveFileStream">Archive file stream</param>
        /// <param name="archive">Archive</param>
        private SessionLog(string path, SessionLogDataContract<T> sessionLogData, FileStream archiveFileStream, ZipArchive archive)
        {
            this.path = path;
            this.sessionLogData = sessionLogData;
            this.archiveFileStream = archiveFileStream;
            this.archive = archive;
        }

        /// <summary>
        /// Create session
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="dateTime">Date and time</param>
        /// <param name="timeSpan">Time span</param>
        /// <param name="gameVersion">Game version</param>
        /// <param name="username">Username</param>
        /// <param name="ipPort">IP and port</param>
        /// <param name="hostname">Hostname</param>
        /// <param name="mode">Mode</param>
        /// <param name="language">Language</param>
        /// <param name="screenshotPaths">Screenshot paths</param>
        /// <param name="chatlogPath">Chatlog path</param>
        /// <param name="savedPositionsPath">Saved positions path</param>
        /// <returns>New session</returns>
        public static SessionLog<T> Create(string path, DateTime dateTime, TimeSpan timeSpan, T userData, SessionLogResourcePathDataContract[] resourcePaths)
        {
            SessionLog<T> ret = null;
            if (path != null)
            {
                if (path != null)
                {
                    SessionLogDataContract<T> session_data = new SessionLogDataContract<T>(dateTime, timeSpan, userData);
                    try
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        FileStream file_stream = File.Open(path, FileMode.Create);
                        ZipArchive archive = new ZipArchive(file_stream, ZipArchiveMode.Create);
                        ZipArchiveEntry entry = archive.CreateEntry("meta.json");
                        if (entry != null)
                        {
                            using (Stream stream = entry.Open())
                            {
                                serializer.WriteObject(stream, session_data);
                            }
                        }
                        if (resourcePaths != null)
                        {
                            foreach (SessionLogResourcePathDataContract resource_path in resourcePaths)
                            {
                                if (resource_path != null)
                                {
                                    if (File.Exists(resource_path.Path))
                                    {
                                        try
                                        {
                                            using (FileStream resource_file_stream = File.Open(resource_path.Path, FileMode.Open))
                                            {
                                                // TODO
                                                // Add support for sub entries
                                                entry = archive.CreateEntry(resource_path.DataType.ToString().ToLower() + "/" + System.IO.Path.GetFileName(resource_path.Path), CompressionLevel.Optimal);
                                                if (entry != null)
                                                {
                                                    using (Stream entry_stream = entry.Open())
                                                    {
                                                        if (entry_stream != null)
                                                        {
                                                            resource_file_stream.CopyTo(entry_stream);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Console.Error.WriteLine(e);
                                        }
                                    }
                                }
                            }
                        }
                        ret = new SessionLog<T>(path, session_data, file_stream, archive);
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e);
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Load session
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>Session if successful, otherwise "null"</returns>
        public static SessionLog<T> Load(string path)
        {
            SessionLog<T> ret = null;
            try
            {
                if (path != null)
                {
                    if (File.Exists(path))
                    {
                        FileStream file_stream = File.Open(path, FileMode.Open, FileAccess.Read);
                        ZipArchive archive = new ZipArchive(file_stream, ZipArchiveMode.Read);
                        ZipArchiveEntry entry = archive.GetEntry("meta.json");
                        if (entry != null)
                        {
                            using (Stream stream = entry.Open())
                            {
                                SessionLogDataContract<T> session_data = serializer.ReadObject(stream) as SessionLogDataContract<T>;
                                if (session_data != null)
                                {
                                    ret = new SessionLog<T>(path, session_data, file_stream, archive);
                                }
                                else
                                {
                                    archive.Dispose();
                                    file_stream.Dispose();
                                }
                            }
                        }
                        else
                        {
                            archive.Dispose();
                            file_stream.Dispose();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            return ret;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (resources != null)
            {
                foreach (SessionLogResource resource in resources.Values)
                {
                    resource.Dispose();
                }
                resources.Clear();
            }
            if (archive != null)
            {
                archive.Dispose();
                archive = null;
            }
            if (archiveFileStream != null)
            {
                archiveFileStream.Dispose();
                archiveFileStream = null;
            }
        }
    }
}
