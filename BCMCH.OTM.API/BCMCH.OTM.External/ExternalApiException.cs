using System.Runtime.Serialization;

namespace BCMCH.OTM.External
{
    [Serializable]
    internal class ExternalApiException : Exception
    {
        public ExternalApiException()
        {
        }

        public ExternalApiException(string? message) : base(message)
        {
        }

        public ExternalApiException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ExternalApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}