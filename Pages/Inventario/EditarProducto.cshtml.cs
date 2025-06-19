using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GimManager.Data;
using GimManager.Models;
using System.Threading.Tasks;

namespace GimManager.Pages.Inventario
{
    public class EditarProductoModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditarProductoModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Producto Producto { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Producto = await _context.Productos.FindAsync(id);

            if (Producto == null)
            {
                return RedirectToPage("/Inventario/Inventario");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Producto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                ModelState.AddModelError("", "Error al guardar");
                return Page();
            }

            return RedirectToPage("/Inventario/Inventario");
        }
    }
}