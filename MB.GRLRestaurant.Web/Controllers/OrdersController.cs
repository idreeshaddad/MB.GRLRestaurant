using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MB.GRLRestaurant.Entities;
using MB.GRLRestaurant.Web.Data;
using AutoMapper;
using MB.GRLRestaurant.Web.Models.Orders;

namespace MB.GRLRestaurant.Web.Controllers
{
    public class OrdersController : Controller
    {
        #region Data and Constructors

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrdersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Actions

        public async Task<IActionResult> Index()
        {
            var orders = await _context
                                .Orders
                                .Include(o => o.Customer)
                                .ToListAsync();

            var orderVMs = _mapper.Map<List<OrderListViewModel>>(orders);

            return View(orderVMs);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context
                                .Orders
                                .Include(o => o.Customer)
                                .Include(o => o.Meals)
                                .Where(o => o.Id == id)
                                .SingleOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            var orderVM = _mapper.Map<OrderDetailsViewModel>(order);

            return View(orderVM);
        }

        public IActionResult Create()
        {
            var orderVM = new OrderViewModel();
            orderVM.CustomerSelectList = new SelectList(_context.Customers, "Id", "FullName");
            orderVM.MealsMultiSelectList = new MultiSelectList(_context.Meals, "Id", "Name");

            return View(orderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderViewModel orderVM)
        {
            if (ModelState.IsValid)
            {
                var order = _mapper.Map<Order>(orderVM);
                await AddMealsToOrder(orderVM, order);

                order.OrderDate = DateTime.Now;

                order.TotalPrice = GetOrderTotalPrice(order.Meals);

                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            orderVM.CustomerSelectList = new SelectList(_context.Customers, "Id", "FullName", orderVM.CustomerId);
            orderVM.MealsMultiSelectList = new MultiSelectList(_context.Meals, "Id", "Name", orderVM.MealIds);

            return View(orderVM);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context
                                .Orders
                                .Include(o => o.Meals)
                                .Where(o => o.Id == id)
                                .SingleOrDefaultAsync();
                                //.SingleOrDefaultAsync(o => o.Id == id);
            
            if (order == null)
            {
                return NotFound();
            }

            var orderVM = _mapper.Map<OrderViewModel>(order);

            orderVM.MealIds = order.Meals.Select(meal => meal.Id).ToList();

            orderVM.CustomerSelectList = new SelectList(_context.Customers, "Id", "FullName", orderVM.CustomerId);
            orderVM.MealsMultiSelectList = new MultiSelectList(_context.Meals, "Id", "Name", orderVM.MealIds);

            return View(orderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderViewModel orderVM)
        {
            if (id != orderVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var order = _mapper.Map<Order>(orderVM);
                    _context.Update(order);
                    // This will save ALL the order table properties EXCEPT many-to-many
                    await _context.SaveChangesAsync();

                    await UpdateOrderMealsAsync(orderVM.MealIds, order.Id);
                    
                    order.TotalPrice = GetOrderTotalPrice(order.Meals);
                    // This will save the many-to-many (Order has many meals)
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(orderVM.Id))
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

            orderVM.CustomerSelectList = new SelectList(_context.Customers, "Id", "FullName", orderVM.CustomerId);
            orderVM.MealsMultiSelectList = new MultiSelectList(_context.Meals, "Id", "Name", orderVM.MealIds);

            return View(orderVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.IsPaid = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Private Methods

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        private async Task AddMealsToOrder(OrderViewModel orderVM, Order order)
        {
            //  orderVM.MealIds = 5, 6
            var meals = await _context.Meals.Where(meal => orderVM.MealIds.Contains(meal.Id)).ToListAsync();
            // meals = Zinger, Tender
            order.Meals.AddRange(meals); // Add zinger and tender to order
        }

        private double GetOrderTotalPrice(List<Meal> meals)
        {
            return meals.Sum(meal => meal.Price);
        }

        private async Task UpdateOrderMealsAsync(List<int> mealIds, int orderId)
        {
            // TODO
            // Get the Order from DB (including meals)
            var order = await _context
                            .Orders
                            .Include(o => o.Meals)
                            .Where(o => o.Id == orderId)
                            .SingleAsync();

            // Clear the order.Meals
            order.Meals.Clear();

            // Get the meals from DB
            var meals = await _context
                                .Meals
                                .Where(meal => mealIds.Contains(meal.Id))
                                .ToListAsync();

            // Add meals to the order
            order.Meals.AddRange(meals);
        }

        #endregion
    }
}
