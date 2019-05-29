import React from "react";
import styled from "styled-components";
import { RouterLinkButton } from "../../atoms/RouterLinkButtons";
import { Button } from "../../atoms/Buttons";
import { ImageCircle } from "../../atoms/Images";
import ButtonGroup from "../../atoms/ButtonGroup";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const Root = styled.div`
  float: right;
`;

const ProfileButton = styled(RouterLinkButton)`
  ${ImageCircle} {
    height: 100%;
    margin-right: ${p => p.theme.size.exTiny};
  }
`;

const PorfileButotnGroup = styled(ButtonGroup)`
  ${ProfileButton},
  ${Button} {
    border: 1px solid ${p => p.theme.color.secondary};
    font-size: ${p => p.theme.fontSize.exSmall};
    padding: 3px ${p => p.theme.size.exTiny};
    font-weight: 600;
    height: ${p => p.theme.size.normal};
    vertical-align: middle;

    :hover {
      color: ${p => p.theme.color.light};
    }
  }
`;

const UserName = styled.span`
  vertical-align: middle;
  color: inherit;
`;

export default () => {
  return (
    <Root>
      <PorfileButotnGroup>
        <ProfileButton to="/trungle.it">
          <ImageCircle
            src={`${process.env.PUBLIC_URL}/photos/farmer-avatar.jpg`}
            alt=""
          />
          <UserName>Lê Trung</UserName>
        </ProfileButton>
        <Button>
          <FontAwesomeIcon icon="caret-down" />
        </Button>
      </PorfileButotnGroup>
    </Root>
  );
};
