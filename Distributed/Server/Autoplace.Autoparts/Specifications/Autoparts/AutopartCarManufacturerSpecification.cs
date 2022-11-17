using Autoplace.Autoparts.Data.Models;
using Autoplace.Common.Models;
using System.Linq.Expressions;

namespace Autoplace.Autoparts.Specifications.Autoparts
{
    public class AutopartCarManufacturerSpecification : Specification<Autopart>
    {
        private readonly int? carManufacturerId;

        public AutopartCarManufacturerSpecification(int? carManufacturerId)
        {
            this.carManufacturerId = carManufacturerId;
        }

        protected override bool Include => carManufacturerId != null;

        public override Expression<Func<Autopart, bool>> ToExpression()
            => autopart => autopart.Car.Model.ManufacturerId == carManufacturerId;
    }
}
