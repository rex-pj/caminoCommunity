import React from "react";
import styled from "styled-components";
import { PanelHeading, PanelDefault, PanelBody } from "../../atoms/Panels";
import { Thumbnail } from "../../molecules/Thumbnails";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ProfileAction from "../ProfileCard/ProfileAction";
import { ActionButton } from "../../molecules/ButtonGroups";
import { SecondaryTitleLink } from "../../atoms/Titles/TitleLinks";
import { HorizontalReactBar } from "../../molecules/Reaction";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { AnchorLink } from "../../atoms/Links";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${(p) => p.theme.size.distance};
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

const PostThumbnail = styled.div`
  margin-top: ${(p) => p.theme.size.distance};
`;

const PanelHeader = styled(PanelHeading)`
  padding-bottom: 0;
`;

export default (props) => {
  const { article } = props;
  const { creator } = article;

  if (creator) {
    creator.info = `Created at ${article.createdDate}`;
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
      {article.thumbnailUrl ? (
        <PostThumbnail>
          <AnchorLink to={article.url}>
            <Thumbnail src={article.thumbnailUrl} alt="" />
          </AnchorLink>
        </PostThumbnail>
      ) : null}
      <PanelBody>
        <div className="panel-content">
          <ContentBody>
            <p dangerouslySetInnerHTML={{ __html: article.content }}></p>
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
