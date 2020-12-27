export const parseUserInfo = (response) => {
  if (response) {
    const { userInfo, userPhotos } = response;

    let userAvatar = {};
    let userCover = {};
    if (userPhotos && userPhotos.length > 0) {
      const avatar = userPhotos.find((item) => item.photoType === "AVATAR");
      if (avatar) {
        userAvatar = avatar;
      }
      const cover = userPhotos.find((item) => item.photoType === "COVER");
      if (cover) {
        userCover = cover;
      }
    }
    const user = {
      ...userInfo,
      userAvatar,
      userCover,
    };
    return user;
  }

  return {};
};
