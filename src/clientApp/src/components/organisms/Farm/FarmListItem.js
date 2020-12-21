import React, { Fragment, useRef, useState, useEffect } from "react";
import AuthorProfile from "../ProfileCard/AuthorProfile";
import { HorizontalReactBar } from "../../molecules/Reaction";
import styled from "styled-components";
import { ActionButton } from "../../molecules/ButtonGroups";
import { PanelHeading, PanelDefault, PanelBody } from "../../atoms/Panels";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import ImageThumb from "../../molecules/Images/ImageThumb";
import { secondaryTitleLink } from "../../atoms/Titles/TitleLinks";
import { AnchorLink } from "../../atoms/Links";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { ButtonIconPrimary } from "../../molecules/ButtonIcons";
import Overlay from "../../atoms/Overlay";
import { convertDateTimeToPeriod } from "../../../utils/DateTimeUtils";
import ContentItemDropdown from "../../molecules/DropdownButton/ContentItemDropdown";
import ModuleMenuListItem from "../../molecules/MenuList/ModuleMenuListItem";
import { withRouter } from "react-router-dom";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${(p) => p.theme.size.distance};

  .no-image {
    height: 200px;
    border-radius: 0;
  }

  img.thumbnail-img {
    width: 100%;
    border-top-left-radius: ${(p) => p.theme.borderRadius.normal};
    border-top-right-radius: ${(p) => p.theme.borderRadius.normal};
  }
`;

const PanelHeader = styled(PanelHeading)`
  padding-bottom: 0;
`;

const ContentTopbar = styled.div`
  margin-bottom: 0;
`;

const Title = styled(secondaryTitleLink)`
  margin-bottom: 0;
`;

const PostActions = styled.div`
  text-align: right;
  position: relative;
`;

const Description = styled.p`
  margin-bottom: 0;
`;

const InteractiveToolbar = styled.div`
  border-top: 1px solid ${(p) => p.theme.color.secondaryDivide};
  padding: ${(p) => p.theme.size.exSmall} ${(p) => p.theme.size.distance};
`;

const InteractiveItem = styled.li`
  margin-right: ${(p) => p.theme.size.small};
  :last-child {
    margin-right: 0;
  }
`;

const Cover = styled.div`
  position: relative;
  max-height: 250px;
  overflow: hidden;
`;

const FollowButton = styled(ButtonIconPrimary)`
  padding: ${(p) => p.theme.size.tiny};
  font-size: ${(p) => p.theme.fontSize.small};
  line-height: 1;

  position: absolute;
  bottom: ${(p) => p.theme.size.distance};
  right: ${(p) => p.theme.size.distance};
  z-index: 1;
`;

const ProfileBox = styled(AuthorProfile)`
  position: absolute;
  bottom: ${(p) => p.theme.size.distance};
  left: ${(p) => p.theme.size.distance};
  z-index: 1;

  a {
    color: ${(p) => p.theme.color.lightText};
  }
`;

const TopBarInfo = styled.div`
  color: ${(p) => p.theme.color.neutralText};
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
    const { creator } = farm;
    if (creator) {
      creator.info = "Farmer";
    }
    return creator;
  };

  return (
    <Panel>
      <Cover>
        <AnchorLink to={farm.url}>
          <ImageThumb src={farm.thumbnailUrl} alt="" />
          <Overlay />
        </AnchorLink>

        <ProfileBox profile={loadCreatedInfo()} />
        <FollowButton icon="user-plus" size="sm">
          Follow
        </FollowButton>
      </Cover>

      <PanelHeader>
        <ContentTopbar>
          <div className="row no-gutters">
            <div className="col col-8 col-sm-9 col-md-10 col-lg-11">
              <Title>
                <AnchorLink to={farm.url}>{farm.name}</AnchorLink>
              </Title>

              <TopBarInfo>
                <span className="mr-3">
                  <FontAwesomeIcon icon="calendar-alt" />
                  <span>{convertDateTimeToPeriod(farm.createdDate)}</span>
                </span>
                {farm.address ? (
                  <Fragment>
                    <FontAwesomeIcon icon="map-marker-alt" />
                    <span>{farm.address}</span>
                  </Fragment>
                ) : null}
              </TopBarInfo>
            </div>

            <div className="col col-4 col-sm-3 col-md-2 col-lg-1">
              <PostActions ref={currentRef}>
                <ActionButton onClick={onActionDropdownShow}>
                  <FontAwesomeIcon icon="angle-down" />
                </ActionButton>
                {isActionDropdownShown ? (
                  <ContentItemDropdown>
                    <ModuleMenuListItem>
                      <span onClick={onEditMode}>
                        <FontAwesomeIcon icon="pencil-alt"></FontAwesomeIcon>{" "}
                        Edit
                      </span>
                    </ModuleMenuListItem>
                  </ContentItemDropdown>
                ) : null}
              </PostActions>
            </div>
          </div>
        </ContentTopbar>
      </PanelHeader>
      <PanelBody>
        <div className="panel-content">
          <Description>
            <span dangerouslySetInnerHTML={{ __html: farm.description }}></span>{" "}
          </Description>
        </div>
      </PanelBody>
      <InteractiveToolbar>
        <HorizontalList>
          <InteractiveItem>
            <HorizontalReactBar reactionNumber={farm.reactionNumber} />
          </InteractiveItem>

          <InteractiveItem>
            <FontButtonItem
              icon="comments"
              title="Discussions"
              dynamicText={farm.commentNumber}
            />
          </InteractiveItem>
        </HorizontalList>
      </InteractiveToolbar>
    </Panel>
  );
});
