using System.Collections.Generic;

namespace SimpleSoft.Mediator.Example.Api.Handlers
{
    public class Page<T>
    {
        public int Total { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}