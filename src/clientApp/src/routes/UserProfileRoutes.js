export default [
  {
    path: ["/:userId/about"],
    page: "user-about",
  },
  {
    path: ["/:userId/update"],
    page: "user-update",
  },
  {
    path: ["/:userId/security"],
    page: "user-security",
  },
  {
    path: ["/:userId/posts", "/:userId/posts/page/:pageNumber"],
    page: "user-posts",
  },
  {
    path: ["/:userId/products", "/:userId/products/page/:pageNumber"],
    page: "user-products",
  },
  {
    path: ["/:userId/farms", "/:userId/farms/page/:pageNumber"],
    page: "user-farms",
  },
  {
    path: ["/:userId/followings", "/:userId/followings/page/:pageNumber"],
    page: "user-followings",
  },
  {
    path: ["/:userId", "/:userId/feeds", "/:userId/feeds/page/:pageNumber"],
    page: "user-feeds",
  },
];
