import React, { useState } from "react";
import styled from "styled-components";
import { RouterLinkButton } from "../../atoms/RouterLinkButtons";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { ImageCircle } from "../../atoms/Images";
import ButtonGroup from "../../atoms/ButtonGroup";
import DropdownButton from "../../molecules/DropdownButton";
import NoAvatar from "../../atoms/NoImages/no-avatar";

const Root = styled.div`
  float: right;
`;

const EmptyAvatar = styled(NoAvatar)`
  border-radius: ${p => p.theme.borderRadius.normal};
  width: 28px;
  height: 28px;
  font-size: 16px;
  display: inline-block;
  vertical-align: middle;
  margin-right: ${p => p.theme.size.exTiny};
`;

const ProfileButton = styled(RouterLinkButton)`
  ${ImageCircle} {
    height: 100%;
    margin-right: ${p => p.theme.size.exTiny};
  }
`;

const PorfileButtonGroup = styled(ButtonGroup)`
  ${ProfileButton},
  ${ButtonPrimary} {
    border: 1px solid ${p => p.theme.color.primaryLight};
    font-size: ${p => p.theme.fontSize.tiny};
    padding: 3px ${p => p.theme.size.exTiny};
    margin: 1px 0;
    font-weight: 600;
    height: ${p => p.theme.size.normal};
    vertical-align: middle;

    :hover {
      color: ${p => p.theme.color.light};
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

export default function(props) {
  const userIdentityId = props.userInfo ? props.userInfo.userIdentityId : null;
  const profileDropdowns = [
    {
      name: "Thông tin cá nhân",
      url: userIdentityId ? `/profile/${userIdentityId}` : ""
    },
    {
      name: "Thoát",
      url: "/auth/signout"
    }
  ];
  const [profileState] = useState({ dropdowns: profileDropdowns });

  return (
    <Root>
      <PorfileButtonGroup>
        <ProfileButton to={`/profile/${userIdentityId}`}>
          {props.userInfo && props.userInfo.avatarUrl ? (
            <ImageCircle
              src={`${process.env.REACT_APP_CDN_AVATAR_API_URL}${props.userInfo.avatarUrl}`}
              alt=""
            />
          ) : (
            <EmptyAvatar />
          )}

          <UserName>
            {props.userInfo ? props.userInfo.displayName : ""}
          </UserName>
        </ProfileButton>
        {profileState.dropdowns ? (
          <DropdownButton
            className="profile-dropdown"
            dropdown={profileState.dropdowns}
          />
        ) : null}
      </PorfileButtonGroup>
    </Root>
  );
}
