import React, { useEffect, useState } from "react";
import styled from "styled-components";
import { RouterLinkButton } from "../../atoms/RouterLinkButtons";
import { Button } from "../../atoms/Buttons";
import { ImageCircle } from "../../atoms/Images";
import ButtonGroup from "../../atoms/ButtonGroup";
import DropdownButton from "../../molecules/DropdownButton";

const Root = styled.div`
  float: right;
`;

const ProfileButton = styled(RouterLinkButton)`
  ${ImageCircle} {
    height: 100%;
    margin-right: ${p => p.theme.size.exTiny};
  }
`;

const PorfileButtonGroup = styled(ButtonGroup)`
  ${ProfileButton},
  ${Button} {
    border: 1px solid ${p => p.theme.color.secondary};
    font-size: ${p => p.theme.fontSize.exSmall};
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
    ${Button} {
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
`;

export default function(props) {
  const [profileState, setProfileState] = useState({ dropdowns: null });

  const userIdentityId = props.userInfo ? props.userInfo.userIdentityId : null;
  useEffect(() => {
    if (profileState.dropdowns === null) {
      setProfileState({
        dropdowns: [
          {
            name: "Thông tin cá nhân",
            url: userIdentityId ? `/profile/${userIdentityId}` : ""
          },
          {
            name: "Thoát",
            url: "/auth/signout"
          }
        ]
      });
    }
  });

  return (
    <Root>
      <PorfileButtonGroup>
        <ProfileButton to={`/profile/${userIdentityId}`}>
          {props.userInfo && props.userInfo.avatarUrl ? (
            <ImageCircle
              src={`${process.env.REACT_APP_CDN_AVATAR_API_URL}${
                props.userInfo.avatarUrl
              }`}
              alt=""
            />
          ) : (
            ""
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
