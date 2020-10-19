using System.Collections.Generic;

namespace SimpleSoft.Mediator.Example.Api.Controllers
{
    public class PageModel<T>
    {
        public int Total { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}