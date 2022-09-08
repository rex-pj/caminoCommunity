import React, { Fragment, useState, useEffect, useContext } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import Profile from "../../components/organisms/Profile/Profile";
import { SessionContext } from "../../store/context/session-context";
import { userQueries } from "../../graphql/fetching/queries";
import { useQuery, useMutation } from "@apollo/client";
import {
  ErrorBar,
  LoadingBar,
} from "../../components/molecules/NotificationBars";
import { useStore } from "../../store/hook-store";
import { parseUserInfo } from "../../services/userService";
import UserProfileRoutes from "../../routes/userProfileRoutes";
import { ButtonIconPrimary } from "../../components/molecules/ButtonIcons";
import styled from "styled-components";
import { userMutations } from "../../graphql/fetching/mutations";
import { authClient } from "../../graphql/client";

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

export default (props) => {
  const [isEditCoverMode, setEditCoverMode] = useState(false);
  const _baseUrl = "/profile";
  const { relogin, isLogin, currentUser } = useContext(SessionContext);
  const { match } = props;
  const { params } = match;
  const { userId, pageNumber } = params;
  const [updateAvatar] = useMutation(userMutations.UPDATE_USER_AVATAR, {
    client: authClient,
  });
  const [deleteAvatar] = useMutation(userMutations.DELETE_USER_AVATAR, {
    client: authClient,
  });
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

  if (loading) {
    return <LoadingBar>Loading</LoadingBar>;
  }

  if (error) {
    return <ErrorBar>Error</ErrorBar>;
  }

  if (!data) {
    return <ErrorBar>Not Found</ErrorBar>;
  }

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

  const userCoverUpdated = async (action, data) => {
    if (data && data.canEdit) {
      return await action({ variables: { criterias: data } })
        .then(async () => {
          refetch().then(() => {
            relogin();
          });
        })
        .catch(() => {
          showValidationError(
            "Có lỗi xảy ra",
            "Có lỗi xảy ra khi cập nhật dữ liệu, bạn vui lòng thử lại"
          );
        });
    }
  };

  const onAvatarUpload = async (variables) => {
    return await updateAvatar({ variables }).then(() => {
      refetch().then(() => {
        relogin();
      });

      return new Promise((resolve) => {
        resolve({});
      });
    });
  };

  const onAvatarDelete = async (variables) => {
    return await deleteAvatar({ variables }).then(() => {
      refetch().then(() => {
        relogin();
      });

      return new Promise((resolve) => {
        resolve({});
      });
    });
  };

  const userInfo = parseUserInfo(data);
  const { canEdit, userIdentityId } = userInfo;

  let currentPage;
  if (pageNumber) {
    currentPage = parseInt(pageNumber);
  }

  return (
    <Fragment>
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
          onUpdated={userCoverUpdated}
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
        userCoverUpdated={userCoverUpdated}
        showValidationError={showValidationError}
        pages={UserProfileRoutes}
        userInfo={userInfo}
      />
    </Fragment>
  );
};
