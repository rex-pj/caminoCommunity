export const apiConfig = {
  camino_api: process.env.REACT_APP_CAMINO_API,
  camino_graphql: `${process.env.REACT_APP_CAMINO_API}/graphql`,
  paths: {
    userPhotos: {
      get: {
        getAvatar: `${process.env.REACT_APP_CAMINO_API}/user-photos/avatars`,
        getCover: `${process.env.REACT_APP_CAMINO_API}/user-photos/covers`,
      },
    },
    pictures: {
      get: {
        getPicture: `${process.env.REACT_APP_CAMINO_API}/pictures`,
      },
    },
  },
};
