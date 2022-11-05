//
//    DynamicRecursionResolverProxy.cs
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

namespace BoDi.Internal
{
    /// <summary>
    /// A proxying resolver which resolves instances of <see cref="IObjectContainer"/> which have been specified as
    /// dependencies in factories or object constructors.  The resolved instance is marked with the resolution path
    /// up to the current point.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is important to address circular dependency detection when they make use of a service resolver to dynamically
    /// resolve further dependencies.
    /// </para>
    /// <para>
    /// Imagine service type A which depends upon an <see cref="IObjectContainer"/> in its constructor.  Then, also in
    /// its constructor it makes use of that resolver to resolve service B.  Service B declares an instance of service A
    /// in its constructor.
    /// </para>
    /// <para>
    /// In the example above, we have a circular dependency, but if the service resolver which is resolved to fulfil
    /// the constructor of service A were not 'aware' of its resolution path (IE: "service A") then it would be impossible
    /// to detect the circular dependency and it would lead to a stack overflow exception.
    /// </para>
    /// <para>
    /// This functionality corresponds to this issue in the original BoDi.  This resolver proxy fixes that issue for the
    /// FlexDi BoDi resolver.
    /// https://github.com/gasparnagy/BoDi/issues/13
    /// </para>
    /// </remarks>
    public class DynamicRecursionResolverProxy : ProxyingResolver
    {
        /// <summary>
        /// Resolves the given resolution request and returns the result.
        /// </summary>
        /// <param name="request">Request.</param>
        public override ResolutionResult Resolve(ResolutionRequest request)
        {
            var result = ProxiedResolver.Resolve(request);

            if(request.ServiceType != typeof(IObjectContainer) || !result.IsSuccess || request.ResolutionPath.IsEmpty)
                return result;

            var container = (IObjectContainer) result.ResolvedObject;
            if(container is DynamicResolutionObjectContainerProxy)
                return result;

            var dynamicContainer = new DynamicResolutionObjectContainerProxy((ObjectContainer) container, request.ResolutionPath);

            return ResolutionResult.Success(request.ResolutionPath, dynamicContainer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BoDi.Internal.DynamicRecursionResolverProxy"/> class.
        /// </summary>
        /// <param name="proxiedResolver">Proxied resolver.</param>
        public DynamicRecursionResolverProxy(IResolver proxiedResolver) : base(proxiedResolver) {}
    }
}
