using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using D_real_social_app.Data;
using D_real_social_app.Models;

namespace D_real_social_app.Controllers
{
    public class ConnectionController : Controller
    {
        private readonly SocialAppContext _context;

        public ConnectionController(SocialAppContext context)
        {
            _context = context;
        }

        // GET: Connection
        public async Task<IActionResult> Index()
        {
              return _context.Connection != null ? 
                          View(await _context.Connection.ToListAsync()) :
                          Problem("Entity set 'SocialAppContext.Connection'  is null.");
        }

        // GET: Connection/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Connection == null)
            {
                return NotFound();
            }

            var connection = await _context.Connection
                .FirstOrDefaultAsync(m => m.ConnectionID == id);
            if (connection == null)
            {
                return NotFound();
            }

            return View(connection);
        }

        // GET: Connection/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Connection/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConnectionID,UserID,UserID2")] Connection connection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(connection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(connection);
        }

        // GET: Connection/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Connection == null)
            {
                return NotFound();
            }

            var connection = await _context.Connection.FindAsync(id);
            if (connection == null)
            {
                return NotFound();
            }
            return View(connection);
        }

        // POST: Connection/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConnectionID,UserID,UserID2")] Connection connection)
        {
            if (id != connection.ConnectionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(connection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConnectionExists(connection.ConnectionID))
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
            return View(connection);
        }

        // GET: Connection/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Connection == null)
            {
                return NotFound();
            }

            var connection = await _context.Connection
                .FirstOrDefaultAsync(m => m.ConnectionID == id);
            if (connection == null)
            {
                return NotFound();
            }

            return View(connection);
        }

        // POST: Connection/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Connection == null)
            {
                return Problem("Entity set 'SocialAppContext.Connection'  is null.");
            }
            var connection = await _context.Connection.FindAsync(id);
            if (connection != null)
            {
                _context.Connection.Remove(connection);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConnectionExists(int id)
        {
          return (_context.Connection?.Any(e => e.ConnectionID == id)).GetValueOrDefault();
        }
    }
}
