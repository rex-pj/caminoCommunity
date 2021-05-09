import React from "react";
import styled from "styled-components";
import { RouterLinkButtonTransparent } from "../../atoms/Buttons/RouterLinkButtons";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { ImageRound } from "../../atoms/Images";
import ButtonGroup from "../../atoms/ButtonGroup";
import DropdownButton from "../../molecules/DropdownButton";
import NoAvatar from "../../atoms/NoImages/no-avatar";

const Root = styled.div`
  float: right;
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
  color: ${(p) => p.theme.color.lightText};
  background-color: ${(p) => p.theme.rgbaColor.darkLight};
  border: 1px solid ${(p) => p.theme.color.secondaryDivide};
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
      color: ${(p) => p.theme.color.lightText};
    }
  }

  .profile-dropdown {
    ${ButtonPrimary} {
      border-left: 0;
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

export default function (props) {
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
    <Root>
      <PorfileButtonGroup>
        <ProfileButton to={`/profile/${userIdentityId}`}>
          {userInfo && userInfo.userAvatar && userInfo.userAvatar.code ? (
            <ImageRound
              src={`${process.env.REACT_APP_CDN_AVATAR_API_URL}${userInfo.userAvatar.code}`}
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
}
