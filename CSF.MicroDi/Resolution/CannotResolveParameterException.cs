using System;
namespace CSF.MicroDi.Resolution
{
  [System.Serializable]
  public class CannotResolveParameterException : ResolutionException
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:CannotResolveParameterException"/> class
    /// </summary>
    public CannotResolveParameterException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CannotResolveParameterException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public CannotResolveParameterException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CannotResolveParameterException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public CannotResolveParameterException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CannotResolveParameterException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected CannotResolveParameterException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
  }
}
