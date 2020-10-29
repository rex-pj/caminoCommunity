import React, { useState, Fragment } from "react";
import styled from "styled-components";
import { PanelHeading, PanelDefault, PanelBody } from "../../atoms/Panels";
import { Thumbnail } from "../../molecules/Thumbnails";
import NoImage from "../../atoms/NoImages/no-image";
import Overlay from "../../atoms/Overlay";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ProfileAction from "../ProfileCard/ProfileAction";
import { ActionButton } from "../../molecules/ButtonGroups";
import { SecondaryTitleLink } from "../../atoms/Titles/TitleLinks";
import { HorizontalReactBar } from "../../molecules/Reaction";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { AnchorLink } from "../../atoms/Links";
import { convertDateTimeToPeriod } from "../../../utils/DateTimeUtils";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${(p) => p.theme.size.distance};

  .no-image {
    height: 200px;
    border-radius: 0;
  }
`;

const ContentTopbar = styled.div`
  margin-bottom: ${(p) => p.theme.size.exSmall};
`;

const PostActions = styled.div`
  text-align: right;
`;

const PostTitle = styled(SecondaryTitleLink)`
  margin-bottom: 0;
`;

const ContentBody = styled.div`
  padding: 0 0 ${(p) => p.theme.size.distance} 0;
`;

const InteractiveItem = styled.li`
  margin-right: ${(p) => p.theme.size.small};
  :last-child {
    margin-right: 0;
  }
`;

const CollapsedThumbnail = styled.div`
  margin-top: ${(p) => p.theme.size.distance};
  overflow-y: hidden;
  max-height: 300px;
  position: relative;
`;

const ExpandedThumbnail = styled.div`
  margin-top: ${(p) => p.theme.size.distance};
`;

const ThumbnailOverlay = styled(Overlay)`
  height: 80px;
  top: auto;
  bottom: 0;
  cursor: pointer;
`;

const PanelHeader = styled(PanelHeading)`
  padding-bottom: 0;
`;

export default (props) => {
  const { article } = props;
  const { creator } = article;
  const [isShowFullImage, setShowFullImage] = useState(false);

  if (creator) {
    var datePeriod = convertDateTimeToPeriod(article.createdDate);
    creator.info = (
      <Fragment>
        <FontAwesomeIcon icon="calendar-alt" />
        {datePeriod}
      </Fragment>
    );
  }

  function showFullImage() {
    setShowFullImage(!isShowFullImage);
  }

  let thumbnailBox = null;
  if (!article.thumbnailUrl) {
    thumbnailBox = <NoImage className="no-image mt-2"></NoImage>;
  } else if (!isShowFullImage) {
    thumbnailBox = (
      <CollapsedThumbnail>
        <Thumbnail src={article.thumbnailUrl} alt="" />
        <ThumbnailOverlay onClick={showFullImage}></ThumbnailOverlay>
      </CollapsedThumbnail>
    );
  } else if (isShowFullImage) {
    thumbnailBox = (
      <ExpandedThumbnail>
        <Thumbnail src={article.thumbnailUrl} alt="" />
      </ExpandedThumbnail>
    );
  }

  return (
    <Panel>
      <PanelHeader>
        <ContentTopbar>
          <div className="row no-gutters">
            <div className="col col-8 col-sm-9 col-md-10 col-lg-11">
              <ProfileAction profile={creator} />
            </div>

            <div className="col col-4 col-sm-3 col-md-2 col-lg-1">
              <PostActions>
                <ActionButton>
                  <FontAwesomeIcon icon="angle-down" />
                </ActionButton>
              </PostActions>
            </div>
          </div>
        </ContentTopbar>
        <PostTitle>
          <AnchorLink to={article.url}>{article.name}</AnchorLink>
        </PostTitle>
      </PanelHeader>
      {thumbnailBox}
      <PanelBody>
        <div className="panel-content">
          <ContentBody>
            <p>{article.description}</p>
          </ContentBody>
        </div>

        <div className="interactive-toolbar">
          <HorizontalList>
            <InteractiveItem>
              <HorizontalReactBar reactionNumber={article.reactionNumber} />
            </InteractiveItem>
            <InteractiveItem>
              <FontButtonItem
                icon="comments"
                title="Discussions"
                dynamicText={article.commentNumber}
              />
            </InteractiveItem>
            <InteractiveItem>
              <FontButtonItem icon="bookmark" title="Save this article" />
            </InteractiveItem>
          </HorizontalList>
        </div>
      </PanelBody>
    </Panel>
  );
};
