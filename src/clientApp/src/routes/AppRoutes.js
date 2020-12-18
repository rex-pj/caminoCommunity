import {
  DefaultLayout,
  DetailLayout,
  FrameLayout,
  AuthLayout,
  ProfileLayout,
  PromptLayout,
} from "../components/templates/Layout";

export default [
  {
    path: ["/articles", "/articles/page/:pageNumber"],
    exact: true,
    page: "articles",
    layout: DefaultLayout,
  },
  {
    path: "/articles/:id",
    exact: true,
    page: "articles/detail",
    layout: DetailLayout,
  },
  {
    path: "/articles/update/:id",
    exact: true,
    page: "articles/update",
    layout: DetailLayout,
  },
  {
    path: ["/products", "/products/page/:pageNumber"],
    exact: true,
    page: "products",
    layout: DefaultLayout,
  },
  {
    path: "/products/:id",
    exact: true,
    page: "products/detail",
    layout: DetailLayout,
  },
  {
    path: "/products/update/:id",
    exact: true,
    page: "products/update",
    layout: DetailLayout,
  },
  {
    path: ["/farms", "/farms/page/:pageNumber"],
    exact: true,
    page: "farms",
    layout: DefaultLayout,
  },
  {
    path: "/farms/:id",
    exact: true,
    page: "farms/detail",
    layout: DetailLayout,
  },
  {
    path: "/farms/update/:id",
    exact: true,
    page: "farms/update",
    layout: DetailLayout,
  },
  {
    path: ["/associations", "/associations/page/:pageNumber"],
    exact: true,
    page: "associations",
    layout: DefaultLayout,
  },
  {
    path: "/associations/:id",
    exact: true,
    page: "associations/detail",
    layout: FrameLayout,
  },
  {
    path: "/auth/forgot-password",
    exact: true,
    page: "auth/forgot-password",
    layout: AuthLayout,
  },
  {
    path: "/auth/signin",
    exact: true,
    page: "auth/signin",
    layout: AuthLayout,
  },
  {
    path: "/auth/signup",
    exact: true,
    page: "auth/signup",
    layout: AuthLayout,
  },
  {
    path: "/auth/signout",
    exact: true,
    page: "auth/signout",
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
    path: ["/", "/page/:pageNumber", "/feeds", "/feeds/page/:pageNumber"],
    exact: true,
    page: "feeds",
    layout: DefaultLayout,
  },
  {
    path: [
      "/user/active/:email/:key",
      "/user/active/:email/:key+",
      "/user/active/:email/*",
    ],
    exact: true,
    page: "user/active",
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
