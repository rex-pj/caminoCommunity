import React from "react";
import styled from "styled-components";
import { PanelHeading, PanelDefault, PanelBody } from "../../atoms/Panels";
import { ThumbnailRound } from "../../molecules/Thumbnails";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ProfileAction from "../ProfileCard/ProfileAction";
import { ActionButton } from "../../molecules/ButtonGroups";
import { SecondaryTitle } from "../../atoms/Titles";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { AnchorLink } from "../../atoms/Links";
import { HorizontalReactBar } from "../../molecules/Reaction";
import Overlay from "../../atoms/Overlay";
import { PriceLabel } from "../../molecules/PriceAndCurrency";

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
  text-overflow: ellipsis;
  overflow: hidden;
  white-space: nowrap;
`;

const InteractiveItem = styled.li`
  margin-right: ${p => p.theme.size.small};
  :last-child {
    margin-right: 0;
  }
`;

const PostThumbnail = styled(ThumbnailRound)`
  border-bottom-left-radius: 0;
  border-bottom-right-radius: 0;
  max-height: 150px;
  overflow: hidden;

  img {
    border-bottom-left-radius: inherit;
    border-bottom-right-radius: inherit;
  }
`;

const ThumbnailBox = styled.div`
  position: relative;
`;

const PanelHeader = styled(PanelHeading)`
  padding-bottom: 0;
`;

const InteractItem = styled(InteractiveItem)`
  margin-right: 0;
`;

const InteractRightItem = styled(InteractiveItem)`
  text-align: right;
  float: right;
`;

const RowItem = styled.div`
  margin-bottom: ${p => p.theme.size.exTiny};
`;

const FarmInfo = styled(RowItem)`
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  font-size: ${p => p.theme.fontSize.exSmall};

  a {
    vertical-align: middle;
    font-weight: 600;
    color: ${p => p.theme.color.normal};
  }

  svg {
    margin-right: ${p => p.theme.size.exTiny};
    font-size: ${p => p.theme.fontSize.tiny};
    color: ${p => p.theme.color.normal};
    vertical-align: middle;
  }

  path {
    color: inherit;
  }
`;

export default function (props) {
  const { product } = props;
  const { creator } = product;

  if (creator) {
    creator.info = "Nông dân";
  }

  return (
    <Panel>
      <ThumbnailBox>
        <AnchorLink to={product.url}>
          <PostThumbnail src={product.thumbnailUrl} alt="" />
          <Overlay />
        </AnchorLink>
      </ThumbnailBox>
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
          <AnchorLink to={product.url}>{product.name}</AnchorLink>
        </PostTitle>
      </PanelHeader>
      <PanelBody>
        <RowItem>
          <PriceLabel price={product.price} currency="vnđ" />
        </RowItem>
        <FarmInfo>
          <FontAwesomeIcon icon="warehouse" />
          <AnchorLink to={product.farmUrl}>{product.farmName}</AnchorLink>
        </FarmInfo>
        <HorizontalList className="clearfix">
          <InteractItem>
            <HorizontalReactBar reactionNumber={product.reactionNumber} />
          </InteractItem>
          <InteractRightItem>
            <FontButtonItem
              icon="comments"
              dynamicText={product.commentNumber}
            />
          </InteractRightItem>
        </HorizontalList>
      </PanelBody>
    </Panel>
  );
};
