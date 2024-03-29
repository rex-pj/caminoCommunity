import * as React from "react";
import { Fragment, useState, useRef, useEffect, useContext } from "react";
import styled from "styled-components";
import { useLocation, useNavigate } from "react-router-dom";
import { PanelHeading, PanelDefault, PanelBody } from "../../molecules/Panels";
import ImageThumb from "../../molecules/Images/ImageThumb";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import AuthorProfile from "../ProfileCard/AuthorProfile";
import { ActionButton } from "../../molecules/ButtonGroups";
import { secondaryTitleLink } from "../../atoms/Titles/TitleLinks";
import { ButtonIconPrimary } from "../../molecules/ButtonIcons";
import { HorizontalList } from "../../molecules/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { AnchorLink } from "../../atoms/Links";
import { HorizontalReactBar } from "../../molecules/Reaction";
import Overlay from "../../atoms/Overlay";
import ContentItemDropdown from "../../molecules/DropdownButton/ContentItemDropdown";
import ModuleMenuListItem from "../../molecules/MenuList/ModuleMenuListItem";
import { SessionContext } from "../../../store/context/session-context";
import DeleteConfirmationModal from "../Modals/DeleteConfirmationModal";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${(p) => p.theme.size.distance};

  .no-image {
    height: 140px;
  }
`;

const ContentTopbar = styled.div`
  margin-bottom: ${(p) => p.theme.size.exSmall};
  position: relative;

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

const PostActions = styled.div`
  text-align: right;
`;

const PostTitle = styled(secondaryTitleLink)`
  margin-bottom: 0;
  text-overflow: ellipsis;
  overflow: hidden;
  white-space: nowrap;
`;

const ContentBody = styled.div`
  padding: 0 0 ${(p) => p.theme.size.distance} 0;
  height: 160px;
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

const FollowButton = styled(ButtonIconPrimary)`
  padding: ${(p) => p.theme.size.tiny};
  font-size: ${(p) => p.theme.fontSize.small};
  line-height: 1;

  position: absolute;
  bottom: ${(p) => p.theme.size.tiny};
  right: ${(p) => p.theme.size.tiny};
`;

const TopBarInfo = styled.div`
  color: ${(p) => p.theme.color.secondaryText};
  font-size: ${(p) => p.theme.fontSize.tiny};

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
  onOpenDeleteConfirmationModal: (e: any) => void;
}

const FarmItem = (props: Props) => {
  const location = useLocation();
  const navigate = useNavigate();
  const { farm, onOpenDeleteConfirmationModal } = props;
  const { creator, createdByIdentityId } = farm;
  const { currentUser, isLogin } = useContext(SessionContext);
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
    navigate(`/farms/update/${farm.id}`, {
      state: {
        from: location.pathname,
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

  const onOpenDeleteConfirmation = () => {
    onOpenDeleteConfirmationModal({
      title: "Delete Farm",
      innerModal: DeleteConfirmationModal,
      message: `Are you sure to delete farm "${farm.name}"?`,
      id: parseFloat(farm.id),
    });
  };

  return (
    <Panel>
      <ThumbnailBox>
        <ImageThumb src={farm.pictureUrl} alt="" />
        <Overlay />
        <FollowButton icon="user-plus" size="sm">
          Follow
        </FollowButton>
      </ThumbnailBox>
      <PanelHeader>
        <ContentTopbar>
          <div className="row g-0">
            <div className="col col-8 col-sm-9 col-md-10 col-lg-11">
              <AuthorProfile profile={loadCreatedInfo()} />
            </div>

            <div className="col col-4 col-sm-3 col-md-2 col-lg-1">
              {isLogin ? (
                <PostActions ref={currentRef}>
                  <ActionButton onClick={onActionDropdownShow}>
                    <FontAwesomeIcon icon="angle-down" />
                  </ActionButton>
                </PostActions>
              ) : null}

              {isActionDropdownShown && isAuthor ? (
                <ContentItemDropdown>
                  <ModuleMenuListItem>
                    <span onClick={onEditMode}>
                      <FontAwesomeIcon
                        icon="pencil-alt"
                        className="me-2"
                      ></FontAwesomeIcon>
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
                </ContentItemDropdown>
              ) : null}
            </div>
          </div>
        </ContentTopbar>
        <PostTitle>
          <AnchorLink
            to={{
              pathname: farm.url,
              state: { from: location.pathname },
            }}
          >
            {farm.name}
          </AnchorLink>
        </PostTitle>
        <TopBarInfo>
          {farm.address ? (
            <Fragment>
              <FontAwesomeIcon icon="map-marker-alt" />
              <span>{farm.address}</span>
            </Fragment>
          ) : null}
        </TopBarInfo>
      </PanelHeader>
      <PanelBody>
        <div className="panel-content">
          <ContentBody>
            <p dangerouslySetInnerHTML={{ __html: farm.description }}></p>
          </ContentBody>
        </div>

        {/* <div className="interactive-toolbar">
          <HorizontalList className="clearfix">
            <InteractItem>
              <HorizontalReactBar reactionNumber={farm.reactionNumber} />
            </InteractItem>
            <InteractRightItem>
              <FontButtonItem
                icon="comments"
                dynamicText={farm.commentNumber}
              />
            </InteractRightItem>
          </HorizontalList>
        </div> */}
      </PanelBody>
    </Panel>
  );
};

export default FarmItem;
