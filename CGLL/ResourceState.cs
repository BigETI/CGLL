using System;
using System.IO;

/// <summary>
/// Community gme launcher library namespace
/// </summary>
namespace CGLL
{
    /// <summary>
    /// Resource state class
    /// </summary>
    public class ResourceState
    {
        /// <summary>
        /// Path
        /// </summary>
        private string path;

        /// <summary>
        /// Path
        /// </summary>
        public string Path
        {
            get
            {
                if (path == null)
                {
                    path = "";
                }
                return path;
            }
        }

        /// <summary>
        /// Last write date and time
        /// </summary>
        public DateTime LastWriteDateTime { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">Path</param>
        public ResourceState(string path)
        {
            this.path = path;
            if (File.Exists(path))
            {
                try
                {
                    LastWriteDateTime = File.GetLastWriteTime(path);
                }
                catch (Exception e)
                {
                    LastWriteDateTime = DateTime.Now;
                    Console.Error.WriteLine(e);
                }
            }
            else
            {
                LastWriteDateTime = DateTime.Now;
            }
        }
    }
}
