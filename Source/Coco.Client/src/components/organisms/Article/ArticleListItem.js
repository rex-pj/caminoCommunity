import React from "react";
import styled from "styled-components";
import { PanelHeading, PanelDefault, PanelBody } from "../../atoms/Panels";
import { Thumbnail } from "../../molecules/Thumbnails";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ProfileAction from "../ProfileCard/ProfileAction";
import { ActionButton } from "../../molecules/ButtonGroups";
import { SecondaryTitle } from "../../atoms/Titles";
import { HorizontalReactBar } from "../../molecules/Reaction";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { AnchorLink } from "../../atoms/Links";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${p => p.theme.size.distance};
`;

const ContentTopbar = styled.div`
  margin-bottom: ${p => p.theme.size.exSmall};
`;

const PostActions = styled.div`
  text-align: right;
`;

const PostTitle = styled(SecondaryTitle)`
  margin-bottom: 0;
`;

const ContentBody = styled.div`
  padding: 0 0 ${p => p.theme.size.distance} 0;
`;

const DetailLink = styled.span`
  margin-left: 3px;
`;

const InteractiveItem = styled.li`
  margin-right: ${p => p.theme.size.small};
  :last-child {
    margin-right: 0;
  }
`;

const PostThumbnail = styled.div`
  margin-top: ${p => p.theme.size.distance};
`;

const PanelHeader = styled(PanelHeading)`
  padding-bottom: 0;
`;

export default function(props) {
  const { article } = props;
  const { creator } = article;

  if (creator) {
    creator.info = `Gửi bài lúc ${article.createdDate}`;
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
      <PostThumbnail>
        <AnchorLink to={article.url}>
          <Thumbnail src={article.thumbnailUrl} alt="" />
        </AnchorLink>
      </PostThumbnail>
      <PanelBody>
        <div className="panel-content">
          <ContentBody>
            {article.description}
            <DetailLink>
              <AnchorLink to={article.url}>Xem Thêm</AnchorLink>
            </DetailLink>
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
                title="Thảo luận"
                dynamicText={article.commentNumber}
              />
            </InteractiveItem>
            <InteractiveItem>
              <FontButtonItem icon="bookmark" title="Đánh dấu" />
            </InteractiveItem>
          </HorizontalList>
        </div>
      </PanelBody>
    </Panel>
  );
};
