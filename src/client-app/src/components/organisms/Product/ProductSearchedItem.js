import React, { Fragment } from "react";
import AuthorProfile from "../ProfileCard/AuthorProfile";
import { withRouter } from "react-router-dom";
import { HorizontalReactBar } from "../../molecules/Reaction";
import styled from "styled-components";
import { PanelHeading, PanelDefault, PanelBody } from "../../molecules/Panels";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ImageThumb from "../../molecules/Images/ImageThumb";
import { secondaryTitleLink } from "../../atoms/Titles/TitleLinks";
import { AnchorLink } from "../../atoms/Links";
import { HorizontalList } from "../../molecules/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { PriceLabel } from "../../molecules/PriceAndCurrency";
import { convertDateTimeToPeriod } from "../../../utils/DateTimeUtils";
import ContentItemDropdown from "../../molecules/DropdownButton/ContentItemDropdown";
import ModuleMenuListItem from "../../molecules/MenuList/ModuleMenuListItem";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${(p) => p.theme.size.distance};

  .no-image {
    height: 100%;
  }
`;

const PanelHeader = styled(PanelHeading)`
  padding-bottom: 0;
`;

const ContentTopbar = styled.div`
  margin-bottom: ${(p) => p.theme.size.exSmall};
  position: relative;

  ${ContentItemDropdown} {
    z-index: 10;
  }

  ${ModuleMenuListItem} {
    margin-top: 0;
    margin-bottom: 0;
    border-bottom: 1px solid ${(p) => p.theme.color.secondaryDivide};
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

  svg {
    margin: 0 ${(p) => p.theme.size.exTiny};
  }
`;

const Description = styled.p`
  margin-bottom: 0;
`;

const InteractiveToolbar = styled.div`
  border-top: 1px solid ${(p) => p.theme.color.secondaryDivide};
  padding: ${(p) => p.theme.size.exSmall} ${(p) => p.theme.size.distance};
`;

const InteractiveItem = styled.li`
  margin-right: ${(p) => p.theme.size.small};
  :last-child {
    margin-right: 0;
  }
`;

export default withRouter(function (props) {
  const { product, location } = props;
  const { creator } = product;

  const loadCreatedInfo = () => {
    if (creator) {
      var datePeriod = convertDateTimeToPeriod(product.createdDate);
      creator.info = (
        <Fragment>
          <FontAwesomeIcon icon="calendar-alt" />
          {datePeriod}
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
            <div className="col col-3 col-sm-3 col-md-2 col-lg-2">
              <AuthorProfile profile={loadCreatedInfo()} />
            </div>
            <div className="col col-9 col-sm-9 col-md-10 col-lg-10">
              {product.price > 0 ? (
                <PriceLabel price={product.price} currency="vnđ" />
              ) : null}
            </div>
          </div>
        </ContentTopbar>
      </PanelHeader>
      <PanelBody>
        <div className="row">
          <div className="col col-9 col-sm-9 col-md-10 col-lg-10">
            <Title className="mb-3">
              <AnchorLink
                to={{
                  pathname: product.url,
                  state: { from: location.pathname },
                }}
              >
                {product.name}
              </AnchorLink>
            </Title>
            <div className="panel-content">
              <Description>
                <span
                  dangerouslySetInnerHTML={{ __html: product.description }}
                ></span>{" "}
              </Description>
            </div>
          </div>
          <div className="col col-3 col-sm-3 col-md-2 col-lg-2">
            <AnchorLink
              to={{
                pathname: product.url,
                state: { from: location.pathname },
              }}
            >
              <ImageThumb src={product.pictureUrl} alt="" />
            </AnchorLink>
          </div>
        </div>
      </PanelBody>
      <InteractiveToolbar>
        <HorizontalList>
          <InteractiveItem>
            <HorizontalReactBar reactionNumber={product.reactionNumber} />
          </InteractiveItem>
          <InteractiveItem>
            <FontButtonItem
              icon="comments"
              dynamicText={product.commentNumber}
              title="Discussions"
            />
          </InteractiveItem>
        </HorizontalList>
      </InteractiveToolbar>
    </Panel>
  );
});
