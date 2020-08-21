//
//    IInspectsServiceCache.cs
//
//    Copyright 2020  Craig Fowler
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
    /// An object which may query the current state of a container, to determine whether
    /// matching service instances have already been resolved &amp; cached or not.
    /// </summary>
    public interface IInspectsServiceCache
    {
        /// <summary>
        /// Gets a value indicating whether a matching service instance is available in the resolver's cache
        /// or not.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When this returns <c>true</c>, it means that a concrete instance of the service has already been
        /// resolved at least once, or was initially registered as an instance.  If the service were to be
        /// resolved using <see cref="IResolvesServices"/> then it would be served from the cache without
        /// creating a new instance.
        /// </para>
        /// <para>
        /// If this method returns <c>false</c> then, if the service were to be resolved, it would result in
        /// the creation of a new object.
        /// </para>
        /// </remarks>
        /// <returns>
        /// <c>true</c>, if an object of the matching <paramref name="type"/> (and optionally registration <paramref name="name"/>) is
        /// available in the cache, <c>false</c> otherwise.</returns>
        /// <param name="type">The service type.</param>
        /// <param name="name">An optional registration name.</param>
        bool IsResolvedInstanceCached(Type type, string name = null);
    }
}
