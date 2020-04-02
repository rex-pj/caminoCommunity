import React, { Fragment } from "react";
import { ButtonIconOutlineSecondary } from "../../molecules/ButtonIcons";

import styled from "styled-components";
import loadable from "@loadable/component";

import ProfileBody from "./ProfileBody";

import { withRouter } from "react-router-dom";
const ProfileAvatar = loadable(() => import("./ProfileAvatar")),
  UserCoverPhoto = loadable(() => import("./UserCoverPhoto")),
  ProfileNavigation = loadable(() => import("./ProfileNavigation"));

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

export default withRouter(({ component: Component, ...rest }) => {
  const {
    isEditCoverMode,
    userId,
    pageNumber,
    baseUrl,
    pages,
    userInfo,
  } = rest;

  return (
    <Fragment>
      <CoverPageBlock>
        {!isEditCoverMode ? (
          <ConnectButton icon="user-plus" size="sm">
            Kết nối
          </ConnectButton>
        ) : null}
        <UserCoverPhoto
          userInfo={userInfo}
          canEdit={userInfo.canEdit}
          onUpdated={rest.userCoverUpdated}
          onToggleEditMode={rest.onToggleEditCoverMode}
          showValidationError={rest.showValidationError}
        />
        <AvatarBlock
          userInfo={userInfo}
          canEdit={userInfo.canEdit && !isEditCoverMode}
        />
        <h2>
          <ProfileNameLink
            href={userInfo.url ? `${baseUrl}/${userInfo.url}` : null}
          >
            {userInfo.displayName}
          </ProfileNameLink>
        </h2>
      </CoverPageBlock>
      <CoverNav>
        <ProfileNavigation userId={userId} baseUrl={baseUrl} />
      </CoverNav>
      <ProfileBody
        userInfo={userInfo}
        pageNumber={pageNumber}
        baseUrl={baseUrl}
        pages={pages}
      />
    </Fragment>
  );
});
