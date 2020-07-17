import React, { Component, Fragment } from "react";
import { AnchorLink } from "../../atoms/Links";
import styled from "styled-components";
import { Thumbnail } from "../../molecules/Thumbnails";
import { PrimaryTitle } from "../../atoms/Titles";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { HorizontalReactBar } from "../../molecules/Reaction";
import { PanelBody, PanelHeading } from "../../atoms/Panels";

const Title = styled(PrimaryTitle)`
  color: ${p => p.theme.color.primary};
`;

const ContentTopBar = styled.div`
  font-size: ${p => p.theme.fontSize.tiny};
  color: ${p => p.theme.color.neutral};

  span {
    color: inherit;
  }
`;

const ContentBody = styled.div`
  padding: 0 0 ${p => p.theme.size.distance} 0;
`;

const InteractiveItem = styled.li`
  margin-right: ${p => p.theme.size.small};
  :last-child {
    margin-right: 0;
  }
`;

const PostThumbnail = styled.div`
  margin-top: ${p => p.theme.size.exTiny};
`;

export default class extends Component {
  render() {
    const { article } = this.props;
    return (
      <Fragment>
        <PanelHeading>
          <Title>{article.title}</Title>
          <ContentTopBar>
            <span>Bài được đăng lúc </span>
            <span>{article.createdDate}</span>
          </ContentTopBar>
        </PanelHeading>
        <PostThumbnail>
          <AnchorLink to={article.url}>
            <Thumbnail src={article.thumbnailUrl} alt="" />
          </AnchorLink>
        </PostThumbnail>
        <PanelBody>
          <div className="clearfix">
            <div>
              <ContentBody>{article.content}</ContentBody>

              <div className="interactive-toolbar">
                <HorizontalList>
                  <InteractiveItem>
                    <HorizontalReactBar
                      reactionNumber={article.reactionNumber}
                    />
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
            </div>
          </div>
        </PanelBody>
      </Fragment>
    );
  }
}
