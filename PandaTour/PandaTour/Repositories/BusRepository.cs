namespace PandaTour.Repositories
{
    using PandaTour.Data;

    public class BusRepository
    {
        private PandaTourContext _context;

        public BusRepository(PandaTourContext context)
        {
            _context = context;
        }
    }
}
