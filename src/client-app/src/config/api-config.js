const caminoGraphApi = `${process.env.REACT_APP_CAMINO_API}/graphql`;
const caminoRestApi = `${process.env.REACT_APP_CAMINO_API}/api`;

export const apiConfig = {
  camino_api: caminoRestApi,
  camino_graphql: caminoGraphApi,
  paths: {
    userPhotos: {
      get: {
        getAvatar: `${caminoRestApi}/user-photos/avatars`,
        getCover: `${caminoRestApi}/user-photos/covers`,
      },
      put: {
        updateAvatar: `${caminoRestApi}/user-photos/avatars`,
        updateCover: `${caminoRestApi}/user-photos/covers`,
      },
      delete: {
        deleteAvatar: `${caminoRestApi}/user-photos/avatars`,
        deleteCover: `${caminoRestApi}/user-photos/covers`,
      },
    },
    pictures: {
      get: {
        getPicture: `${caminoRestApi}/pictures`,
      },
      put: {
        putValidatePicture: `${caminoRestApi}/pictures/validations`,
      },
    },
    authentications: {
      post: {
        postLogin: `${caminoRestApi}/authentications/login`,
        postRefreshToken: `${caminoRestApi}/authentications/refresh-tokens`,
        postForgotPassword: `${caminoRestApi}/authentications/forgot-password`,
        postResetPassword: `${caminoRestApi}/authentications/reset-password`,
      },
      patch: {
        patchUpdatePassword: `${caminoRestApi}/authentications/update-passwords`,
      },
    },
    users: {
      post: {
        postRegister: `${caminoRestApi}/users/registration`,
      },
      patch: {
        patchUser: `${caminoRestApi}/users/partials`,
      },
      put: {
        putUserIdentifiers: `${caminoRestApi}/users/identifiers`,
      },
    },
    articles: {
      post: {
        postArticle: `${caminoRestApi}/articles`,
      },
      put: {
        putArticle: `${caminoRestApi}/articles`,
      },
      delete: {
        deleteArticle: `${caminoRestApi}/articles`,
      },
    },
    farms: {
      post: {
        postFarm: `${caminoRestApi}/farms`,
      },
      put: {
        putFarm: `${caminoRestApi}/farms`,
      },
      delete: {
        deleteFarm: `${caminoRestApi}/farms`,
      },
    },
    products: {
      post: {
        postProduct: `${caminoRestApi}/products`,
      },
      put: {
        putProduct: `${caminoRestApi}/products`,
      },
      delete: {
        deleteProduct: `${caminoRestApi}/products`,
      },
    },
  },
};
