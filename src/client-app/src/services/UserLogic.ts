export const parseUserInfo = (response: any) => {
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

  const avatar = userPhotos.find((item: any) => item.photoType === "AVATAR");
  if (avatar) {
    user.userAvatar = avatar;
  }

  const cover = userPhotos.find((item: any) => item.photoType === "COVER");
  if (cover) {
    user.userCover = cover;
  }

  return user;
};
