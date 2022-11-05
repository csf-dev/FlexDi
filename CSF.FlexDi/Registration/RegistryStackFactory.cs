//
//    RegistryStackFactory.cs
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
using System.Collections.Generic;

namespace CSF.FlexDi.Registration
{
    /// <summary>
    /// A factory service which creates instances of <see cref="StackOfRegistriesRegistrationProvider"/>.
    /// </summary>
    public class RegistryStackFactory
    {
        /// <summary>
        /// Creates a registration provider for a given set of resolution information.
        /// </summary>
        /// <returns>The registration provider.</returns>
        /// <param name="resolutionInfo">Resolution info.</param>
        public static IServiceRegistrationProvider CreateRegistryStack(IProvidesResolutionInfo resolutionInfo)
        {
            if(resolutionInfo == null)
                throw new ArgumentNullException(nameof(resolutionInfo));
      
            var providers = new List<IServiceRegistrationProvider>();
            var currentResolutionInfo = resolutionInfo;

            while(currentResolutionInfo != null)
            {
                providers.Add(currentResolutionInfo.Registry);
                currentResolutionInfo = currentResolutionInfo.Parent;
            }

            return new StackOfRegistriesRegistrationProvider(providers.ToArray());
        }
    }
}
