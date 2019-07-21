import React from "react";
import styled from "styled-components";
import { ImageCircle } from "../../atoms/Images";
import { Thumbnail } from "../../molecules/Thumbnails";
import Menubar from "./Menubar";
import Overlay from "../../atoms/Overlay";
import { AnchorLink } from "../../atoms/Links";

const Root = styled.div`
  position: relative;
`;

const CoverWrapper = styled.div`
  max-height: 90px;
  overflow: hidden;
  position: relative;

  img {
    border-top-left-radius: ${p => p.theme.borderRadius.normal};
    border-top-right-radius: ${p => p.theme.borderRadius.normal};
  }
`;

const Username = styled.h3`
  margin-bottom: 0;
  position: absolute;
  bottom: calc(${p => p.theme.size.normal} + 15px);
  left: 15px;
  z-index: 1;
  line-height: 1;
  text-shadow: ${p => p.theme.shadow.TextShadow};
  a {
    color: ${p => p.theme.color.white};
  }
`;

const ProfileImage = styled(ImageCircle)`
  position: absolute;
  top: -20px;
  left: 15px;
  width: 55px;
  height: 55px;
  border: 5px solid ${p => p.theme.rgbaColor.cyanMoreLight};
  z-index: 1;
`;

const BoxShadowBar = styled.div`
  position: relative;
`;

const StaticBar = styled.div`
  height: ${p => p.theme.size.medium};
  border-bottom-left-radius: ${p => p.theme.borderRadius.normal};
  border-bottom-right-radius: ${p => p.theme.borderRadius.normal};
`;

export default function(props) {
  const { className, menuList } = props;
  const { userInfo } = props;
  const userIdentityId = userInfo ? userInfo.userIdentityId : null;
  const { avatarUrl } = userInfo;
  return (
    <Root className={className}>
      {avatarUrl ? (
        <ProfileImage
          src={`${process.env.REACT_APP_CDN_AVATAR_API_URL}${avatarUrl}`}
        />
      ) : null}

      <BoxShadowBar>
        <CoverWrapper>
          <Thumbnail
            src={`${process.env.PUBLIC_URL}/photos/profile-card.jpg`}
          />
          <Overlay />
        </CoverWrapper>
        <StaticBar>
          <Menubar menuList={menuList} />
        </StaticBar>
        <Username>
          <AnchorLink to={userIdentityId ? `/profile/${userIdentityId}` : ""}>
            {props.userInfo ? props.userInfo.displayName : ""}
          </AnchorLink>
        </Username>
      </BoxShadowBar>
    </Root>
  );
}
