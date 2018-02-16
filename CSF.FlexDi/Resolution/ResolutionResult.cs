//
//    ResolutionResult.cs
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
  /// Represents the result of a <see cref="ResolutionRequest"/>.
  /// </summary>
  public class ResolutionResult
  {
    /// <summary>
    /// Gets a value indicating whether this <see cref="T:CSF.FlexDi.Resolution.ResolutionResult"/> is a success.
    /// </summary>
    /// <value><c>true</c> if is success; otherwise, <c>false</c>.</value>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets the resolution path.
    /// </summary>
    /// <value>The resolution path.</value>
    public ResolutionPath ResolutionPath { get; }

    /// <summary>
    /// Gets the resolved service/component instance.
    /// </summary>
    /// <value>The resolved object.</value>
    public object ResolvedObject { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.FlexDi.Resolution.ResolutionResult"/> class.
    /// </summary>
    /// <param name="success">If set to <c>true</c> success.</param>
    /// <param name="resolutionPath">Resolution path.</param>
    /// <param name="resolvedObject">Resolved object.</param>
    public ResolutionResult(bool success, ResolutionPath resolutionPath, object resolvedObject)
    {
      if(resolutionPath == null)
        throw new ArgumentNullException(nameof(resolutionPath));

      IsSuccess = success;
      ResolvedObject = resolvedObject;
    }

    /// <summary>
    /// Creates an instance of <see cref="ResolutionResult"/> representing failed resolution.
    /// </summary>
    /// <param name="resolutionPath">Resolution path.</param>
    public static ResolutionResult Failure(ResolutionPath resolutionPath)
      => new ResolutionResult(false, resolutionPath, null);

    /// <summary>
    /// Creates an instance of <see cref="ResolutionResult"/> representing successful resolution.
    /// </summary>
    /// <param name="resolutionPath">Resolution path.</param>
    /// <param name="resolvedObject">Resolved object.</param>
    public static ResolutionResult Success(ResolutionPath resolutionPath, object resolvedObject)
      => new ResolutionResult(true, resolutionPath, resolvedObject);
  }
}
