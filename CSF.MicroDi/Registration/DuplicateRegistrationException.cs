using System;
namespace CSF.MicroDi.Registration
{
  /// <summary>
  /// Exception raised when a duplicate dependency registration is added.
  /// </summary>
  [System.Serializable]
  public class DuplicateRegistrationException : System.Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:DuplicateRegistrationException"/> class
    /// </summary>
    public DuplicateRegistrationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:MyException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public DuplicateRegistrationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:MyException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public DuplicateRegistrationException(string message, System.Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:MyException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected DuplicateRegistrationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
  }
}
