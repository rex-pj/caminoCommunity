import React, { Fragment } from "react";
import styled from "styled-components";
import { useLocation } from "react-router-dom";
import { PanelHeading, PanelDefault, PanelBody } from "../../molecules/Panels";
import ImageThumb from "../../molecules/Images/ImageThumb";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { secondaryTitleLink } from "../../atoms/Titles/TitleLinks";
import { HorizontalList } from "../../molecules/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { AnchorLink } from "../../atoms/Links";
import { HorizontalReactBar } from "../../molecules/Reaction";
import Overlay from "../../atoms/Overlay";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${(p) => p.theme.size.distance};

  .no-image {
    height: 140px;
  }
`;

const PostTitle = styled(secondaryTitleLink)`
  margin-bottom: 0;
  text-overflow: ellipsis;
  overflow: hidden;
  white-space: nowrap;
`;

const ContentBody = styled.div`
  padding: 0 0 ${(p) => p.theme.size.distance} 0;
  height: 160px;
`;

const InteractiveItem = styled.li`
  margin-right: ${(p) => p.theme.size.small};
  :last-child {
    margin-right: 0;
  }
`;

const ThumbnailBox = styled.div`
  position: relative;

  img {
    border-top-left-radius: ${(p) => p.theme.borderRadius.normal};
    border-top-right-radius: ${(p) => p.theme.borderRadius.normal};
  }
`;

const PanelHeader = styled(PanelHeading)`
  padding-bottom: 0;
`;

const InteractItem = styled(InteractiveItem)`
  margin-right: 0;
`;

const InteractRightItem = styled(InteractiveItem)`
  text-align: right;
  float: right;
`;

const TopBarInfo = styled.div`
  color: ${(p) => p.theme.color.neutralText};
  font-size: ${(p) => p.theme.fontSize.tiny};

  span {
    color: inherit;
    vertical-align: middle;
  }

  svg {
    margin-right: ${(p) => p.theme.size.exTiny};
    color: inherit;
    vertical-align: middle;
  }

  path {
    color: inherit;
  }
`;

export default (props) => {
  const location = useLocation();
  const { user } = props;
  return (
    <Panel>
      <ThumbnailBox>
        <ImageThumb src={user.pictureUrl} alt="" />
        <Overlay />
      </ThumbnailBox>
      <PanelHeader>
        <PostTitle>
          <AnchorLink
            to={{
              pathname: user.url,
              state: { from: location.pathname },
            }}
          >
            {user.name}
          </AnchorLink>
        </PostTitle>
        <TopBarInfo>
          {user.address ? (
            <Fragment>
              <FontAwesomeIcon icon="map-marker-alt" />
              <span>{user.address}</span>
            </Fragment>
          ) : null}
        </TopBarInfo>
      </PanelHeader>
      <PanelBody>
        <div className="panel-content">
          <ContentBody>
            <p dangerouslySetInnerHTML={{ __html: user.description }}></p>
          </ContentBody>
        </div>

        <div className="interactive-toolbar">
          <HorizontalList className="clearfix">
            <InteractItem>
              <HorizontalReactBar reactionNumber={user.reactionNumber} />
            </InteractItem>
            <InteractRightItem>
              <FontButtonItem
                icon="comments"
                dynamicText={user.commentNumber}
              />
            </InteractRightItem>
          </HorizontalList>
        </div>
      </PanelBody>
    </Panel>
  );
};
