import React from "react";
import { AnchorLink } from "../../atoms/Links";
import styled from "styled-components";
import { Thumbnail } from "../../molecules/Thumbnails";
import Breadcrumb from "../../organisms/Navigation/Breadcrumb";
import { ButtonIconOutlineSecondary } from "../../molecules/ButtonIcons";
import Overlay from "../../atoms/Overlay";
import { PanelDefault } from "../../atoms/Panels";

const GroupThumbnail = styled.div`
  margin-top: 0;
  position: relative;
`;

const BreadCrumbNav = styled(Breadcrumb)`
  border: 0;
  border-top-left-radius: 0;
  border-top-right-radius: 0;
  margin-bottom: 0;
`;

const FollowButton = styled(ButtonIconOutlineSecondary)`
  padding: ${(p) => p.theme.size.tiny};
  font-size: ${(p) => p.theme.rgbaColor.small};
  line-height: 1;

  position: absolute;
  bottom: ${(p) => p.theme.size.distance};
  right: ${(p) => p.theme.size.distance};
  z-index: 1;
`;

const ThumbnailOverlay = styled(Overlay)`
  height: 100px;
  top: auto;
  bottom: 0;
`;

export default function (props) {
  const { community, breadcrumbs } = props;
  return (
    <PanelDefault>
      <GroupThumbnail>
        <AnchorLink to={community.info.url}>
          <Thumbnail src={community.thumbnailUrl} alt="" />
        </AnchorLink>
        <ThumbnailOverlay />
        <FollowButton icon="handshake" size="sm">
          Join
        </FollowButton>
      </GroupThumbnail>
      <BreadCrumbNav list={breadcrumbs} />
    </PanelDefault>
  );
}
