﻿//
//    ContainerDisposedException.cs
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
namespace CSF.FlexDi
{
  /// <summary>
  /// Exception raised when an attempt is made to use an <see cref="IContainer"/> instance which has been disposed.
  /// </summary>
#if !NETSTANDARD1_1
    [System.Serializable]
#endif
    public class ContainerDisposedException : ContainerException
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ContainerDisposedException"/> class
    /// </summary>
    public ContainerDisposedException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContainerDisposedException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public ContainerDisposedException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContainerDisposedException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public ContainerDisposedException(string message, Exception inner) : base(message, inner)
    {
    }

#if !NETSTANDARD1_1
    /// <summary>
    /// Initializes a new instance of the <see cref="ContainerDisposedException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected ContainerDisposedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
#endif
  }
}
