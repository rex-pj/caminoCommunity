import React, { Fragment } from "react";
import styled from "styled-components";
import { useLocation } from "react-router-dom";
import { PanelHeading, PanelDefault, PanelBody } from "../../molecules/Panels";
import ImageThumb from "../../molecules/Images/ImageThumb";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import AuthorProfile from "../ProfileCard/AuthorProfile";
import { secondaryTitleLink } from "../../atoms/Titles/TitleLinks";
import { HorizontalReactBar } from "../../molecules/Reaction";
import { HorizontalList } from "../../molecules/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { AnchorLink } from "../../atoms/Links";
import { convertDateTimeToPeriod } from "../../../utils/DateTimeUtils";
import ContentItemDropdown from "../../molecules/DropdownButton/ContentItemDropdown";
import ModuleMenuListItem from "../../molecules/MenuList/ModuleMenuListItem";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${(p) => p.theme.size.distance};

  .no-image {
    height: 200px;
    border-radius: 0;
  }

  img {
    width: 100%;
  }

  ${ContentItemDropdown} {
    z-index: 10;
  }
`;

const ContentTopbar = styled.div`
  margin-bottom: ${(p) => p.theme.size.exSmall};
  position: relative;

  ${ModuleMenuListItem} {
    margin-top: 0;
    margin-bottom: 0;
    border-bottom: 1px solid ${(p) => p.theme.color.neutralBg};
  }

  ${ModuleMenuListItem}:last-child {
    border-bottom: 0;
  }

  ${ModuleMenuListItem} span {
    padding-top: ${(p) => p.theme.size.tiny};
    padding-bottom: ${(p) => p.theme.size.tiny};
  }
`;

const PostTitle = styled(secondaryTitleLink)`
  margin-bottom: ${(p) => p.theme.size.distance};
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

const InteractiveToolbar = styled.div`
  border-top: 1px solid ${(p) => p.theme.color.neutralBorder};
  padding: ${(p) => p.theme.size.exSmall} ${(p) => p.theme.size.distance};
`;

const PanelHeader = styled(PanelHeading)`
  padding-bottom: 0;
`;

const ArticleSearchItem = (props) => {
  const location = useLocation();
  const { article } = props;

  const loadCreatedInfo = () => {
    const { creator } = article;
    if (creator) {
      var datePeriod = convertDateTimeToPeriod(article.createdDate);
      creator.info = (
        <Fragment>
          <FontAwesomeIcon icon="calendar-alt" />
          <span>{datePeriod}</span>
        </Fragment>
      );
    }
    return creator;
  };

  return (
    <Panel>
      <PanelHeader>
        <ContentTopbar>
          <div className="row g-0">
            <div className="col col-8 col-sm-9 col-md-10 col-lg-11">
              <AuthorProfile profile={loadCreatedInfo()} />
            </div>
          </div>
        </ContentTopbar>
      </PanelHeader>

      <PanelBody className="panel-content">
        <div className="row">
          <div className="col col-9 col-sm-9 col-md-19 col-lg-10">
            <PostTitle>
              <AnchorLink
                to={{
                  pathname: article.url,
                  state: { from: location.pathname },
                }}
              >
                {article.name}
              </AnchorLink>
            </PostTitle>
            <ContentBody>
              <p dangerouslySetInnerHTML={{ __html: article.description }}></p>{" "}
            </ContentBody>
          </div>
          <div className="col col-3 col-sm-3 col-md-2 col-lg-2">
            <AnchorLink
              to={{
                pathname: article.url,
                state: { from: location.pathname },
              }}
            >
              <ImageThumb src={article.pictureUrl} />
            </AnchorLink>
          </div>
        </div>
      </PanelBody>
      <InteractiveToolbar className="interactive-toolbar">
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
      </InteractiveToolbar>
    </Panel>
  );
};

export default ArticleSearchItem;
