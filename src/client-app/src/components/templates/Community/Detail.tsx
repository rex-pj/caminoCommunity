import * as React from "react";
import { AnchorLink } from "../../atoms/Links";
import styled from "styled-components";
import { Thumbnail } from "../../molecules/Thumbnails";
import Breadcrumb, {
  IBreadcrumbItem,
} from "../../organisms/Navigation/Breadcrumb";
import { ButtonIconPrimary } from "../../molecules/ButtonIcons";
import Overlay from "../../atoms/Overlay";
import { PanelDefault } from "../../molecules/Panels";

const GroupThumbnail = styled.div`
  margin-top: 0;
  position: relative;
`;

const BreadCrumbNav = styled(Breadcrumb)`
  border: 0;
  border-top-left-radius: 0;
  border-top-right-radius: 0;
  margin-bottom: 0;
  li {
    padding-top: ${(p) => p.theme.size.tiny};
    padding-left: ${(p) => p.theme.size.tiny};
  }
`;

const FollowButton = styled(ButtonIconPrimary)`
  padding: ${(p) => p.theme.size.tiny};
  font-size: ${(p) => p.theme.fontSize.small};
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

type Props = {
  community?: any;
  breadcrumbs: IBreadcrumbItem[];
};

const Detail = (props: Props) => {
  const { community, breadcrumbs } = props;
  return (
    <PanelDefault>
      <GroupThumbnail>
        <AnchorLink to={community.info.url}>
          <Thumbnail src={community.pictureUrl} alt="" />
        </AnchorLink>
        <ThumbnailOverlay />
        <FollowButton icon="handshake" size="sm">
          Join
        </FollowButton>
      </GroupThumbnail>
      <BreadCrumbNav list={breadcrumbs} />
    </PanelDefault>
  );
};

export default Detail;
