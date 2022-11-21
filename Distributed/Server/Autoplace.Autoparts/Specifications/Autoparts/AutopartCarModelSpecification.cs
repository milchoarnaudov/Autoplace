using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Models;
using System.Linq.Expressions;

namespace Autoplace.Autoparts.Specifications.Autoparts
{
    public class AutopartCarModelSpecification : Specification<Autopart>
    {
        private readonly int? carModelId;

        public AutopartCarModelSpecification(int? carModelId)
        {
            this.carModelId = carModelId;
        }

        protected override bool Include => carModelId != null;

        public override Expression<Func<Autopart, bool>> ToExpression()
            => autopart => autopart.Car.ModelId == carModelId;
    }
}
