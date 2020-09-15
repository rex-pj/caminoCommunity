import React, { Fragment } from "react";
import { ButtonIconOutlineSecondary } from "../../molecules/ButtonIcons";
import styled from "styled-components";
import { Switch, withRouter } from "react-router-dom";
import loadable from "@loadable/component";
import Timeline from "./Timeline";

const AsyncTabContent = loadable((props) =>
    import(`${"../../../pages/user/"}${props.page}`)
  ),
  UserProfileInfo = loadable(() => import("./UserProfileInfo"));

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

export default withRouter((props) => {
  const { isEditCoverMode, userId, baseUrl, pages, userInfo } = props;

  const { canEdit } = userInfo;

  console.log(baseUrl);
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
          onUpdated={props.userCoverUpdated}
          onToggleEditMode={props.onToggleEditCoverMode}
          showValidationError={props.showValidationError}
        />
        <AvatarBlock
          userInfo={userInfo}
          canEdit={canEdit && !isEditCoverMode}
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
      <Fragment>
        <div className="row">
          <div className="col col-8 col-sm-8 col-md-8 col-lg-9">
            <Switch>
              {pages
                ? pages.map((item) => {
                    return (
                      <Timeline
                        key={item.page}
                        path={item.path}
                        component={(props) => (
                          <AsyncTabContent
                            {...props}
                            userId={userId}
                            canEdit={canEdit}
                            userUrl={`${baseUrl}/${userId}`}
                            page={item.page}
                          />
                        )}
                      />
                    );
                  })
                : null}

              <Timeline
                path={[`${baseUrl}`]}
                exact={true}
                component={() => <div>NOT FOUND</div>}
              />
            </Switch>
          </div>
          <div className="col col-4 col-sm-4 col-md-4 col-lg-3">
            <UserProfileInfo userInfo={userInfo} />
          </div>
        </div>
      </Fragment>
    </Fragment>
  );
});
