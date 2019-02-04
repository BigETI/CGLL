using System.Runtime.Serialization;

/// <summary>
/// Community game launcher library namespace
/// </summary>
namespace CGLL
{
    /// <summary>
    /// Session log resource path data contract
    /// </summary>
    [DataContract]
    public class SessionLogResourcePathDataContract
    {
        /// <summary>
        /// Session resource path
        /// </summary>
        [DataMember]
        private string path;

        /// <summary>
        /// Session resource data type
        /// </summary>
        [DataMember]
        private ESessionResourceDataType dataType;

        /// <summary>
        /// Session resource path
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
        /// Session resource data type
        /// </summary>
        public ESessionResourceDataType DataType => dataType;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">Session resource path</param>
        /// <param name="dataType">Session resource data type</param>
        public SessionLogResourcePathDataContract(string path, ESessionResourceDataType dataType)
        {
            this.path = path;
            this.dataType = dataType;
        }
    }
}
