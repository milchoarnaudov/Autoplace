using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Models;
using System.Linq.Expressions;

namespace Autoplace.Autoparts.Specifications.Autoparts
{
    public class AutopartNameSpecification : Specification<Autopart>
    {
        private readonly string name;

        public AutopartNameSpecification(string name)
        {
            this.name = name;
        }

        protected override bool Include => name != null;

        public override Expression<Func<Autopart, bool>> ToExpression()
            => autopart => autopart.Name.ToLower()
            .Contains(name.ToLower());
    }
}
