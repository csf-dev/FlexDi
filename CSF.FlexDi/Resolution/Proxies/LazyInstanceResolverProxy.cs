//
//    LazyInstanceResolverProxy.cs
//
//    Copyright 2018  Craig Fowler
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
namespace CSF.FlexDi.Resolution.Proxies
{
    /// <summary>
    /// A proxying resolver which resolves lazy object instances.
    /// </summary>
    public class LazyInstanceResolverProxy : ProxyingResolver
    {
        /// <summary>
        /// Resolves the given resolution request and returns the result.
        /// </summary>
        /// <param name="request">Request.</param>
        public override ResolutionResult Resolve(ResolutionRequest request)
        {
            if(!LazyFactory.IsLazyType(request.ServiceType))
                return ProxiedResolver.Resolve(request);

            var lazyInnerType = LazyFactory.GetInnerLazyType(request.ServiceType);
            var lazyRequest = GetLazyResolutionRequest(request, lazyInnerType);

            var lazyObject = LazyFactory.GetLazyObject(lazyInnerType, GetObjectFactory(lazyRequest));
            return ResolutionResult.Success(request.ResolutionPath, lazyObject);
        }

        static ResolutionRequest GetLazyResolutionRequest(ResolutionRequest sourceRequest, Type lazyInnerType)
            => new ResolutionRequest(lazyInnerType, sourceRequest.Name, sourceRequest.ResolutionPath);

        Func<object> GetObjectFactory(ResolutionRequest lazyRequest)
        {
            return () => {
                var result = ProxiedResolver.Resolve(lazyRequest);
                if(!result.IsSuccess)
                {
                    var message = String.Format(Resources.ExceptionFormats.LazyResolutionFailure,
                                                                            lazyRequest.ServiceType.FullName);
                    throw new ResolutionException(message) { ResolutionPath = lazyRequest.ResolutionPath };
                }
                return result.ResolvedObject;
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CSF.FlexDi.Resolution.Proxies.LazyInstanceResolverProxy"/> class.
        /// </summary>
        /// <param name="proxiedResolver">Proxied resolver.</param>
        public LazyInstanceResolverProxy(IResolver proxiedResolver) : base(proxiedResolver) {}
    }
}
