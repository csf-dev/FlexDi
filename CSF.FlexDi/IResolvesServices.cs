//
//    IResolvesServices.cs
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

using CSF.FlexDi.Resolution;

namespace CSF.FlexDi
{
    /// <summary>
    /// A type which can resolve service instances.
    /// </summary>
    public interface IResolvesServices
    {
        /// <summary>
        /// Attempts to resolve a component, as specified by a <see cref="ResolutionRequest"/> instance.
        /// The result indicates whether resolution was successful or not, and if it is, contains a reference to the resolved
        /// component.
        /// </summary>
        /// <returns>A resolution result instance.</returns>
        /// <param name="request">A resolution request specifying what is to be resolved.</param>
        ResolutionResult TryResolve(ResolutionRequest request);
    }
}
