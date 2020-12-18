import React, { Fragment, useState, useRef, useEffect } from "react";
import AuthorProfile from "../ProfileCard/AuthorProfile";
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
import Dropdown from "../../molecules/DropdownButton/Dropdown";
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

const DropdownList = styled(Dropdown)`
  position: absolute;
  right: 0;
  top: calc(100% + ${(p) => p.theme.size.exTiny});
  background: ${(p) => p.theme.color.white};
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  min-width: calc(${(p) => p.theme.size.large} * 3);
  border-radius: ${(p) => p.theme.borderRadius.normal};
  padding: ${(p) => p.theme.size.exTiny} 0;

  ${ModuleMenuListItem} span {
    display: block;
    margin-bottom: 0;
    border-bottom: 1px solid ${(p) => p.theme.color.lighter};
    padding: ${(p) => p.theme.size.exTiny} ${(p) => p.theme.size.tiny};
    cursor: pointer;
    text-align: left;

    :hover {
      background-color: ${(p) => p.theme.color.lighter};
    }

    :last-child {
      border-bottom: 0;
    }
  }
`;

export default function (props) {
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

  if (creator) {
    var datePeriod = convertDateTimeToPeriod(product.createdDate);
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
                <ActionButton onClick={onActionDropdownShow}>
                  <FontAwesomeIcon icon="angle-down" />
                </ActionButton>
                {isActionDropdownShown ? (
                  <DropdownList>
                    <ModuleMenuListItem>
                      <span onClick={onEditMode}>
                        <FontAwesomeIcon icon="pencil-alt"></FontAwesomeIcon>{" "}
                        Edit
                      </span>
                    </ModuleMenuListItem>
                  </DropdownList>
                ) : null}
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
                title="Discussions"
              />
            </InteractiveItem>
          </HorizontalList>
        </InteractiveToolbar>
      </PanelBody>
    </Panel>
  );
}
