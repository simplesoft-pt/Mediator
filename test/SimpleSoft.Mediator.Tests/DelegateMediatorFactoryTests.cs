using System;
using System.Linq;
using Xunit;

namespace SimpleSoft.Mediator.Tests
{
    public class DelegateMediatorFactoryTests
    {
        [Fact]
        public void GivenADelegateFactoryWhenPassingNullArgumentsThenAnArgumentNullExceptionMustBeThrown()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var factory = new DelegateMediatorServiceProvider(null, type => null);
                Assert.Null(factory);
            });
            Assert.NotNull(ex);

            ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var factory = new DelegateMediatorServiceProvider(type => null, null);
                Assert.Null(factory);
            });
            Assert.NotNull(ex);
        }

        [Fact]
        public void GivenADelegateFactoryWhenBuildingHandlersThenDelegatesMustBeInvoked()
        {
            var serviceInvoked = false;
            var serviceCollectionInvoked = false;

            var factory = new DelegateMediatorServiceProvider(
                type =>
                {
                    serviceInvoked = true;
                    return null;
                }, type =>
                {
                    serviceCollectionInvoked = true;
                    return Enumerable.Empty<object>();
                });

            factory.BuildService<ICommandHandler<Command>>();
            Assert.True(serviceInvoked);

            factory.BuildServices<IEventHandler<Event>>();
            Assert.True(serviceCollectionInvoked);
        }
    }
}
