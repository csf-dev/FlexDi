//
//    CannotInstantiateTypeWithoutAnyConstructorsException.cs
//
//    Copyright 2018  Craig Fowler et al
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
//    For further copyright info, including a complete author/contributor
//    list, please refer to the file NOTICE.txt

using System;
namespace CSF.FlexDi.Resolution
{
  /// <summary>
  /// An exception which is raised when attempting to instantiate an instance of a type, but where no accessible
  /// constructors can be found upon that type.
  /// </summary>
#if !NETSTANDARD1_1
    [System.Serializable]
#endif
  public class CannotInstantiateTypeWithoutAnyConstructorsException : ResolutionException
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CannotInstantiateTypeWithoutAnyConstructorsException"/> class
    /// </summary>
    public CannotInstantiateTypeWithoutAnyConstructorsException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CannotInstantiateTypeWithoutAnyConstructorsException"/> class
    /// </summary>
    /// <param name="message">A <see cref="System.String"/> that describes the exception. </param>
    public CannotInstantiateTypeWithoutAnyConstructorsException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CannotInstantiateTypeWithoutAnyConstructorsException"/> class
    /// </summary>
    /// <param name="message">A <see cref="System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public CannotInstantiateTypeWithoutAnyConstructorsException(string message, Exception inner) : base(message, inner)
    {
    }

#if !NETSTANDARD1_1
    /// <summary>
    /// Initializes a new instance of the <see cref="CannotInstantiateTypeWithoutAnyConstructorsException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected CannotInstantiateTypeWithoutAnyConstructorsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
#endif
  }
}
