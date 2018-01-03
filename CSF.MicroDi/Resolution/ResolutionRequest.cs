//
//    ResolutionRequest.cs
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
namespace CSF.MicroDi.Resolution
{
  public class ResolutionRequest
  {
    public Type ServiceType { get; private set; }

    public string Name { get; private set; }

    public ResolutionPath ResolutionPath { get; private set; }

    public ResolutionRequest GetCopyWithoutName()
    {
      return new ResolutionRequest(ServiceType, null);
    }

    public override string ToString()
    {
      var namePart = (Name != null)? $"('{Name}')" : string.Empty;
      return $"[ResolutionRequest: {ServiceType.FullName}{namePart}]";
    }

    public ResolutionRequest(Type serviceType, ResolutionPath resolutionPath = null)
      : this(serviceType, null, resolutionPath) {}

    public ResolutionRequest(Type serviceType, string name, ResolutionPath resolutionPath = null)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));

      ServiceType = serviceType;
      Name = name;
      ResolutionPath = resolutionPath ?? new ResolutionPath();
    }
  }
}
