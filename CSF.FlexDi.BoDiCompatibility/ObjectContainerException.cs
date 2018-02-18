//
//    ObjectContainerException.cs
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
using System.Linq;

namespace BoDi
{
  /// <summary>
  /// Base exception type raised by the <see cref="IObjectContainer"/> type.
  /// </summary>
#if !BODI_LIMITEDRUNTIME
  [Serializable]
#endif
  public class ObjectContainerException : Exception
  {
    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="T:BoDi.ObjectContainerException"/> class.
    /// </summary>
    /// <param name="message">Message.</param>
    /// <param name="resolutionPath">Resolution path.</param>
    public ObjectContainerException(string message, Type[] resolutionPath)
      : base(GetMessage(message, resolutionPath)) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="T:BoDi.ObjectContainerException"/> class.
    /// </summary>
    /// <param name="message">Message.</param>
    /// <param name="resolutionPath">Resolution path.</param>
    /// <param name="inner">Inner.</param>
    public ObjectContainerException(string message, Type[] resolutionPath, Exception inner)
      : base(GetMessage(message, resolutionPath), inner) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="T:BoDi.ObjectContainerException"/> class.
    /// </summary>
    /// <param name="info">Info.</param>
    /// <param name="context">Context.</param>
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
