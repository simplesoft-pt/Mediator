using System;
using System.Linq;
using Xunit;

namespace SimpleSoft.Mediator.Tests
{
    public class DelegateHandlerFactoryTests
    {
        [Fact]
        public void GivenADelegateHandlerFactoryWhenPassingNullArgumentsThenAnArgumentNullExceptionMustBeThrown()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var factory = new DelegateHandlerFactory(null, type => null);
                Assert.Null(factory);
            });
            Assert.NotNull(ex);

            ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var factory = new DelegateHandlerFactory(type => null, null);
                Assert.Null(factory);
            });
            Assert.NotNull(ex);
        }

        [Fact]
        public void GivenADelegateHandlerFactoryWhenBuildingHandlersThenDelegatesMustBeInvoked()
        {
            var serviceInvoked = false;
            var serviceCollectionInvoked = false;

            var factory = new DelegateHandlerFactory(
                type =>
                {
                    serviceInvoked = true;
                    return null;
                }, type =>
                {
                    serviceCollectionInvoked = true;
                    return Enumerable.Empty<object>();
                });

            factory.BuildCommandHandlerFor<Command>();
            Assert.True(serviceInvoked);

            factory.BuildEventHandlersFor<Event>();
            Assert.True(serviceCollectionInvoked);
        }
    }
}
