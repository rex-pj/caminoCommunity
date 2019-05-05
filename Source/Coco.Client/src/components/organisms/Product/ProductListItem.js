import React from "react";
import ProfileAction from "../ProfileCard/ProfileAction";
import { HorizontalReactBar } from "../../molecules/Reaction";
import styled from "styled-components";
import { ActionButton } from "../../molecules/ButtonGroups";
import { PanelHeading, PanelDefault, PanelBody } from "../../atoms/Panels";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { ImageRound } from "../../atoms/Images";
import { SecondaryTitle } from "../../atoms/Titles";
import { AnchorLink } from "../../atoms/Links";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { PriceLabel } from "../../molecules/PriceAndCurrency";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${p => p.theme.size.distance};
`;

const PanelHeader = styled(PanelHeading)`
  padding-bottom: 0;
`;

const ContentTopbar = styled.div`
  margin-bottom: ${p => p.theme.size.exSmall};
`;

const Title = styled(SecondaryTitle)`
  margin-bottom: 0;

  svg {
    margin: 0 ${p => p.theme.size.exTiny};
  }
`;

const PostActions = styled.div`
  text-align: right;
`;

const Description = styled.p`
  margin-bottom: 0;
`;

const InteractiveToolbar = styled.div`
  margin-top: ${p => p.theme.size.distance};
`;

const InteractiveItem = styled.li`
  margin-right: ${p => p.theme.size.small};
  :last-child {
    margin-right: 0;
  }
`;

export default function(props) {
  const { product } = props;
  const { creator } = product;

  if (creator) {
    creator.info = `Gửi bài lúc ${product.createdDate}`;
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
          <AnchorLink to={product.farmUrl}>{product.farmName}</AnchorLink>
          <FontAwesomeIcon icon="angle-right" />
          <AnchorLink to={product.url}>{product.name}</AnchorLink>
        </Title>
      </PanelHeader>
      <PanelBody>
        <div className="row">
          <div className="col col-6 col-sm-6 col-md-5 col-lg-5">
            <AnchorLink to={product.url}>
              <ImageRound src={product.thumbnailUrl} alt="" />
            </AnchorLink>
          </div>

          <div className="col col-6 col-sm-6 col-md-7 col-lg-7">
            <PriceLabel price={product.price} currency="vnđ" />
            <div className="panel-content">
              <Description>
                {product.description}{" "}
                <AnchorLink to={product.url}>Xem Thêm</AnchorLink>
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
