using System;
using System.Runtime.Serialization;

/// <summary>
/// Community game launcher library namespace
/// </summary>
namespace CGLL
{
    /// <summary>
    /// Session log data contract class
    /// </summary>
    /// <typeparam name="T">User data type</typeparam>
    [DataContract]
    public class SessionLogDataContract<T>
    {
        /// <summary>
        /// Date and time
        /// </summary>
        [DataMember]
        private DateTime dateTime = DateTime.Now;

        /// <summary>
        /// Time span
        /// </summary>
        [DataMember]
        private TimeSpan timeSpan = TimeSpan.Zero;

        /// <summary>
        /// Session log user data
        /// </summary>
        [DataMember]
        private T userData;

        /// <summary>
        /// Date and time
        /// </summary>
        public DateTime DateTime => dateTime;

        /// <summary>
        /// Time span
        /// </summary>
        public TimeSpan TimeSpan => timeSpan;

        /// <summary>
        /// Session log user data
        /// </summary>
        public T UserData => userData;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SessionLogDataContract()
        {
            // ...
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dateTime">Date and time</param>
        /// <param name="timeSpan">Time span</param>
        /// <param name="userData">User data</param>
        public SessionLogDataContract(DateTime dateTime, TimeSpan timeSpan, T userData)
        {
            this.dateTime = dateTime;
            this.timeSpan = timeSpan;
            this.userData = userData;
        }
    }
}
