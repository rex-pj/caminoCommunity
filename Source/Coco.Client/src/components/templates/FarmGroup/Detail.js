import React from "react";
import { AnchorLink } from "../../atoms/Links";
import styled from "styled-components";
import { Thumbnail } from "../../molecules/Thumbnails";
import Breadcrumb from "../../molecules/Breadcrumb";
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
  padding: ${p => p.theme.size.tiny};
  font-size: ${p => p.theme.fontSize.exSmall};
  line-height: 1;

  position: absolute;
  bottom: ${p => p.theme.size.distance};
  right: ${p => p.theme.size.distance};
  z-index: 1;
`;

const ThumbnailOverlay = styled(Overlay)`
  height: 100px;
  top: auto;
  bottom: 0;
`;

export default function(props) {
  const { farmGroup, breadcrumbs } = props;
  return (
    <PanelDefault>
      <GroupThumbnail>
        <AnchorLink to={farmGroup.info.url}>
          <Thumbnail src={farmGroup.thumbnailUrl} alt="" />
        </AnchorLink>
        <ThumbnailOverlay />
        <FollowButton icon="handshake" size="sm">
          Tham gia
        </FollowButton>
      </GroupThumbnail>
      <BreadCrumbNav list={breadcrumbs} />
    </PanelDefault>
  );
}
