//
//    ExceptionTransformer.cs
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
using CSF.FlexDi;
using CSF.FlexDi.Registration;

namespace BoDi.Internal
{
    /// <summary>
    /// This class catches FlexDi exceptions and transforms them to the same exception type that BoDi would have raised
    /// under the same circumstances.
    /// </summary>
    static class ExceptionTransformer
    {
        static internal void TransformExceptions(Action action)
        {
            if(action == null)
                throw new ArgumentNullException(nameof(action));

            try
            {
                action();
            }
            catch(InvalidTypeRegistrationException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch(ContainerException ex)
            {
                throw new ObjectContainerException(ex.Message, GetResolutionPath(ex), ex);
            }
        }

        static internal T TransformExceptions<T>(Func<T> action)
        {
            if(action == null)
                throw new ArgumentNullException(nameof(action));

            try
            {
                return action();
            }
            catch(InvalidTypeRegistrationException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch(ContainerException ex)
            {
                throw new ObjectContainerException(ex.Message, GetResolutionPath(ex), ex);
            }
        }

        static Type[] GetResolutionPath(ContainerException ex)
        {
            if(ex.ResolutionPath != null)
                return ex.ResolutionPath.GetRegistrations().Select(x => x.ServiceType).ToArray();

            return new Type[0];
        }
    }
}
