import React from "react";
import styled from "styled-components";
import { PanelHeading, PanelDefault, PanelBody } from "../../atoms/Panels";
import { ThumbnailRound } from "../../molecules/Thumbnails";
import { SecondaryTitleLink } from "../../atoms/Titles/TitleLinks";
import { ButtonIconOutlineSecondary } from "../../molecules/ButtonIcons";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { AnchorLink } from "../../atoms/Links";
import Overlay from "../../atoms/Overlay";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${(p) => p.theme.size.distance};
`;

const PostTitle = styled(SecondaryTitleLink)`
  margin-bottom: 0;
  text-overflow: ellipsis;
  overflow: hidden;
  white-space: nowrap;
  max-height: 48px;
`;

const ContentBody = styled.div`
  padding: 0 0 ${(p) => p.theme.size.distance} 0;
  height: 75px;
  overflow: hidden;
  text-overflow: ellipsis;
`;

const InteractiveItem = styled.li`
  margin-right: ${(p) => p.theme.size.small};
  :last-child {
    margin-right: 0;
  }
`;

const PostThumbnail = styled(ThumbnailRound)`
  border-bottom-left-radius: 0;
  border-bottom-right-radius: 0;
  max-height: 150px;
  overflow: hidden;

  img {
    border-bottom-left-radius: inherit;
    border-bottom-right-radius: inherit;
  }
`;

const ThumbnailBox = styled.div`
  position: relative;
`;

const PanelHeader = styled(PanelHeading)`
  padding-bottom: 0;
`;

const InteractItem = styled(InteractiveItem)`
  margin-right: 0;
`;

const FollowButton = styled(ButtonIconOutlineSecondary)`
  padding: ${(p) => p.theme.size.tiny};
  font-size: ${(p) => p.theme.rgbaColor.small};
  line-height: 1;

  position: absolute;
  bottom: ${(p) => p.theme.size.tiny};
  right: ${(p) => p.theme.size.tiny};
`;

const ThumbnailOverlay = styled(Overlay)`
  height: 80px;
  top: auto;
  bottom: 0;
`;

export default (props) => {
  const { association } = props;

  return (
    <Panel>
      <ThumbnailBox>
        <AnchorLink to={association.thumbnailUrl}>
          <PostThumbnail src={association.thumbnailUrl} alt="" />
          <ThumbnailOverlay />
        </AnchorLink>
        <FollowButton icon="handshake" size="sm">
          Join
        </FollowButton>
      </ThumbnailBox>
      <PanelHeader>
        <PostTitle>
          <AnchorLink to={association.url}>{association.name}</AnchorLink>
        </PostTitle>
      </PanelHeader>
      <PanelBody>
        <div className="panel-content">
          <ContentBody>{association.description}</ContentBody>
        </div>

        <div className="interactive-toolbar">
          <HorizontalList className="clearfix">
            <InteractItem>
              <FontButtonItem
                icon="users"
                dynamicText={association.followingNumber}
              />
            </InteractItem>
          </HorizontalList>
        </div>
      </PanelBody>
    </Panel>
  );
};
