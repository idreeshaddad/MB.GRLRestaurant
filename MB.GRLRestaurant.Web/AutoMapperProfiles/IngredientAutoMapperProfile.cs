using AutoMapper;
using MB.GRLRestaurant.Entities;
using MB.GRLRestaurant.Web.Models.Ingredients;

namespace MB.GRLRestaurant.Web.AutoMapperProfiles
{
    public class IngredientAutoMapperProfile : Profile
    {
        public IngredientAutoMapperProfile()
        {
            CreateMap<Ingredient, IngredientViewModel>().ReverseMap();
        }
    }
}
