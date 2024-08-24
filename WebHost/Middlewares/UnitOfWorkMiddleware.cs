using Domains.Interfaces.IUnitofWork;
namespace WebHost.Middlewares
{
    public class UnitOfWorkMiddleware
    {
        private readonly RequestDelegate _next;

        public UnitOfWorkMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUnitOfWork unitOfWork)
        {
            await unitOfWork.BeginTransactionAsync();
            try
            {
                await _next(context);
                await unitOfWork.SaveAsync(); 
                await unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
