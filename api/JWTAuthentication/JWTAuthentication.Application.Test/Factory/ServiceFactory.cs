using JWTAuthentication.Application.Test.Contexts;
using Moq;

namespace JWTAuthentication.Application.Test.Factory
{
    public abstract class ServiceFactory<TService> : IDisposable
    {
        public Mock<AuthenticationOrganizationContextTest> _dbContext { get; protected set; }

        public ServiceFactory(Mock<AuthenticationOrganizationContextTest> dbContext)
        {
            _dbContext = dbContext;
        }
        public abstract TService GetServiceInstace(Dictionary<string, IList<object>> datas);
        public abstract TService GetServiceInstace();
        protected abstract TService BuildInstace();

        public void Dispose()
        {
            _dbContext = null;
        }
    }
}
