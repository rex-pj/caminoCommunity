import React, { Fragment, useState, useEffect, useContext } from "react";
import { withRouter } from "react-router-dom";
import Profile from "../../components/organisms/User/Profile";
import { SessionContext } from "../../store/context/SessionContext";
import { GET_USER_INFO } from "../../utils/GraphQLQueries/queries";
import { useQuery } from "@apollo/client";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import Loading from "../../components/atoms/Loading";
import { useStore } from "../../store/hook-store";
import { parseUserInfo } from "../../services/UserService";
import UserProfileRoutes from "../../routes/UserProfileRoutes";
import { ButtonIconOutlineSecondary } from "../../components/molecules/ButtonIcons";
import styled from "styled-components";
import loadable from "@loadable/component";

const ProfileAvatar = loadable(() =>
    import("../../components/organisms/User/ProfileAvatar")
  ),
  UserCoverPhoto = loadable(() =>
    import("../../components/organisms/User/UserCoverPhoto")
  ),
  ProfileNavigation = loadable(() =>
    import("../../components/organisms/User/ProfileNavigation")
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
    color: ${(p) => p.theme.color.white};
  }
`;

const ProfileNameLink = styled.a`
  font-weight: 600;
  font-size: ${(p) => p.theme.fontSize.large};
`;

const CoverNav = styled.div`
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  background-color: ${(p) => p.theme.color.white};
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

const ConnectButton = styled(ButtonIconOutlineSecondary)`
  padding: ${(p) => p.theme.size.tiny};
  font-size: ${(p) => p.theme.rgbaColor.small};
  line-height: 1;

  position: absolute;
  bottom: ${(p) => p.theme.size.distance};
  right: ${(p) => p.theme.size.distance};
  z-index: 3;
`;

export default withRouter((props) => {
  const [isEditCoverMode, setEditCoverMode] = useState(false);
  const _baseUrl = "/profile";
  const sessionContext = useContext(SessionContext);
  const { match } = props;
  const { params } = match;
  const { userId, pageNumber } = params;
  const { loading, error, data, refetch } = useQuery(GET_USER_INFO, {
    variables: {
      criterias: {
        userId,
      },
    },
  });

  const [state, dispatch] = useStore(false);
  useEffect(() => {
    if (state.type === "AVATAR_UPDATED") {
      refetch();
    }
  }, [state, refetch, sessionContext]);

  if (loading) {
    return <Loading>Loading</Loading>;
  }

  if (error) {
    return <ErrorBlock>Error</ErrorBlock>;
  }

  if (!data) {
    return <ErrorBlock>Not Found</ErrorBlock>;
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
          await refetch().then(() => {
            if (sessionContext.relogin) {
              sessionContext.relogin();
            }
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

  const userInfo = parseUserInfo(data);
  const { canEdit } = userInfo;

  let currentPage = 0;
  if (pageNumber) {
    currentPage = parseInt(pageNumber);
  }

  return (
    <Fragment>
      <CoverPageBlock>
        {!isEditCoverMode ? (
          <ConnectButton icon="user-plus" size="sm">
            Connect
          </ConnectButton>
        ) : null}
        <UserCoverPhoto
          userInfo={userInfo}
          canEdit={canEdit}
          onUpdated={userCoverUpdated}
          onToggleEditMode={onToggleEditCoverMode}
          showValidationError={showValidationError}
        />
        <AvatarBlock
          userInfo={userInfo}
          canEdit={canEdit && !isEditCoverMode}
        />
        <h2>
          <ProfileNameLink
            href={userInfo.url ? `${_baseUrl}/${userInfo.url}` : null}
          >
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
});
