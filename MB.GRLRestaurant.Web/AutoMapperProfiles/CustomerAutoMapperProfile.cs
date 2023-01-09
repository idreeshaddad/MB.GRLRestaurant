using AutoMapper;
using MB.GRLRestaurant.Entities;
using MB.GRLRestaurant.Web.Models.Customers;

namespace MB.GRLRestaurant.Web.AutoMapperProfiles
{
    public class CustomerAutoMapperProfile : Profile
    {
        public CustomerAutoMapperProfile()
        {
            CreateMap<Customer, CustomerListViewModel>();
            CreateMap<Customer, CustomerDetailsViewModel>();
            CreateMap<CustomerViewModel, Customer>().ReverseMap();
        }
    }
}
