using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MB.GRLRestaurant.Entities;
using MB.GRLRestaurant.Web.Data;
using AutoMapper;
using MB.GRLRestaurant.Web.Models.Meals;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MB.GRLRestaurant.Web.Controllers
{
    public class MealsController : Controller
    {
        #region Data and Const

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MealsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Actions

        public async Task<IActionResult> Index()
        {
            var meals = await _context.Meals.ToListAsync();

            var mealVMs = _mapper.Map<List<MealListViewModel>>(meals);

            return View(mealVMs);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Meals == null)
            {
                return NotFound();
            }

            var meal = await _context.Meals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meal == null)
            {
                return NotFound();
            }

            var mealVM = _mapper.Map<MealDetailsViewModel>(meal);

            return View(mealVM);
        }

        public IActionResult Create()
        {
            var mealVM = new MealViewModel();
            mealVM.MultiSelectListIngredients = new MultiSelectList(_context.Ingredients, "Id", "Name");
            
            return View(mealVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MealViewModel mealVM)
        {
            if (ModelState.IsValid)
            {
                var meal = _mapper.Map<Meal>(mealVM);

                await AddIngredientsToMeal(mealVM, meal);

                _context.Add(meal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            mealVM.MultiSelectListIngredients = new MultiSelectList(_context.Ingredients, "Id", "Name", mealVM.IngredientIds);
            return View(mealVM);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Meals == null)
            {
                return NotFound();
            }

            var meal = await _context
                                .Meals
                                .Include(meal => meal.Ingredients)
                                .Where(meal => meal.Id == id)
                                .SingleOrDefaultAsync();

            if (meal == null)
            {
                return NotFound();
            }

            var mealVM = _mapper.Map<MealViewModel>(meal);

            // Get ids from ingredients manually (We used the one in automapper profile)
            //mealVM.IngredientIds = meal.Ingredients.Select(ing => ing.Id).ToList();

            mealVM.MultiSelectListIngredients = new MultiSelectList(_context.Ingredients, "Id", "Name", mealVM.IngredientIds);

            return View(mealVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MealViewModel mealVM)
        {
            if (id != mealVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var meal = _mapper.Map<Meal>(mealVM);
                    
                    // Save the meal info (without many to many ... meal - ingredients)
                    _context.Update(meal);
                    await _context.SaveChangesAsync();
                    
                    // Update the many to many then save
                    await UpdateMealIngredients(meal.Id, mealVM.IngredientIds);

                    _context.Update(meal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MealExists(mealVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }


            return View(mealVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Meals == null)
            {
                return NotFound();
            }

            var meal = await _context.Meals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Meals == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Meals'  is null.");
            }
            var meal = await _context.Meals.FindAsync(id);
            if (meal != null)
            {
                _context.Meals.Remove(meal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Private

        private async Task AddIngredientsToMeal(MealViewModel mealVM, Meal meal)
        {
            // Get the ingredients from ingrednt ids
            var ingredients = await _context
                                    .Ingredients
                                    .Where(ing => mealVM.IngredientIds.Contains(ing.Id))
                                    .ToListAsync();

            // add the ingrednets to meal.Ingredients
            meal.Ingredients.AddRange(ingredients);
        }

        private async Task UpdateMealIngredients(int mealId, List<int> ingredientIds)
        {
            // Get the meal including the ingredients
            var meal = await _context
                            .Meals
                            .Include(meal => meal.Ingredients)
                            .Where(meal => meal.Id == mealId)
                            .SingleAsync();

            // Clear the ingredients
            meal.Ingredients.Clear();

            // Get the ingredients from ingrednt ids
            var ingredients = await _context
                                    .Ingredients
                                    .Where(ing => ingredientIds.Contains(ing.Id))
                                    .ToListAsync();

            // add the ingrednets to meal.Ingredients
            meal.Ingredients.AddRange(ingredients);
        }

        private bool MealExists(int id)
        {
            return _context.Meals.Any(e => e.Id == id);
        } 

        #endregion
    }
}
