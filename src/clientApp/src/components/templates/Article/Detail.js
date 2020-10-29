import React, { Fragment } from "react";
import { PanelDefault } from "../../atoms/Panels";
import styled from "styled-components";
import { Thumbnail } from "../../molecules/Thumbnails";
import { PrimaryTitle } from "../../atoms/Titles";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { HorizontalReactBar } from "../../molecules/Reaction";
import { PanelBody, PanelHeading } from "../../atoms/Panels";
import { convertDateTimeToPeriod } from "../../../utils/DateTimeUtils";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const Title = styled(PrimaryTitle)`
  color: ${(p) => p.theme.color.primary};
`;

const ContentTopBar = styled.div`
  font-size: ${(p) => p.theme.fontSize.tiny};
  color: ${(p) => p.theme.color.neutral};

  span {
    color: inherit;
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
  margin-top: ${(p) => p.theme.size.exTiny};
`;

export default function (props) {
  const { article } = props;
  return (
    <Fragment>
      <PanelDefault>
        <PanelHeading>
          <Title>{article.name}</Title>
          <ContentTopBar>
            <FontAwesomeIcon icon="calendar-alt" />
            <span>{convertDateTimeToPeriod(article.createdDate)}</span>
          </ContentTopBar>
        </PanelHeading>
        {article.thumbnailUrl ? (
          <PostThumbnail>
            <Thumbnail src={article.thumbnailUrl} alt="" />
          </PostThumbnail>
        ) : null}

        <PanelBody>
          <div className="clearfix">
            <div>
              <ContentBody>
                <span
                  dangerouslySetInnerHTML={{ __html: article.content }}
                ></span>
              </ContentBody>

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
                      title="Discussions"
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
      </PanelDefault>
    </Fragment>
  );
}
