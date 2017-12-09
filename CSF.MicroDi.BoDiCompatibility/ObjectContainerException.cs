using System;
using System.Linq;

namespace BoDi
{
#if !BODI_LIMITEDRUNTIME
  [Serializable]
#endif
  public class ObjectContainerException : Exception
  {
    #region constructors

    public ObjectContainerException(string message, Type[] resolutionPath) : base(GetMessage(message, resolutionPath)) {}

#if !BODI_LIMITEDRUNTIME
    protected ObjectContainerException(System.Runtime.Serialization.SerializationInfo info,
                                       System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
#endif

    #endregion

    #region static methods

    static string GetMessage(string message, Type[] resolutionPath)
    {
      if (resolutionPath == null || resolutionPath.Length == 0)
        return message;

      return string.Format("{0} (resolution path: {1})", message, string.Join("->", resolutionPath.Select(t => t.FullName).ToArray()));
    }

    #endregion
  }
}
