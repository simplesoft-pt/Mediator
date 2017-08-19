using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SimpleSoft.Mediator.Tests.Mocks;
using Xunit;

namespace SimpleSoft.Mediator.Tests.TMediator
{
    public class MediatorTests
    {
        [Fact]
        public void GivenAMediatorWhenPassingNullArgumentsThenAnArgumentNullExceptionMustBeThrown()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var m = new Mediator(null,
                    new DelegateMediatorFactory(type => null, type => Enumerable.Empty<object>()));
                Assert.Null(m);
            });
            Assert.NotNull(ex);

            ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var m = new Mediator(LoggingManager.CreateTestLogger<Mediator>(), null);
                Assert.Null(m);
            });
            Assert.NotNull(ex);
        }

        [Fact]
        public async Task GivenAMediatorWhenPublishingNullCommandThenAnArgumentNullExceptionMustBeThrown()
        {
            var mediator = new Mediator(
                LoggingManager.CreateTestLogger<Mediator>(),
                new DelegateMediatorFactory(type => null, type => Enumerable.Empty<object>()));

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await mediator.PublishAsync<MockCommand>(null, CancellationToken.None);
            });
            Assert.NotNull(ex);
        }

        [Fact]
        public async Task GivenAMediatorWhenPublishingNullResultCommandThenAnArgumentNullExceptionMustBeThrown()
        {
            var mediator = new Mediator(
                LoggingManager.CreateTestLogger<Mediator>(),
                new DelegateMediatorFactory(type => null, type => Enumerable.Empty<object>()));

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await mediator.PublishAsync<MockResultCommand, object>(null, CancellationToken.None);
            });
            Assert.NotNull(ex);
        }

        [Fact]
        public async Task GivenAMediatorWhenPublishingNullEventThenAnArgumentNullExceptionMustBeThrown()
        {
            var mediator = new Mediator(
                LoggingManager.CreateTestLogger<Mediator>(),
                new DelegateMediatorFactory(type => null, type => Enumerable.Empty<object>()));

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await mediator.BroadcastAsync<MockEvent>(null, CancellationToken.None);
            });
            Assert.NotNull(ex);
        }

        [Fact]
        public async Task GivenAMediatorWhenPassingACommandWithAnHandlerThenPublishMustSucceed()
        {
            var handlerFound = false;

            var mediator = new Mediator(
                LoggingManager.CreateTestLogger<Mediator>(),
                new DelegateMediatorFactory(
                    type =>
                    {
                        handlerFound = type == typeof(ICommandHandler<MockCommand>);
                        return handlerFound ? new MockCommandHandler() : null;
                    },
                    type => Enumerable.Empty<object>()));

            await mediator.PublishAsync(new MockCommand(), CancellationToken.None);
            Assert.True(handlerFound);
        }

        [Fact]
        public async Task GivenAMediatorWhenPassingACommandResultWithAnHandlerThenPublishMustSucceed()
        {
            var handlerFound = false;

            var mediator = new Mediator(
                LoggingManager.CreateTestLogger<Mediator>(),
                new DelegateMediatorFactory(
                    type =>
                    {
                        handlerFound = type == typeof(ICommandHandler<MockResultCommand, object>);
                        return handlerFound ? new MockResultCommandHandler() : null;
                    },
                    type => Enumerable.Empty<object>()));

            await mediator.PublishAsync<MockResultCommand, object>(new MockResultCommand(), CancellationToken.None);
            Assert.True(handlerFound);
        }

        [Fact]
        public async Task GivenAMediatorWhenPassingACommandWithoutAnHandlerThenCommandHandlerNotFoundExceptionMustBeThrown()
        {
            var mediator = new Mediator(
                LoggingManager.CreateTestLogger<Mediator>(),
                new DelegateMediatorFactory(
                    type => null,
                    type => Enumerable.Empty<object>()));

            var cmd = new MockCommand();
            var ex = await Assert.ThrowsAsync<CommandHandlerNotFoundException>(async () =>
            {
                await mediator.PublishAsync(cmd, CancellationToken.None);
            });
            Assert.NotNull(ex);
            Assert.Same(cmd, ex.CommandData);
        }
    }
}
