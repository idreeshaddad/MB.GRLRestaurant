using AutoMapper;
using MB.GRLRestaurant.Entities;
using MB.GRLRestaurant.Web.Models.Meals;

namespace MB.GRLRestaurant.Web.AutoMapperProfiles
{
    public class MealsAutoMapperProfile : Profile
    {
        public MealsAutoMapperProfile()
        {
            CreateMap<Meal, MealListViewModel>();
            CreateMap<Meal, MealDetailsViewModel>();


            CreateMap<Meal, MealViewModel>()
                .ForMember(dest => dest.IngredientIds,
                                    opts => opts.MapFrom(src => src.Ingredients.Select(ing => ing.Id).ToList()));
            
            
            CreateMap<MealViewModel, Meal>();
        }
    }
}
