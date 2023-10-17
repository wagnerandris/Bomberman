namespace Persistence
{
    [Serializable]
    public class MapFileException : Exception
    {
        public MapFileException() { }
        public MapFileException(string message) : base(message) { }
        public MapFileException(string message, Exception inner) : base(message, inner) { }
        protected MapFileException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
