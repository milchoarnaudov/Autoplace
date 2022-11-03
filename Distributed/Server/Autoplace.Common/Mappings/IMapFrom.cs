using AutoMapper;

namespace Autoplace.Common.Mappings
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile mapper) => mapper.CreateMap(typeof(T), GetType());
    }
}
