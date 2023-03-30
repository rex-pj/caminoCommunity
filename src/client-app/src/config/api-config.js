import { envConfig } from "./env-config";
const camino_rest = `${envConfig.camino_host}/api`;
const camino_graphql = `${envConfig.camino_host}/graphql`;

export const apiConfig = {
  camino_api: camino_rest,
  camino_graphql: camino_graphql,
  paths: {
    userPhotos: {
      get: {
        getAvatar: `${camino_rest}/user-photos/avatars`,
        getCover: `${camino_rest}/user-photos/covers`,
      },
      put: {
        updateAvatar: `${camino_rest}/user-photos/avatars`,
        updateCover: `${camino_rest}/user-photos/covers`,
      },
      delete: {
        deleteAvatar: `${camino_rest}/user-photos/avatars`,
        deleteCover: `${camino_rest}/user-photos/covers`,
      },
    },
    pictures: {
      get: {
        getPicture: `${camino_rest}/pictures`,
      },
      put: {
        putValidatePicture: `${camino_rest}/pictures/validations`,
      },
    },
    authentications: {
      post: {
        postLogin: `${camino_rest}/authentications/login`,
        postRefreshToken: `${camino_rest}/authentications/refresh-tokens`,
        postForgotPassword: `${camino_rest}/authentications/forgot-password`,
        postResetPassword: `${camino_rest}/authentications/reset-password`,
      },
      patch: {
        patchUpdatePassword: `${camino_rest}/authentications/update-passwords`,
      },
    },
    users: {
      post: {
        postRegister: `${camino_rest}/users/registration`,
      },
      patch: {
        patchUser: `${camino_rest}/users/partials`,
      },
      put: {
        putUserIdentifiers: `${camino_rest}/users/identifiers`,
      },
    },
    articles: {
      post: {
        postArticle: `${camino_rest}/articles`,
      },
      put: {
        putArticle: `${camino_rest}/articles`,
      },
      delete: {
        deleteArticle: `${camino_rest}/articles`,
      },
    },
    farms: {
      post: {
        postFarm: `${camino_rest}/farms`,
      },
      put: {
        putFarm: `${camino_rest}/farms`,
      },
      delete: {
        deleteFarm: `${camino_rest}/farms`,
      },
    },
    products: {
      post: {
        postProduct: `${camino_rest}/products`,
      },
      put: {
        putProduct: `${camino_rest}/products`,
      },
      delete: {
        deleteProduct: `${camino_rest}/products`,
      },
    },
  },
};
