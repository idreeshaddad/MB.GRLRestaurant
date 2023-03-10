using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MB.GRLRestaurant.Entities;
using MB.GRLRestaurant.Web.Data;
using AutoMapper;
using MB.GRLRestaurant.Web.Models.Ingredients;

namespace MB.GRLRestaurant.Web.Controllers
{
    public class IngredientsController : Controller
    {
        #region Data and Const

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public IngredientsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Actions

        public async Task<IActionResult> Index()
        {
            var ingredients = await _context.Ingredients.ToListAsync();

            var ingredientsVMs = _mapper.Map<List<IngredientViewModel>>(ingredients);

            return View(ingredientsVMs);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ingredients == null)
            {
                return NotFound();
            }

            var ingredient = await _context
                                    .Ingredients
                                    .SingleOrDefaultAsync(ingredient => ingredient.Id == id);

            if (ingredient == null)
            {
                return NotFound();
            }

            var ingredientVM = _mapper.Map<IngredientViewModel>(ingredient);

            return View(ingredientVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IngredientViewModel ingredientVM)
        {
            if (ModelState.IsValid)
            {
                var ingredient = _mapper.Map<Ingredient>(ingredientVM);

                _context.Add(ingredient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(ingredientVM);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ingredients == null)
            {
                return NotFound();
            }

            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient == null)
            {
                return NotFound();
            }

            var ingredientVM = _mapper.Map<IngredientViewModel>(ingredient);

            return View(ingredientVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IngredientViewModel ingredientVM)
        {
            if (id != ingredientVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var ingredient = _mapper.Map<Ingredient>(ingredientVM);

                    _context.Update(ingredient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientExists(ingredientVM.Id))
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

            return View(ingredientVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ingredients == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Ingredients'  is null.");
            }
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Privates

        private bool IngredientExists(int id)
        {
            return _context.Ingredients.Any(e => e.Id == id);
        } 

        #endregion
    }
}
