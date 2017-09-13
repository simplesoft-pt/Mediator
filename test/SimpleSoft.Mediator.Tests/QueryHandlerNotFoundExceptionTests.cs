using System;
using SimpleSoft.Mediator.Tests.Mocks;
using Xunit;

namespace SimpleSoft.Mediator.Tests
{
    public class QueryHandlerNotFoundExceptionTests
    {
        [Fact]
        public void GivenANullQueryWhenBuildingACommandHandlerNotFoundExceptionThenArgumentNullExceptionMustBeThrown()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var e = QueryHandlerNotFoundException.Build<IQuery<object>, object>(null);
                Assert.Null(e);
            });

            Assert.NotNull(ex);
        }

        [Fact]
        public void GivenAQueryWhenBuildingAQueryHandlerNotFoundExceptionThenAllPropertiesMustBeCorrect()
        {
            var query = new MockQuery();
            var queryType = query.GetType();

            var ex = QueryHandlerNotFoundException.Build<MockQuery, object>(query);

            Assert.NotNull(ex.QueryData);
            Assert.Same(query, ex.QueryData);

            Assert.NotNull(ex.QueryType);
            Assert.Same(queryType, ex.QueryType);

            Assert.NotNull(ex.QueryName);
            Assert.Equal(queryType.Name, ex.QueryName);

            Assert.NotNull(ex.Message);
            Assert.Contains(ex.QueryName, ex.Message);

            Assert.NotNull(ex.Query<MockQuery, object>());
            Assert.Throws<InvalidCastException>(() =>
            {
                ex.Query<IQuery<int>, int>();
            });
        }
    }
}
