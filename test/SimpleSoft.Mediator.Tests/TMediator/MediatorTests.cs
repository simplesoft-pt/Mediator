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
                var m = new Mediator(null, new IPipeline[0]);
                Assert.Null(m);
            });
            Assert.NotNull(ex);

            ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var m = new Mediator(null, null);
                Assert.Null(m);
            });
            Assert.NotNull(ex);
        }

        [Fact]
        public async Task GivenAMediatorWhenSendingNullCommandThenAnArgumentNullExceptionMustBeThrown()
        {
            var mediator = new Mediator(
                new DelegateMediatorServiceProvider(type => null, type => Enumerable.Empty<object>()),
                new IPipeline[0]);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await mediator.SendAsync<MockCommand>(null, CancellationToken.None);
            });
            Assert.NotNull(ex);
        }

        [Fact]
        public async Task GivenAMediatorWhenSendingNullResultCommandThenAnArgumentNullExceptionMustBeThrown()
        {
            var mediator = new Mediator(
                new DelegateMediatorServiceProvider(type => null, type => Enumerable.Empty<object>()),
                new IPipeline[0]);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await mediator.SendAsync<object>(null, CancellationToken.None);
            });
            Assert.NotNull(ex);
        }

        [Fact]
        public async Task GivenAMediatorWhenSendingNullEventThenAnArgumentNullExceptionMustBeThrown()
        {
            var mediator = new Mediator(
                new DelegateMediatorServiceProvider(type => null, type => Enumerable.Empty<object>()),
                new IPipeline[0]);

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await mediator.BroadcastAsync(null, CancellationToken.None);
            });
            Assert.NotNull(ex);
        }

        [Fact]
        public async Task GivenAMediatorWhenPassingACommandWithAnHandlerThenSendMustSucceed()
        {
            var handlerFound = false;

            var mediator = new Mediator(
                new DelegateMediatorServiceProvider(
                    type =>
                    {
                        handlerFound = type == typeof(ICommandHandler<MockCommand>);
                        return handlerFound ? new MockCommandHandler() : null;
                    },
                    type => Enumerable.Empty<object>()),
                new IPipeline[0]);

            await mediator.SendAsync(new MockCommand(), CancellationToken.None);
            Assert.True(handlerFound);
        }

        [Fact]
        public async Task GivenAMediatorWhenPassingACommandResultWithAnHandlerThenSendMustSucceed()
        {
            var handlerFound = false;

            var mediator = new Mediator(
                new DelegateMediatorServiceProvider(
                    type =>
                    {
                        handlerFound = type == typeof(ICommandHandler<MockResultCommand, object>);
                        return handlerFound ? new MockResultCommandHandler() : null;
                    },
                    type => Enumerable.Empty<object>()),
                new IPipeline[0]);

            await mediator.SendAsync(new MockResultCommand(), CancellationToken.None);
            Assert.True(handlerFound);
        }

        [Fact]
        public async Task GivenAMediatorWhenPassingACommandWithoutAnHandlerThenCommandHandlerNotFoundExceptionMustBeThrown()
        {
            var mediator = new Mediator(
                new DelegateMediatorServiceProvider(
                    type => null,
                    type => Enumerable.Empty<object>()),
                new IPipeline[0]);

            var cmd = new MockCommand();
            var ex = await Assert.ThrowsAsync<CommandHandlerNotFoundException>(async () =>
            {
                await mediator.SendAsync(cmd, CancellationToken.None);
            });
            Assert.NotNull(ex);
            Assert.Same(cmd, ex.CommandData);
        }
    }
}
