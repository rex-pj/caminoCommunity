import React, { useEffect, Fragment } from "react";
import styled from "styled-components";
import { PrimaryTitle } from "../../atoms/Titles";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { HorizontalReactBar } from "../../molecules/Reaction";
import { PanelBody, PanelDefault } from "../../atoms/Panels";
import { ActionButton } from "../../molecules/ButtonGroups";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Breadcrumb from "../../organisms/Navigation/Breadcrumb";
import { ButtonIconOutline } from "../../molecules/ButtonIcons";
import ProductItem from "../../organisms/Product/ProductItem";
import { TertiaryHeading } from "../../atoms/Heading";
import ThumbnailSlider from "../../organisms/ThumbnailSlider";

const Title = styled(PrimaryTitle)`
  margin-bottom: ${(p) => p.theme.size.exTiny};
  color: ${(p) => p.theme.color.primary};
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

const TopBarInfo = styled.div`
  color: ${(p) => p.theme.color.light};
  font-size: ${(p) => p.theme.fontSize.tiny};
  margin-bottom: ${(p) => p.theme.size.distance};
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

const PostActions = styled.div`
  text-align: right;
  color: ${(p) => p.theme.color.neutral};

  button {
    vertical-align: middle;
  }
`;

const BreadCrumbNav = styled(Breadcrumb)`
  border: 0;
  border-bottom: 1px solid ${(p) => p.theme.color.lighter};
  border-radius: 0;
  margin-bottom: 0;
`;

const FollowButton = styled(ButtonIconOutline)`
  padding: ${(p) => p.theme.size.tiny};
  font-size: ${(p) => p.theme.rgbaColor.small};
  line-height: 1;

  position: absolute;
  top: 0;
  right: 0;
  z-index: 1;
`;

const FarmProductsBox = styled.div`
  margin-top: ${(p) => p.theme.size.distance};
`;

export default (props) => {
  const { fetchFarmProducts } = props;
  useEffect(() => {
    fetchFarmProducts();
  }, [fetchFarmProducts]);

  const { farm, breadcrumbs, farmProducts } = props;
  return (
    <Fragment>
      <PanelDefault>
        <ThumbnailSlider images={farm.images} numberOfDisplay={5} />
        <BreadCrumbNav list={breadcrumbs} />
        <PanelBody>
          <TopBarInfo>
            <Title>{farm.title}</Title>
            <FontAwesomeIcon icon="map-marker-alt" />
            <span>{farm.address}</span>

            <FollowButton icon="user-plus" size="sm">
              Follow
            </FollowButton>
          </TopBarInfo>
          <div className="clearfix">
            <ContentBody>{farm.content}</ContentBody>

            <div className="interactive-toolbar">
              <div className="row">
                <div className="col col-8 col-sm-9 col-md-10 col-lg-11">
                  <HorizontalList>
                    <InteractiveItem>
                      <HorizontalReactBar
                        reactionNumber={farm.reactionNumber}
                      />
                    </InteractiveItem>
                    <InteractiveItem>
                      <FontButtonItem
                        icon="comments"
                        title="Discussions"
                        dynamicText={farm.commentNumber}
                      />
                    </InteractiveItem>
                    <InteractiveItem>
                      <FontButtonItem icon="bookmark" title="Đánh dấu" />
                    </InteractiveItem>
                  </HorizontalList>
                </div>
                <div className="col col-4 col-sm-3 col-md-2 col-lg-1">
                  <PostActions>
                    <ActionButton>
                      <FontAwesomeIcon icon="angle-down" />
                    </ActionButton>
                  </PostActions>
                </div>
              </div>
            </div>
          </div>
        </PanelBody>
      </PanelDefault>
      <FarmProductsBox>
        <TertiaryHeading>{farm.title}'s products</TertiaryHeading>
        <div className="row">
          {farmProducts
            ? farmProducts.map((item, index) => {
                return (
                  <div
                    key={index}
                    className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
                  >
                    <ProductItem product={item} />
                  </div>
                );
              })
            : null}
        </div>
      </FarmProductsBox>
    </Fragment>
  );
};
