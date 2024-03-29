import * as React from "react";
import { Fragment, useRef, useEffect, useState, useContext } from "react";
import styled from "styled-components";
import { AnchorLink } from "../../atoms/Links";
import { PrimaryTitle } from "../../atoms/Titles";
import { HorizontalList } from "../../molecules/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { HorizontalReactBar } from "../../molecules/Reaction";
import { PanelBody, PanelDefault } from "../../molecules/Panels";
import { ActionButton } from "../../molecules/ButtonGroups";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ThumbnailSlider from "../../organisms/ThumbnailSlider";
import { PriceLabel } from "../../molecules/PriceAndCurrency";
import { TypographyDark } from "../../atoms/Typographies";
import { useLocation, useNavigate } from "react-router-dom";
import Dropdown from "../../molecules/DropdownButton/Dropdown";
import ModuleMenuListItem from "../../molecules/MenuList/ModuleMenuListItem";
import DeleteConfirmationModal from "../../organisms/Modals/DeleteConfirmationModal";
import { SessionContext } from "../../../store/context/session-context";
import AttributesDetail from "../../organisms/Product/AttributesDetail";
import { IBreadcrumbItem } from "../../organisms/Navigation/Breadcrumb";

const Title = styled(PrimaryTitle)`
  color: ${(p) => p.theme.color.darkText};
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
  color: ${(p) => p.theme.color.secondaryText};
  font-size: ${(p) => p.theme.fontSize.tiny};
  border-bottom: 1px solid ${(p) => p.theme.color.neutralBg};
  padding-bottom: ${(p) => p.theme.size.tiny};
  margin-bottom: ${(p) => p.theme.size.distance};
  position: relative;

  ${ModuleMenuListItem} {
    margin-top: 0;
    margin-bottom: 0;
    border-bottom: 1px solid ${(p) => p.theme.color.neutralBg};
  }

  ${ModuleMenuListItem}:last-child {
    border-bottom: 0;
  }
`;

const PostActions = styled.div`
  text-align: right;
  color: ${(p) => p.theme.color.secondaryText};

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
    color: ${(p) => p.theme.color.primaryText};
    margin-bottom: 0;
  }

  ${TypographyDark} {
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
    color: ${(p) => p.theme.color.primaryText};
  }

  svg {
    margin-right: ${(p) => p.theme.size.exTiny};
    font-size: ${(p) => p.theme.fontSize.tiny};
    color: ${(p) => p.theme.color.primaryText};
    vertical-align: middle;
  }

  path {
    color: inherit;
  }
`;

const DropdownList = styled(Dropdown)`
  position: absolute;
  right: 0;
  top: ${(p) => p.theme.size.normal};
  background: ${(p) => p.theme.color.whiteBg};
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  min-width: calc(${(p) => p.theme.size.large} * 3);
  border-radius: ${(p) => p.theme.borderRadius.normal};
  padding: ${(p) => p.theme.size.exTiny} 0;

  ${ModuleMenuListItem} {
    border-bottom: 1px solid ${(p) => p.theme.color.neutralBg};
  }

  ${ModuleMenuListItem}:last-child {
    border-bottom: 0;
  }

  ${ModuleMenuListItem} span {
    display: block;
    margin-bottom: 0;

    padding: ${(p) => p.theme.size.tiny} ${(p) => p.theme.size.tiny};
    cursor: pointer;
    text-align: left;

    :hover {
      background-color: ${(p) => p.theme.color.lightBg};
    }
  }
`;

type Props = {
  product?: any;
  breadcrumbs: IBreadcrumbItem[];
  onOpenDeleteConfirmationModal: (e: any) => void;
};

const Detail = (props: Props) => {
  const navigate = useNavigate();
  const location = useLocation();
  const { product, onOpenDeleteConfirmationModal } = props;
  const { currentUser, isLogin } = useContext(SessionContext);
  const { createdByIdentityId } = product;
  const isAuthor =
    currentUser && createdByIdentityId === currentUser.userIdentityId;
  const [isActionDropdownShown, setActionDropdownShown] = useState(false);
  const currentRef = useRef<any>();
  const onActionDropdownHide = (e: MouseEvent) => {
    if (currentRef.current && !currentRef.current.contains(e.target)) {
      setActionDropdownShown(false);
    }
  };

  const onActionDropdownShow = () => {
    setActionDropdownShown(true);
  };

  const onEditMode = async () => {
    navigate(`/products/update/${product.id}`, {
      state: {
        from: location.pathname,
      },
    });
  };

  const onOpenDeleteConfirmation = () => {
    onOpenDeleteConfirmationModal({
      title: "Delete Farm",
      innerModal: DeleteConfirmationModal,
      message: `Are you sure to delete product "${product.name}"?`,
      id: parseFloat(product.id),
    });
  };

  useEffect(() => {
    document.addEventListener("click", onActionDropdownHide, false);
    return () => {
      document.removeEventListener("click", onActionDropdownHide);
    };
  });

  const { productAttributes } = product;

  return (
    <Fragment>
      <PanelDefault>
        {product.images ? (
          <ThumbnailSlider
            currentImage={product.images[0]}
            images={product.images}
            displayNumber={5}
          />
        ) : null}

        <PanelBody>
          <div className="clearfix">
            <TopBarInfo>
              <div className="row">
                <div className="col col-8 col-sm-9 col-md-10 col-lg-11">
                  <Title>{product.title}</Title>
                </div>
                <div className="col col-4 col-sm-3 col-md-2 col-lg-1">
                  {isLogin ? (
                    <PostActions ref={currentRef}>
                      <ActionButton
                        className="dropdown-action"
                        onClick={onActionDropdownShow}
                      >
                        <FontAwesomeIcon icon="angle-down" />
                      </ActionButton>
                    </PostActions>
                  ) : null}

                  {isActionDropdownShown && isAuthor ? (
                    <DropdownList>
                      <ModuleMenuListItem>
                        <span onClick={onEditMode}>
                          <FontAwesomeIcon icon="pencil-alt"></FontAwesomeIcon>{" "}
                          Edit
                        </span>
                      </ModuleMenuListItem>
                      <ModuleMenuListItem>
                        <span onClick={onOpenDeleteConfirmation}>
                          <FontAwesomeIcon
                            icon="trash-alt"
                            className="me-2"
                          ></FontAwesomeIcon>
                          Delete
                        </span>
                      </ModuleMenuListItem>
                    </DropdownList>
                  ) : null}
                </div>
              </div>
            </TopBarInfo>
            <RowItem>
              <label>Giá:</label>
              <LabelPrice price={product.price} currency="vnđ" />
            </RowItem>
            {product.farms
              ? product.farms.map((pf: any) => {
                  if (!pf.id) {
                    return null;
                  }

                  return (
                    <FarmInfo key={pf.id}>
                      <FontAwesomeIcon icon="warehouse" />
                      <AnchorLink to={pf.url}>{pf.name}</AnchorLink>
                    </FarmInfo>
                  );
                })
              : null}

            <AttributesDetail
              price={product.price}
              productAttributes={productAttributes}
            ></AttributesDetail>
            <ContentBody>
              <span
                dangerouslySetInnerHTML={{ __html: product.description }}
              ></span>
            </ContentBody>

            {/* <div className="interactive-toolbar">
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
            </div> */}
          </div>
        </PanelBody>
      </PanelDefault>
    </Fragment>
  );
};

export default Detail;
