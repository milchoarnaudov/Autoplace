using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Models;
using System.Linq.Expressions;

namespace Autoplace.Autoparts.Specifications.Autoparts
{
    public class AutopartPriceSpecification : Specification<Autopart>
    {
        private readonly decimal? maxPrice;

        public AutopartPriceSpecification(decimal? maxPrice)
        {
            this.maxPrice = maxPrice;
        }

        protected override bool Include => maxPrice != null;

        public override Expression<Func<Autopart, bool>> ToExpression()
            => autopart => autopart.Price <= maxPrice;
    }
}
