import React, { Fragment } from "react";
import styled from "styled-components";
import { PanelHeading, PanelDefault, PanelBody } from "../../atoms/Panels";
import ImageThumb from "../../molecules/Images/ImageThumb";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import AuthorProfile from "../ProfileCard/AuthorProfile";
import { ActionButton } from "../../molecules/ButtonGroups";
import { SecondaryTitleLink } from "../../atoms/Titles/TitleLinks";
import { HorizontalReactBar } from "../../molecules/Reaction";
import { HorizontalList } from "../../atoms/List";
import { AnchorLink } from "../../atoms/Links";
import { convertDateTimeToPeriod } from "../../../utils/DateTimeUtils";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${(p) => p.theme.size.distance};

  .no-image {
    height: 140px;
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

const DetailLink = styled.span`
  margin-left: 3px;
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
    var datePeriod = convertDateTimeToPeriod(article.createdDate);
    creator.info = (
      <Fragment>
        <FontAwesomeIcon icon="calendar-alt" />
        {datePeriod}
      </Fragment>
    );
  }

  return (
    <Panel>
      <PanelHeader>
        <ContentTopbar>
          <div className="row no-gutters">
            <div className="col col-8 col-sm-9 col-md-10 col-lg-11">
              <AuthorProfile profile={creator} />
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
          <ImageThumb src={article.thumbnailUrl} alt="" />
        </AnchorLink>
      </PostThumbnail>
      <PanelBody>
        <div className="panel-content">
          <ContentBody>
            <span
              dangerouslySetInnerHTML={{ __html: article.description }}
            ></span>
            <DetailLink>
              <AnchorLink to={article.url}>Detail</AnchorLink>
            </DetailLink>
          </ContentBody>
        </div>

        <div className="interactive-toolbar">
          <HorizontalList>
            <InteractiveItem>
              <HorizontalReactBar reactionNumber={article.reactionNumber} />
            </InteractiveItem>
          </HorizontalList>
        </div>
      </PanelBody>
    </Panel>
  );
};
