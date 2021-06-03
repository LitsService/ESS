using ESS_Web_Application.Controllers;
using ESS_Web_Application.Entity;
using ESS_Web_Application.Models;
using ESS_Web_Application.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Data.Entity;
using System.Web;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace ESS_Web_Application
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
            container.RegisterType<IdentityUser, ApplicationUser>();
            container.RegisterType<DbContext, DBContext>();
            container.RegisterType<IAuthenticationManager>(
                    new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication)); container.RegisterType<SignInManager<ApplicationUser, string>, ApplicationSignInManager>();
            container.RegisterType<UserManager<ApplicationUser>, ApplicationUserManager>();
            //        container.RegisterType<DbContext, DBContext>(
            //new HierarchicalLifetimeManager());
            //        container.RegisterType<UserManager<ApplicationUser>>(
            //            new HierarchicalLifetimeManager());
            //        container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
            //            new HierarchicalLifetimeManager());

            //        container.RegisterType<IAccountController, AccountController>();
        }
    }
}