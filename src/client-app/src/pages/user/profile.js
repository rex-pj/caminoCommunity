import React, { useState, useEffect, useContext } from "react";
import { useParams } from "react-router-dom";
import Profile from "../../components/organisms/Profile/Profile";
import { SessionContext } from "../../store/context/session-context";
import { userQueries } from "../../graphql/fetching/queries";
import { useQuery } from "@apollo/client";
import { useStore } from "../../store/hook-store";
import { parseUserInfo } from "../../services/UserLogic";
import { ButtonIconPrimary } from "../../components/molecules/ButtonIcons";
import styled from "styled-components";
import { ProfileLayout } from "../../components/templates/Layout";
import UserPhotoService from "../../services/userPhotoService";

const ProfileAvatar = React.lazy(() =>
    import("../../components/organisms/Profile/ProfileAvatar")
  ),
  ProfileCover = React.lazy(() =>
    import("../../components/organisms/Profile/ProfileCover")
  ),
  ProfileNavigation = React.lazy(() =>
    import("../../components/organisms/Profile/ProfileNavigation")
  );

const CoverPageBlock = styled.div`
  position: relative;
  height: 300px;
  overflow: hidden;

  h2 {
    left: 135px;
    bottom: ${(p) => p.theme.size.small};
    z-index: 3;
    margin-bottom: 0;
    position: absolute;
    color: ${(p) => p.theme.color.whiteText};
  }
`;

const ProfileNameLink = styled.a`
  font-weight: 600;
  color: ${(p) => p.theme.color.lightText};
  font-size: ${(p) => p.theme.fontSize.large};

  :hover {
    color: inherit;
  }
`;

const CoverNav = styled.div`
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  background-color: ${(p) => p.theme.color.whiteBg};
  border-bottom-left-radius: ${(p) => p.theme.borderRadius.normal};
  border-bottom-right-radius: ${(p) => p.theme.borderRadius.normal};
  margin-bottom: ${(p) => p.theme.size.distance};
`;

const AvatarBlock = styled(ProfileAvatar)`
  position: absolute;
  bottom: ${(p) => p.theme.size.distance};
  left: ${(p) => p.theme.size.distance};
  z-index: 3;
`;

const ConnectButton = styled(ButtonIconPrimary)`
  padding: ${(p) => p.theme.size.tiny};
  font-size: ${(p) => p.theme.fontSize.small};
  line-height: 1;

  position: absolute;
  bottom: ${(p) => p.theme.size.distance};
  right: ${(p) => p.theme.size.distance};
  z-index: 3;
`;

const ProfilePage = () => {
  const [isEditCoverMode, setEditCoverMode] = useState(false);
  const _baseUrl = "/profile";
  const { relogin, isLogin, currentUser } = useContext(SessionContext);
  const { userId, pageNumber } = useParams();
  const _userPhotoService = new UserPhotoService();
  const { loading, error, data, refetch } = useQuery(
    userQueries.GET_USER_INFO,
    {
      variables: {
        criterias: {
          userId,
        },
      },
    }
  );

  const [state, dispatch] = useStore(false);
  useEffect(() => {
    if (state.type === "AVATAR_UPDATED") {
      refetch();
    }
  }, [state, refetch]);

  const onToggleEditCoverMode = (e) => {
    setEditCoverMode(e);
  };

  const showValidationError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  const onUserCoverUpdated = async (variables) => {
    await _userPhotoService
      .updateCover(variables)
      .then(() => {
        refetch().then(() => {
          relogin();
        });

        return Promise.resolve({});
      })
      .catch(() => {
        showValidationError(
          "Có lỗi xảy ra",
          "Có lỗi xảy ra khi cập nhật dữ liệu, bạn vui lòng thử lại"
        );
        return Promise.reject({});
      });
  };

  const onUserCoverDeleted = async () => {
    return _userPhotoService.deleteCover().then(() => {
      refetch().then(() => {
        relogin();
      });

      return Promise.resolve({});
    });
  };

  const onAvatarUpload = async (variables) => {
    await _userPhotoService.updateAvatar(variables).then(() => {
      refetch().then(() => {
        relogin();
      });

      return Promise.resolve({});
    });
  };

  const onAvatarDelete = async () => {
    return _userPhotoService.deleteAvatar().then(() => {
      refetch().then(() => {
        relogin();
      });

      return Promise.resolve({});
    });
  };

  const userInfo = parseUserInfo(data);
  const { canEdit, userIdentityId } = userInfo;

  let currentPage;
  if (pageNumber) {
    currentPage = parseInt(pageNumber);
  }

  return (
    <ProfileLayout isLoading={!!loading} hasData={true} hasError={!!error}>
      <CoverPageBlock>
        {!isEditCoverMode &&
        isLogin &&
        currentUser.userIdentityId !== userIdentityId ? (
          <ConnectButton icon="user-plus" size="sm">
            Connect
          </ConnectButton>
        ) : null}
        <ProfileCover
          userInfo={userInfo}
          canEdit={canEdit}
          onDeleted={onUserCoverDeleted}
          onUpdated={onUserCoverUpdated}
          onToggleEditMode={onToggleEditCoverMode}
          showValidationError={showValidationError}
        />
        <AvatarBlock
          userInfo={userInfo}
          canEdit={canEdit && !isEditCoverMode}
          onUpload={onAvatarUpload}
          onDelete={onAvatarDelete}
        />
        <h2>
          <ProfileNameLink href={userInfo.url}>
            {userInfo.displayName}
          </ProfileNameLink>
        </h2>
      </CoverPageBlock>
      <CoverNav>
        <ProfileNavigation userId={userId} baseUrl={_baseUrl} />
      </CoverNav>
      <Profile
        userId={userId}
        pageNumber={currentPage}
        baseUrl={_baseUrl}
        onToggleEditCoverMode={onToggleEditCoverMode}
        showValidationError={showValidationError}
        userInfo={userInfo}
      />
    </ProfileLayout>
  );
};

export default ProfilePage;
