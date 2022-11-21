using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Models;
using System.Linq.Expressions;

namespace Autoplace.Autoparts.Specifications.Autoparts
{
    public class AutopartConditionSpecification : Specification<Autopart>
    {
        private readonly int? conditionId;

        public AutopartConditionSpecification(int? conditionId)
        {
            this.conditionId = conditionId;
        }

        protected override bool Include => conditionId != null;

        public override Expression<Func<Autopart, bool>> ToExpression()
            => autopart => autopart.ConditionId == conditionId;
    }
}
