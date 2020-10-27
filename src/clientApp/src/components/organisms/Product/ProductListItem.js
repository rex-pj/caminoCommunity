import React, { Fragment } from "react";
import ProfileAction from "../ProfileCard/ProfileAction";
import { HorizontalReactBar } from "../../molecules/Reaction";
import styled from "styled-components";
import { ActionButton } from "../../molecules/ButtonGroups";
import { PanelHeading, PanelDefault, PanelBody } from "../../atoms/Panels";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ImageThumb from "../../molecules/Images/ImageThumb";
import { SecondaryTitleLink } from "../../atoms/Titles/TitleLinks";
import { AnchorLink } from "../../atoms/Links";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { PriceLabel } from "../../molecules/PriceAndCurrency";
import { convertDateTimeToPeriod } from "../../../utils/DateTimeUtils";

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
`;

const Title = styled(SecondaryTitleLink)`
  margin-bottom: 0;

  svg {
    margin: 0 ${(p) => p.theme.size.exTiny};
  }
`;

const PostActions = styled.div`
  text-align: right;
`;

const Description = styled.p`
  margin-bottom: 0;
`;

const InteractiveToolbar = styled.div`
  margin-top: ${(p) => p.theme.size.distance};
`;

const InteractiveItem = styled.li`
  margin-right: ${(p) => p.theme.size.small};
  :last-child {
    margin-right: 0;
  }
`;

export default function (props) {
  const { product } = props;
  const { creator } = product;

  if (creator) {
    var datePeriod = convertDateTimeToPeriod(product.createdDate);
    creator.info = `${datePeriod}`;
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

        <Title>
          {product.productFarms
            ? product.productFarms.map((pf) => {
                if (!pf.id) {
                  return null;
                }

                return (
                  <Fragment>
                    <AnchorLink to={pf.url}>{pf.farmName}</AnchorLink>
                    <FontAwesomeIcon icon="angle-right" />
                  </Fragment>
                );
              })
            : null}

          <AnchorLink to={product.url}>{product.name}</AnchorLink>
        </Title>
      </PanelHeader>
      <PanelBody>
        <div className="row">
          <div className="col col-6 col-sm-6 col-md-5 col-lg-5">
            <AnchorLink to={product.url}>
              <ImageThumb src={product.thumbnailUrl} alt="" />
            </AnchorLink>
          </div>

          <div className="col col-6 col-sm-6 col-md-7 col-lg-7">
            {product.price > 0 ? (
              <PriceLabel price={product.price} currency="vnđ" />
            ) : null}

            <div className="panel-content">
              <Description>
                <span
                  dangerouslySetInnerHTML={{ __html: product.description }}
                ></span>{" "}
                <AnchorLink to={product.url}>Detail</AnchorLink>
              </Description>
            </div>
          </div>
        </div>
        <InteractiveToolbar>
          <HorizontalList>
            <InteractiveItem>
              <HorizontalReactBar reactionNumber={product.reactionNumber} />
            </InteractiveItem>
            <InteractiveItem>
              <FontButtonItem
                icon="comments"
                dynamicText={product.commentNumber}
                title="Thảo luận"
              />
            </InteractiveItem>
          </HorizontalList>
        </InteractiveToolbar>
      </PanelBody>
    </Panel>
  );
}
