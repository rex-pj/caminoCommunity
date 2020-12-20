import React, { useState, useRef, useEffect } from "react";
import styled from "styled-components";
import { withRouter } from "react-router-dom";
import { PanelHeading, PanelDefault, PanelBody } from "../../atoms/Panels";
import ImageThumb from "../../molecules/Images/ImageThumb";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import AuthorProfile from "../ProfileCard/AuthorProfile";
import { ActionButton } from "../../molecules/ButtonGroups";
import { SecondaryTitleLink } from "../../atoms/Titles/TitleLinks";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { AnchorLink } from "../../atoms/Links";
import { HorizontalReactBar } from "../../molecules/Reaction";
import Overlay from "../../atoms/Overlay";
import { PriceLabel } from "../../molecules/PriceAndCurrency";
import ContentItemDropdown from "../../molecules/DropdownButton/ContentItemDropdown";
import ModuleMenuListItem from "../../molecules/MenuList/ModuleMenuListItem";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${(p) => p.theme.size.distance};

  .no-image {
    height: 140px;
  }
`;

const ContentBody = styled.div`
  padding: 0 0 ${(p) => p.theme.size.distance} 0;
  height: 160px;
`;

const ContentTopbar = styled.div`
  margin-bottom: ${(p) => p.theme.size.exSmall};
`;

const PostActions = styled.div`
  text-align: right;
`;

const PostTitle = styled(SecondaryTitleLink)`
  margin-bottom: 0;
  text-overflow: ellipsis;
  overflow: hidden;
  white-space: nowrap;
`;

const InteractiveItem = styled.li`
  margin-right: ${(p) => p.theme.size.small};
  :last-child {
    margin-right: 0;
  }
`;

const ThumbnailBox = styled.div`
  position: relative;

  img {
    border-top-left-radius: ${(p) => p.theme.borderRadius.normal};
    border-top-right-radius: ${(p) => p.theme.borderRadius.normal};
  }
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
  margin-bottom: ${(p) => p.theme.size.exTiny};
`;

const FarmInfo = styled(RowItem)`
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  font-size: ${(p) => p.theme.rgbaColor.small};

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

export default withRouter((props) => {
  const { product } = props;
  const { creator } = product;
  const [isActionDropdownShown, setActionDropdownShown] = useState(false);
  const currentRef = useRef();
  const onActionDropdownHide = (e) => {
    if (currentRef.current && !currentRef.current.contains(e.target)) {
      setActionDropdownShown(false);
    }
  };

  const onActionDropdownShow = () => {
    setActionDropdownShown(true);
  };

  const onEditMode = async () => {
    props.history.push({
      pathname: `/products/update/${product.id}`,
      state: {
        from: props.location.pathname,
      },
    });
  };

  useEffect(() => {
    document.addEventListener("click", onActionDropdownHide, false);
    return () => {
      document.removeEventListener("click", onActionDropdownHide);
    };
  });

  const loadCreatedInfo = () => {
    if (creator) {
      creator.info = "Farmer";
    }
    return creator;
  };

  let description = null;
  if (product.description && product.description.length >= 120) {
    description = `${product.description.substring(0, 120)}...`;
  }

  return (
    <Panel>
      <ThumbnailBox>
        <AnchorLink to={product.url}>
          <ImageThumb src={product.thumbnailUrl} alt="" />
          <Overlay />
        </AnchorLink>
      </ThumbnailBox>

      <PanelHeader>
        <ContentTopbar>
          <div className="row no-gutters">
            <div className="col col-8 col-sm-9 col-md-10 col-lg-11">
              <AuthorProfile profile={loadCreatedInfo()} />
            </div>

            <div className="col col-4 col-sm-3 col-md-2 col-lg-1">
              <PostActions ref={currentRef}>
                <ActionButton onClick={onActionDropdownShow}>
                  <FontAwesomeIcon icon="angle-down" />
                </ActionButton>
              </PostActions>
              {isActionDropdownShown ? (
                <ContentItemDropdown>
                  <ModuleMenuListItem>
                    <span onClick={onEditMode}>
                      <FontAwesomeIcon icon="pencil-alt"></FontAwesomeIcon> Edit
                    </span>
                  </ModuleMenuListItem>
                </ContentItemDropdown>
              ) : null}
            </div>
          </div>
        </ContentTopbar>
        <PostTitle>
          <AnchorLink to={product.url}>{product.name}</AnchorLink>
        </PostTitle>
      </PanelHeader>
      <PanelBody>
        <RowItem>
          {product.price > 0 ? (
            <PriceLabel price={product.price} currency="vnđ" />
          ) : null}
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

        <div className="panel-content">
          <ContentBody>
            <p dangerouslySetInnerHTML={{ __html: description }}></p>
          </ContentBody>
        </div>
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
});
