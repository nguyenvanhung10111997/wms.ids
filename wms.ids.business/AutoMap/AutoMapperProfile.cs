using AutoMapper;

namespace wms.business.AutoMap
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DateTimeOffset?, DateTime?>().ConvertUsing<DateTimeOffsetToDateTimeNullAble>();
            CreateMap<DateTimeOffset, DateTime>().ConvertUsing<DateTimeOffsetToDateTime>();
        }

        public class DateTimeOffsetToDateTimeNullAble : ITypeConverter<DateTimeOffset?, DateTime?>
        {
            public DateTime? Convert(DateTimeOffset? source, DateTime? destination, ResolutionContext context)
            {
                if (source == null)
                {
                    return null;
                }
                else
                {
                    return source.Value.DateTime;
                }
            }
        }

        public class DateTimeOffsetToDateTime : ITypeConverter<DateTimeOffset, DateTime>
        {
            public DateTime Convert(DateTimeOffset source, DateTime destination, ResolutionContext context)
            {
                return source.DateTime;
            }
        }
    }
}
