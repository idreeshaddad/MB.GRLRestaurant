using AutoMapper;
using MB.GRLRestaurant.Entities;
using MB.GRLRestaurant.Web.Models.Orders;

namespace MB.GRLRestaurant.Web.AutoMapperProfiles
{
    public class OrdersAutoMapperProfile : Profile
    {
        public OrdersAutoMapperProfile()
        {
            CreateMap<Order, OrderListViewModel>();
            CreateMap<Order, OrderDetailsViewModel>();
            CreateMap<OrderViewModel, Order>().ReverseMap();
        }
    }
}
