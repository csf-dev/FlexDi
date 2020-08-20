//
//    ConstructorWithMostParametersSelector.cs
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
using System.Linq;
using System.Reflection;

namespace CSF.FlexDi.Resolution
{
    /// <summary>
    /// Implementation of <see cref="ISelectsConstructor"/> which will choose the single constructor of a type which
    /// has the most parameters.  If multiple constructors are tied for the most parameters then an exception will
    /// be raised.
    /// </summary>
    public class ConstructorWithMostParametersSelector : ISelectsConstructor
    {
        readonly bool includeNonPublicConstructors;

        /// <summary>
        /// Selects and returns a constructor from the given type.
        /// </summary>
        /// <returns>The selected constructor.</returns>
        /// <param name="type">The type for which to select a constructor.</param>
        public ConstructorInfo SelectConstructor(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var allConstructors = GetAllConstructors(type);
            var constructorWithMostParameters = allConstructors
              .OrderByDescending(x => x.GetParameters().Count())
              .FirstOrDefault();

            AssertAConstructorIsFound(type, constructorWithMostParameters);
            AssertConstructorIsNotAmbiguous(type, constructorWithMostParameters, allConstructors);

            return constructorWithMostParameters;
        }

        IEnumerable<ConstructorInfo> GetAllConstructors(Type type)
        {
#if !NETSTANDARD1_1
            return type.GetTypeInfo().GetConstructors(GetBindingFlags());
#else
            return type.GetTypeInfo().DeclaredConstructors;
#endif
        }

#if !NETSTANDARD1_1
        BindingFlags GetBindingFlags()
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;

            if (includeNonPublicConstructors)
                flags = flags | BindingFlags.NonPublic;

            return flags;
        }
#endif

        void AssertAConstructorIsFound(Type type, ConstructorInfo ctor)
        {
            if (ctor == null)
            {
                var message = String.Format(Resources.ExceptionFormats.ImplementationTypeMustHaveAConstructor, type.FullName);
                throw new CannotInstantiateTypeWithoutAnyConstructorsException(message);
            }
        }

        void AssertConstructorIsNotAmbiguous(Type type,
                                             ConstructorInfo constructorWithMostParameters,
                                             IEnumerable<ConstructorInfo> allConstructors)
        {
            var paramCount = constructorWithMostParameters.GetParameters().Count();
            if (allConstructors.Count(x => x.GetParameters().Count() == paramCount) > 1)
            {
                var parametersText = (paramCount == 1) ? "1 parameter" : $"{paramCount} parameters";
                var message = String.Format(Resources.ExceptionFormats.AmbiguousConstructor, type.FullName, parametersText);
                throw new AmbiguousConstructorException(message);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSF.FlexDi.Resolution.ConstructorWithMostParametersSelector"/> class.
        /// </summary>
        public ConstructorWithMostParametersSelector() : this(false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSF.FlexDi.Resolution.ConstructorWithMostParametersSelector"/> class.
        /// </summary>
        /// <param name="includeNonPublicConstructors">If set to <c>true</c> then non-public constructors will be considered, otherwise they will not.</param>
        public ConstructorWithMostParametersSelector(bool includeNonPublicConstructors)
        {
            this.includeNonPublicConstructors = includeNonPublicConstructors;
        }
    }
}
