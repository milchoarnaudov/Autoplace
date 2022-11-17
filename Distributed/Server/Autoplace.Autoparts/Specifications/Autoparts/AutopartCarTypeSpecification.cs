using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Models;
using System.Linq.Expressions;

namespace Autoplace.Autoparts.Specifications.Autoparts
{
    public class AutopartCarTypeSpecification : Specification<Autopart>
    {
        private readonly int? carTypeId;

        public AutopartCarTypeSpecification(int? carTypeId)
        {
            this.carTypeId = carTypeId;
        }

        protected override bool Include => carTypeId != null;

        public override Expression<Func<Autopart, bool>> ToExpression()
            => autopart => autopart.Car.CarTypeId == carTypeId;
    }
}
