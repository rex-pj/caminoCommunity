import {
  FrameLayout,
  AuthLayout,
  ProfileLayout,
  PromptLayout,
} from "../components/templates/Layout";

export default [
  {
    path: ["/", "/page/:pageNumber", "/feeds", "/feeds/page/:pageNumber"],
    exact: true,
    page: "feeds",
    layout: FrameLayout,
  },
  {
    path: ["/articles", "/articles/page/:pageNumber"],
    exact: true,
    page: "articles",
    layout: FrameLayout,
  },
  {
    path: "/articles/:id",
    exact: true,
    page: "articles/detail",
    layout: FrameLayout,
  },
  {
    path: "/articles/update/:id",
    exact: true,
    page: "articles/update",
    layout: FrameLayout,
  },
  {
    path: ["/products", "/products/page/:pageNumber"],
    exact: true,
    page: "products",
    layout: FrameLayout,
  },
  {
    path: "/products/:id",
    exact: true,
    page: "products/detail",
    layout: FrameLayout,
  },
  {
    path: "/products/update/:id",
    exact: true,
    page: "products/update",
    layout: FrameLayout,
  },
  {
    path: "/shopping-cart",
    exact: true,
    page: "shopping-cart/index",
    layout: FrameLayout,
  },
  {
    path: ["/farms", "/farms/page/:pageNumber"],
    exact: true,
    page: "farms",
    layout: FrameLayout,
  },
  {
    path: "/farms/:id",
    exact: true,
    page: "farms/detail",
    layout: FrameLayout,
  },
  {
    path: "/farms/update/:id",
    exact: true,
    page: "farms/update",
    layout: FrameLayout,
  },
  {
    path: ["/communities", "/communities/page/:pageNumber"],
    exact: true,
    page: "communities",
    layout: FrameLayout,
  },
  {
    path: "/communities/:id",
    exact: true,
    page: "communities/detail",
    layout: FrameLayout,
  },
  {
    path: "/auth/forgot-password",
    exact: true,
    page: "auth/forgot-password",
    layout: AuthLayout,
  },
  {
    path: "/auth/login",
    exact: true,
    page: "auth/login",
    layout: AuthLayout,
  },
  {
    path: "/auth/signup",
    exact: true,
    page: "auth/signup",
    layout: AuthLayout,
  },
  {
    path: "/auth/logout",
    exact: true,
    page: "auth/logout",
    layout: PromptLayout,
  },
  {
    path: [
      "/profile/:userId",
      "/profile/:userId/:pageName",
      "/profile/:userId/:pageName/page/:pageNumber",
    ],
    exact: true,
    page: "user/profile",
    layout: ProfileLayout,
  },
  {
    path: ["/search", "/search/:keyword"],
    exact: true,
    page: "feeds/search",
    layout: FrameLayout,
  },
  {
    path: [
      "/user/active/:email/:key",
      "/user/active/:email/:key+",
      "/user/active/:email/*",
    ],
    exact: true,
    page: "user/user-active",
    layout: PromptLayout,
  },
  {
    path: [
      "/user/reset-password/:email/:key",
      "/user/reset-password/:email/:key+",
      "/user/reset-password/:email/*",
    ],
    exact: true,
    page: "user/reset-password",
    layout: PromptLayout,
  },
  {
    path: "/error",
    exact: true,
    page: "error/index",
    layout: PromptLayout,
  },
  {
    path: "*",
    exact: true,
    page: "error/not-found",
    layout: PromptLayout,
  },
];
