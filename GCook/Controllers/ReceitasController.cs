
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GCook.Data;
using GCook.Models;

namespace GCook.Controllers
{
    public class ReceitasController : Controller
    {
        private readonly AppDbContext _context;        
        private readonly IWebHostEnvironment _host;

        public ReceitasController(AppDbContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        // GET: Receitas
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Receitas.Include(r => r.Categoria);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Receitas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receita = await _context.Receitas
                .Include(r => r.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receita == null)
            {
                return NotFound();
            }

            return View(receita);
        }

        // GET: Receitas/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome");
            return View();
        }

        // POST: Receitas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,TempoPreparo,Rendimento,Dificuldade,Foto,Preparo,CategoriaId")] Receita receita, IFormFile Foto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receita);
                await _context.SaveChangesAsync();

                if (Foto != null)
                {
                    string nomeArquivo = receita.Id + Path.GetExtension(Foto.FileName);
                    string caminho = Path.Combine(_host.WebRootPath, "img/receitas");
                    string arquivo = Path.Combine(caminho, nomeArquivo);
                    using (FileStream stream = new(arquivo, FileMode.Create))
                    {
                        Foto.CopyTo(stream);
                    }
                    receita.Foto = "img/receitas/" + nomeArquivo;
                    await _context.SaveChangesAsync();
                } 

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", receita.CategoriaId);
            return View(receita);
        }

        // GET: Receitas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receita = await _context.Receitas.FindAsync(id);
            if (receita == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", receita.CategoriaId);
            return View(receita);
        }

        // POST: Receitas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,TempoPreparo,Rendimento,Dificuldade,Foto,Preparo,CategoriaId")] Receita receita)
        {
            if (id != receita.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceitaExists(receita.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", receita.CategoriaId);
            return View(receita);
        }

        // GET: Receitas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receita = await _context.Receitas
                .Include(r => r.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receita == null)
            {
                return NotFound();
            }

            return View(receita);
        }

        // POST: Receitas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receita = await _context.Receitas.FindAsync(id);
            if (receita != null)
            {
                _context.Receitas.Remove(receita);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceitaExists(int id)
        {
            return _context.Receitas.Any(e => e.Id == id);
        }
    }
}
