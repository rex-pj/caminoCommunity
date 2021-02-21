using Microsoft.AspNetCore.Mvc;
using Module.Web.Navigation.Models;
using System.Collections.Generic;
using System.Linq;

namespace Module.Web.Navigation.ViewComponents
{
    public class MainNavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var mainNavigation = new MainNavigationModel()
            {
                Name = "Camino",
                SubName = "Community",
                Logo = "/img/logo.png",
                Url = "/",
                TabNavigations = new List<TabNavigationModel>() {
                    new TabNavigationModel()
                    {
                        Name = "Content",
                        Code = "content",
                        Navigations = new List<NavigationModel>()
                        {
                            new NavigationModel()
                            {
                                Icon = "fa fa-home",
                                Name = "Home",
                                Route = "Home",
                                Url = "/Home"
                            },
                            new NavigationModel()
                            {
                                Icon = "far fa-newspaper",
                                Name = "Article",
                                Route = "articleNav",
                                Url = "/Article",
                                SubNavigations = new List<NavigationModel>()
                                {
                                    new NavigationModel()
                                    {
                                        Name = "Articles",
                                        Route = "article/index",
                                        Url = "/Article"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Article Categories",
                                        Route = "articlecategory/index",
                                        Url = "/ArticleCategory"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Article Pictures",
                                        Route = "article/pictures",
                                        Url = "/Article/Pictures"
                                    }
                                }
                            },
                            new NavigationModel()
                            {
                                Icon = "fa fa-warehouse",
                                Name = "Farm",
                                Route = "farmNav",
                                Url = "/Farm",
                                SubNavigations = new List<NavigationModel>()
                                {
                                    new NavigationModel()
                                    {
                                        Name = "Farms",
                                        Route = "farm/index",
                                        Url = "/Farm"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Farm Types",
                                        Route = "farmtype/index",
                                        Url = "/FarmType"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Farm Pictures",
                                        Route = "farm/pictures",
                                        Url = "/Farm/Pictures"
                                    }
                                }
                            },
                            new NavigationModel()
                            {
                                Icon = "far fa-flag",
                                Name = "Community",
                                Route = "CommunityNav",
                                Url = "/Community",
                                SubNavigations = new List<NavigationModel>()
                                {
                                    new NavigationModel()
                                    {
                                        Name = "Farm Communities",
                                        Route = "community/index",
                                        Url = "/Community"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Farm Community Types",
                                        Route = "communitytype/index",
                                        Url = "/CommunityType"
                                    }
                                }
                            },
                            new NavigationModel()
                            {
                                Icon = "fa fa-apple-alt",
                                Name = "Product",
                                Route = "productNav",
                                Url = "/Product",
                                SubNavigations = new List<NavigationModel>()
                                {
                                    new NavigationModel()
                                    {
                                        Name = "Products",
                                        Route = "product/index",
                                        Url = "/Product"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Product Categories",
                                        Route = "productcategory/index",
                                        Url = "/ProductCategory"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Product Attributes",
                                        Route = "productattribute/index",
                                        Url = "/ProductAttribute"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Product Pictures",
                                        Route = "product/pictures",
                                        Url = "/Product/Pictures"
                                    }
                                }
                            }
                        }
                    },
                    new TabNavigationModel()
                    {
                        Name = "Security",
                        Code = "security",
                        Navigations = new List<NavigationModel>()
                        {
                            new NavigationModel()
                            {
                                Icon = "far fa-user",
                                Name = "User",
                                Route = "userNav",
                                Url = "/User",
                                SubNavigations = new List<NavigationModel>()
                                {
                                    new NavigationModel()
                                    {
                                        Name = "Users",
                                        Route = "user/index",
                                        Url = "/User"
                                    }
                                }
                            },
                            new NavigationModel()
                            {
                                Icon = "far fa-user-circle",
                                Name = "Authorization",
                                Route = "authorizationNav",
                                Url = "/AuthorizationPolicy",
                                SubNavigations = new List<NavigationModel>()
                                {
                                    new NavigationModel()
                                    {
                                        Name = "Authorization Policies",
                                        Route = "authorizationpolicy/index",
                                        SubRoutes = new List<string>()
                                        {
                                            "userauthorizationpolicy/index",
                                            "roleauthorizationpolicy/index"
                                        },
                                        Url = "/AuthorizationPolicy"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "Roles",
                                        Route = "role/index",
                                        Url = "/Role"
                                    }
                                }
                            },
                            new NavigationModel()
                            {
                                Icon = "fas fa-fingerprint",
                                Route = "identityNav",
                                Name = "Identity",
                                SubNavigations = new List<NavigationModel>()
                                {
                                    new NavigationModel()
                                    {
                                        Name = "Countries",
                                        Route = "country/index",
                                        Url = "/Country"
                                    },
                                    new NavigationModel()
                                    {
                                        Name = "UserStatus",
                                        Route = "userstatus",
                                        Url = "/UserStatus"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var controllerName = RouteData.Values["controller"].ToString().ToLower();
            var actionName = RouteData.Values["action"].ToString().ToLower();
            foreach (var tab in mainNavigation.TabNavigations)
            {
                foreach (var navigation in tab.Navigations)
                {
                    foreach (var subNavigation in navigation.SubNavigations)
                    {
                        subNavigation.IsActived = subNavigation.Route.ToLower().Equals($"{controllerName}/{actionName}");
                        if (!subNavigation.IsActived && !navigation.SubNavigations.Any(x => x.Route.Equals($"{controllerName}/{actionName}")))
                        {
                            subNavigation.IsActived = subNavigation.Url.ToLower().Equals($"/{controllerName}");
                        }

                        if (!subNavigation.IsActived && subNavigation.SubRoutes.Any())
                        {
                            subNavigation.IsActived = subNavigation.SubRoutes.Any(x => x.ToLower() == $"{controllerName}/{actionName}");
                        }
                    }

                    navigation.IsActived = controllerName == navigation.Route.ToLower() || navigation.SubNavigations.Any(x => x.IsActived);
                }

                tab.IsActived = tab.Navigations.Any(x => x.IsActived);
            }

            return View(mainNavigation);
        }
    }
}
