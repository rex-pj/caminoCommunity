import React, { Fragment } from "react";
import styled from "styled-components";
import { AnchorLink } from "../../atoms/Links";
import { PrimaryTitle } from "../../atoms/Titles";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { HorizontalReactBar } from "../../molecules/Reaction";
import { PanelBody, PanelDefault } from "../../atoms/Panels";
import { ActionButton } from "../../molecules/ButtonGroups";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ThumbnailSlider from "../../organisms/ThumbnailSlider";
import { PriceLabel } from "../../molecules/PriceAndCurrency";
import { TypographyPrimary } from "../../atoms/Typographies";
import { withRouter } from "react-router-dom";

const Title = styled(PrimaryTitle)`
  color: ${(p) => p.theme.color.primary};
`;

const ContentBody = styled.div`
  padding: ${(p) => p.theme.size.distance} 0 ${(p) => p.theme.size.distance} 0;
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
`;

const PostActions = styled.div`
  text-align: right;
  color: ${(p) => p.theme.color.neutral};

  button {
    vertical-align: middle;
  }
`;

const RowItem = styled.div`
  margin-bottom: ${(p) => p.theme.size.exTiny};

  label {
    font-weight: 600;
    display: inline-block;
    margin-right: ${(p) => p.theme.size.exTiny};
    font-size: ${(p) => p.theme.fontSize.small};
    color: ${(p) => p.theme.color.neutral};
    margin-bottom: 0;
  }

  ${TypographyPrimary} {
    font-size: ${(p) => p.theme.fontSize.small};
    display: inline-block;
    margin-bottom: 0;
    font-weight: 600;
  }
`;

const LabelPrice = styled(PriceLabel)`
  display: inline-block;
`;

const FarmInfo = styled.div`
  font-size: ${(p) => p.theme.fontSize.small};

  a {
    vertical-align: middle;
    font-weight: 600;
    color: ${(p) => p.theme.color.neutral};
  }

  svg {
    margin-right: ${(p) => p.theme.size.exTiny};
    font-size: ${(p) => p.theme.fontSize.tiny};
    color: ${(p) => p.theme.color.neutral};
    vertical-align: middle;
  }

  path {
    color: inherit;
  }
`;

export default withRouter(function (props) {
  const { product } = props;
  return (
    <Fragment>
      <PanelDefault>
        {product.images ? (
          <ThumbnailSlider
            currentImage={product.images[0]}
            images={product.images}
            numberOfDisplay={5}
          />
        ) : null}

        <PanelBody>
          <div className="clearfix">
            <TopBarInfo>
              <Title>{product.title}</Title>
            </TopBarInfo>
            <RowItem>
              <label>Giá:</label>
              <LabelPrice price={product.price} currency="vnđ" />
            </RowItem>
            {product.productFarms
              ? product.productFarms.map((pf) => {
                  if (!pf.id) {
                    return null;
                  }

                  return (
                    <FarmInfo key={pf.id}>
                      <FontAwesomeIcon icon="warehouse" />
                      <AnchorLink to={pf.url}>{pf.farmName}</AnchorLink>
                    </FarmInfo>
                  );
                })
              : null}

            <ContentBody>
              <span
                dangerouslySetInnerHTML={{ __html: product.description }}
              ></span>
            </ContentBody>

            <div className="interactive-toolbar">
              <div className="row">
                <div className="col col-8 col-sm-9 col-md-10 col-lg-11">
                  <HorizontalList>
                    <InteractiveItem>
                      <HorizontalReactBar
                        reactionNumber={product.reactionNumber}
                      />
                    </InteractiveItem>
                    <InteractiveItem>
                      <FontButtonItem
                        icon="comments"
                        title="Discussions"
                        dynamicText={product.commentNumber}
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
    </Fragment>
  );
});
