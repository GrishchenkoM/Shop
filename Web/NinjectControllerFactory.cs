using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic.Repositories.Implementations;
using BusinessLogic.Repositories.Interfaces;
using Domain;
using Ninject;

namespace Web
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        public NinjectControllerFactory()
        {
            _ninjectKernel = new StandardKernel();
            AddBinding();
        }

        // извлекаем экземпляр контроллера для заданного контекста запроса и типа контроллера   
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController) _ninjectKernel.Get(controllerType);
        }

        // опрделеим все привязки   
        private void AddBinding()
        {
            _ninjectKernel.Bind<ICustomerRepository>().To<CustomerRepository>();
            _ninjectKernel.Bind<IOrderRepository>().To<OrderRepository>();
            _ninjectKernel.Bind<IProductRepository>().To<ProductRepository>();
            _ninjectKernel.Bind<DbDataContext>()
                          .ToSelf()
                          .WithConstructorArgument("connectionString",
                                                   ConfigurationManager.ConnectionStrings[0].ConnectionString);
            _ninjectKernel.Inject(Membership.Provider);
        }
        
        private IKernel _ninjectKernel;
    }
}