import * as React from "react";
import { Fragment } from "react";
import AuthorProfile from "../ProfileCard/AuthorProfile";
import styled from "styled-components";
import { PanelHeading, PanelDefault, PanelBody } from "../../molecules/Panels";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ImageThumb from "../../molecules/Images/ImageThumb";
import { secondaryTitleLink } from "../../atoms/Titles/TitleLinks";
import { AnchorLink } from "../../atoms/Links";
import { convertDateTimeToPeriod } from "../../../utils/DateTimeUtils";
import ModuleMenuListItem from "../../molecules/MenuList/ModuleMenuListItem";
import { useLocation } from "react-router-dom";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${(p) => p.theme.size.distance};

  .no-image {
    height: 200px;
    border-radius: 0;
  }

  img.thumbnail-img {
    width: 100%;
  }
`;

const PanelHeader = styled(PanelHeading)`
  padding-bottom: 0;
`;

const ContentTopbar = styled.div`
  margin-bottom: 0;

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

const Title = styled(secondaryTitleLink)`
  margin-bottom: 0;
`;

const Description = styled.div`
  margin-bottom: 0;
`;

const InteractiveToolbar = styled.div`
  border-top: 1px solid ${(p) => p.theme.color.neutralBorder};
  padding: ${(p) => p.theme.size.exSmall} ${(p) => p.theme.size.distance};
`;

const InteractiveItem = styled.li`
  margin-right: ${(p) => p.theme.size.small};
  :last-child {
    margin-right: 0;
  }
`;

const TopBarInfo = styled.span`
  color: ${(p) => p.theme.color.neutralText};
  font-size: ${(p) => p.theme.fontSize.tiny};
  position: relative;

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

interface Props {
  farm?: any;
}

const FarmSearchedItem = (props: Props) => {
  const location = useLocation();
  const { farm } = props;
  const loadCreatedInfo = () => {
    const { creator } = farm;
    if (creator) {
      creator.info = (
        <Fragment>
          <TopBarInfo>
            <span className="me-3">
              <FontAwesomeIcon icon="calendar-alt" />
              <span>{convertDateTimeToPeriod(farm.createdDate)}</span>
            </span>
            {farm.address ? (
              <Fragment>
                <FontAwesomeIcon icon="map-marker-alt" />
                <span>{farm.address}</span>
              </Fragment>
            ) : null}
          </TopBarInfo>
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
          <div className="col col-9 col-sm-9 col-md-10 col-lg-10">
            <Title className="mb-3">
              <AnchorLink
                to={{
                  pathname: farm.url,
                  state: { from: location.pathname },
                }}
              >
                {farm.name}
              </AnchorLink>
            </Title>
            <div className="panel-content">
              <Description>
                <span
                  dangerouslySetInnerHTML={{ __html: farm.description }}
                ></span>{" "}
              </Description>
            </div>
          </div>
          <div className="col col-3 col-sm-3 col-md-2 col-lg-2">
            <AnchorLink
              to={{
                pathname: farm.url,
                state: { from: location.pathname },
              }}
            >
              <ImageThumb src={farm.pictureUrl} alt="" />
            </AnchorLink>
          </div>
        </div>
      </PanelBody>
      {/* <InteractiveToolbar>
        <HorizontalList>
          <InteractiveItem>
            <HorizontalReactBar reactionNumber={farm.reactionNumber} />
          </InteractiveItem>

          <InteractiveItem>
            <FontButtonItem
              icon="comments"
              title="Discussions"
              dynamicText={farm.commentNumber}
            />
          </InteractiveItem>
        </HorizontalList>
      </InteractiveToolbar> */}
    </Panel>
  );
};

export default FarmSearchedItem;
