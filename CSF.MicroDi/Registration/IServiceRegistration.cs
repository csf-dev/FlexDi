using System;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public interface IServiceRegistration
  {
    Multiplicity Multiplicity { get; }

    Type ServiceType { get; }

    string Name { get; }

    bool DisposeWithContainer { get; }

    IFactoryAdapter GetFactoryAdapter(ResolutionRequest request);

    void AssertIsValid();

    bool MatchesKey(ServiceRegistrationKey key);
  }
}
