using Moq;

namespace Crossroads.Service.Auth.Tests.Fakes
{
    public abstract class FakeFactory<T>
    {
        protected FakeFactory(MockRepository mockRepository) { }

        public abstract T Build();
    }
}