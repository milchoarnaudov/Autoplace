using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Models;
using System.Linq.Expressions;

namespace Autoplace.Autoparts.Specifications.Autoparts
{
    public class AutopartCategorySpecification : Specification<Autopart>
    {
        private readonly int? categoryId;

        public AutopartCategorySpecification(int? categoryId)
        {
            this.categoryId = categoryId;
        }

        protected override bool Include => categoryId != null;

        public override Expression<Func<Autopart, bool>> ToExpression()
            => autopart => autopart.CategoryId == categoryId;
    }
}
