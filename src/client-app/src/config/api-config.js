export const apiConfig = {
  camino_api: process.env.REACT_APP_CAMINO_API,
  camino_graphql: `${process.env.REACT_APP_CAMINO_API}/graphql`,
  paths: {
    userPhotos: {
      get: {
        getAvatar: `${process.env.REACT_APP_CAMINO_API}/user-photos/avatars`,
        getCover: `${process.env.REACT_APP_CAMINO_API}/user-photos/covers`,
      },
      put: {
        updateAvatar: `${process.env.REACT_APP_CAMINO_API}/user-photos/avatars`,
        updateCover: `${process.env.REACT_APP_CAMINO_API}/user-photos/covers`,
      },
      delete: {
        deleteAvatar: `${process.env.REACT_APP_CAMINO_API}/user-photos/avatars`,
        deleteCover: `${process.env.REACT_APP_CAMINO_API}/user-photos/covers`,
      },
    },
    pictures: {
      get: {
        getPicture: `${process.env.REACT_APP_CAMINO_API}/pictures`,
      },
    },
    authentications: {
      post: {
        postLogin: `${process.env.REACT_APP_CAMINO_API}/authentications/login`,
        postRefreshToken: `${process.env.REACT_APP_CAMINO_API}/authentications/refresh-tokens`,
        postForgotPassword: `${process.env.REACT_APP_CAMINO_API}/authentications/forgot-password`,
        postResetPassword: `${process.env.REACT_APP_CAMINO_API}/authentications/reset-password`,
      },
      patch: {
        patchUpdatePassword: `${process.env.REACT_APP_CAMINO_API}/authentications/update-passwords`,
      },
    },
    users: {
      post: {
        postRegister: `${process.env.REACT_APP_CAMINO_API}/users/registration`,
      },
      patch: {
        patchUser: `${process.env.REACT_APP_CAMINO_API}/users/partials`,
      },
      put: {
        putUserIdentifiers: `${process.env.REACT_APP_CAMINO_API}/users/identifiers`,
      },
    },
  },
};
