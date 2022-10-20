export const parseUserInfo = (response) => {
  if (!response) {
    return {};
  }

  const { userInfo, userPhotos } = response;
  let user = {
    ...userInfo,
    userAvatar: {},
    userCover: {},
  };
  if (!userPhotos || userPhotos.length === 0) {
    return user;
  }

  const avatar = userPhotos.find((item) => item.photoType === "AVATAR");
  if (avatar) {
    user.userAvatar = avatar;
  }

  const cover = userPhotos.find((item) => item.photoType === "COVER");
  if (cover) {
    user.userCover = cover;
  }

  return user;
};
