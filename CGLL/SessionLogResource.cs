using System;
using System.IO;

/// <summary>
/// Community game launcher library namespace
/// </summary>
namespace CGLL
{
    /// <summary>
    /// Session log resource
    /// </summary>
    public class SessionLogResource : IDisposable
    {
        /// <summary>
        /// Name
        /// </summary>
        private string name;

        /// <summary>
        /// Dispose stream on dispose
        /// </summary>
        private bool disposeStreamOnDispose;

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get
            {
                if (name == null)
                {
                    name = "";
                }
                return name;
            }
        }

        /// <summary>
        /// Resource stream
        /// </summary>
        public Stream ResourceStream { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Session log name</param>
        /// <param name="resourceStream">Session log resource stream</param>
        /// <param name="disposeStreamOnDispose">Dispose stream on dispose</param>
        public SessionLogResource(string name, Stream resourceStream, bool disposeStreamOnDispose)
        {
            this.name = name;
            ResourceStream = resourceStream;
            this.disposeStreamOnDispose = disposeStreamOnDispose;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (disposeStreamOnDispose && (ResourceStream != null))
            {
                ResourceStream.Dispose();
                ResourceStream = null;
            }
        }
    }
}
