
using Microsoft.EntityFrameworkCore;

namespace Ats.Data.EntityFramework
{
    public class DbFactory : Disposable, IDbFactory
    {
        private SCDataContext _context;
        public static DbContextOptions<SCDataContext> options;

        public DbFactory(SCDataContext context)
        {
            this._context = context;
        }

        public DbFactory()
        {

        }
        
        public SCDataContext Init()
        {
            return _context ?? (_context = new SCDataContext(options, null));
        }


        protected override void DisposeCore()
        {
            if (_context != null)
                _context.Dispose();
        }
    }
}
