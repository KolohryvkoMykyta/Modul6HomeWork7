﻿using Order.Host.Data.Entities;
using Order.Host.Models.Dtos;

namespace Order.Host.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderEntity, OrderDto>();
            CreateMap<ProductEntity, OrderProductDto>();
            CreateMap<OrderDto, OrderEntity>();
            CreateMap<OrderProductDto, ProductEntity>();
        }
    }
}
