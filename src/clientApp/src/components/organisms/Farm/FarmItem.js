import React, { Fragment, useState, useRef, useEffect } from "react";
import styled from "styled-components";
import { withRouter } from "react-router-dom";
import { PanelHeading, PanelDefault, PanelBody } from "../../atoms/Panels";
import ImageThumb from "../../molecules/Images/ImageThumb";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import AuthorProfile from "../ProfileCard/AuthorProfile";
import { ActionButton } from "../../molecules/ButtonGroups";
import { SecondaryTitleLink } from "../../atoms/Titles/TitleLinks";
import { ButtonIconOutlineSecondary } from "../../molecules/ButtonIcons";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { AnchorLink } from "../../atoms/Links";
import { HorizontalReactBar } from "../../molecules/Reaction";
import Overlay from "../../atoms/Overlay";
import ContentItemDropdown from "../../molecules/DropdownButton/ContentItemDropdown";
import ModuleMenuListItem from "../../molecules/MenuList/ModuleMenuListItem";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${(p) => p.theme.size.distance};

  .no-image {
    height: 140px;
  }
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

const FollowButton = styled(ButtonIconOutlineSecondary)`
  padding: ${(p) => p.theme.size.tiny};
  font-size: ${(p) => p.theme.rgbaColor.small};
  line-height: 1;

  position: absolute;
  bottom: ${(p) => p.theme.size.tiny};
  right: ${(p) => p.theme.size.tiny};
`;

const TopBarInfo = styled.div`
  color: ${(p) => p.theme.color.neutral};
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

export default withRouter((props) => {
  const { farm } = props;
  const { creator } = farm;
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
      pathname: `/farms/update/${farm.id}`,
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

  return (
    <Panel>
      <ThumbnailBox>
        <ImageThumb src={farm.thumbnailUrl} alt="" />
        <Overlay />
        <FollowButton icon="user-plus" size="sm">
          Follow
        </FollowButton>
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
          <AnchorLink to={farm.url}>{farm.name}</AnchorLink>
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

        <div className="interactive-toolbar">
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
        </div>
      </PanelBody>
    </Panel>
  );
});
