import * as React from "react";
import styled from "styled-components";
import { RouterLinkButtonTransparent } from "../../../atoms/Buttons/RouterLinkButtons";
import { ButtonPrimary } from "../../../atoms/Buttons/Buttons";
import { ImageRound } from "../../../atoms/Images";
import ButtonGroup from "../../../atoms/ButtonGroup";
import DropdownButton from "../../../molecules/DropdownButton";
import NoAvatar from "../../../molecules/NoImages/no-avatar";
import { apiConfig } from "../../../../config/api-config";

const Root = styled.div`
  display: inline-block;
`;

const EmptyAvatar = styled(NoAvatar)`
  border-radius: ${(p) => p.theme.borderRadius.normal};
  width: 28px;
  height: 28px;
  font-size: 16px;
  display: inline-block;
  vertical-align: middle;
  margin-right: ${(p) => p.theme.size.exTiny};
`;

const ProfileButton = styled(RouterLinkButtonTransparent)`
  color: ${(p) => p.theme.color.neutralText};
  ${ImageRound} {
    height: 100%;
    margin-right: ${(p) => p.theme.size.exTiny};
  }
`;

const PorfileButtonGroup = styled(ButtonGroup)`
  ${ProfileButton},
  ${ButtonPrimary} {
    border: 1px solid ${(p) => p.theme.rgbaColor.light};
    background-color: ${(p) => p.theme.rgbaColor.darkLight};
    font-size: ${(p) => p.theme.fontSize.tiny};
    padding: 3px;
    margin: 1px 0;
    font-weight: 600;
    height: ${(p) => p.theme.size.normal};
    vertical-align: middle;

    :hover {
      color: ${(p) => p.theme.color.neutralText};
    }
  }

  .profile-dropdown {
    ${ButtonPrimary} {
      border-left: 0;
      color: ${(p) => p.theme.color.neutralText};
    }

    button {
      border-top-left-radius: 0;
      border-bottom-left-radius: 0;
    }
  }
`;

const UserName = styled.span`
  vertical-align: middle;
  color: inherit;
  font-size: inherit;
`;

interface TopProfileContainerProps
  extends React.HTMLAttributes<HTMLDivElement> {
  userInfo: any;
}

const TopProfileContainer: React.FC<TopProfileContainerProps> = (props) => {
  const { userInfo } = props;
  const userIdentityId = userInfo ? userInfo.userIdentityId : null;
  const profileDropdowns = [
    {
      name: "My Profile",
      url: userIdentityId ? `/profile/${userIdentityId}` : "",
    },
    {
      name: "Logout",
      url: "/auth/logout",
    },
  ];

  return (
    <Root className={props.className}>
      <PorfileButtonGroup>
        <ProfileButton to={`/profile/${userIdentityId}`}>
          {userInfo && userInfo.userAvatar && userInfo.userAvatar.id ? (
            <ImageRound
              src={`${apiConfig.paths.userPhotos.get.getAvatar}/${userInfo.userAvatar.id}`}
              alt=""
            />
          ) : (
            <EmptyAvatar />
          )}

          <UserName className="d-none d-sm-inline">
            {userInfo ? userInfo.displayName : ""}
          </UserName>
        </ProfileButton>
        <DropdownButton
          className="profile-dropdown"
          dropdown={profileDropdowns}
        />
      </PorfileButtonGroup>
    </Root>
  );
};

export default TopProfileContainer;
