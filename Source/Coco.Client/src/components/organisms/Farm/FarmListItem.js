import React from "react";
import ProfileAction from "../ProfileCard/ProfileAction";
import { HorizontalReactBar } from "../../molecules/Reaction";
import styled from "styled-components";
import { ActionButton } from "../../molecules/ButtonGroups";
import { PanelHeading, PanelDefault, PanelBody } from "../../atoms/Panels";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { ImageRound } from "../../atoms/Images";
import { SecondaryTitleLink } from "../../atoms/Titles/TitleLinks";
import { AnchorLink } from "../../atoms/Links";
import { HorizontalList } from "../../atoms/List";
import { FontButtonItem } from "../../molecules/ActionIcons";
import { ButtonIconOutlineSecondary } from "../../molecules/ButtonIcons";
import Overlay from "../../atoms/Overlay";

const Panel = styled(PanelDefault)`
  position: relative;
  margin-bottom: ${p => p.theme.size.distance};
`;

const PanelHeader = styled(PanelHeading)`
  padding-bottom: 0;
`;

const ContentTopbar = styled.div`
  margin-bottom: 0;
`;

const Title = styled(SecondaryTitleLink)`
  margin-bottom: 0;
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

const Cover = styled.div`
  position: relative;
  max-height: 250px;
  overflow: hidden;
`;

const CoverImage = styled(ImageRound)`
  border-bottom-left-radius: 0;
  border-bottom-right-radius: 0;
`;

const FollowButton = styled(ButtonIconOutlineSecondary)`
  padding: ${p => p.theme.size.tiny};
  font-size: ${p => p.theme.rgbaColor.small};
  line-height: 1;

  position: absolute;
  bottom: ${p => p.theme.size.distance};
  right: ${p => p.theme.size.distance};
  z-index: 1;
`;

const ProfileBox = styled(ProfileAction)`
  position: absolute;
  bottom: ${p => p.theme.size.distance};
  left: ${p => p.theme.size.distance};
  z-index: 1;

  a {
    color: ${p => p.theme.color.light};
  }
`;

const TopBarInfo = styled.div`
  color: ${p => p.theme.color.neutral};
  font-size: ${p => p.theme.fontSize.tiny};

  span {
    color: inherit;
    vertical-align: middle;
  }

  svg {
    margin-right: ${p => p.theme.size.exTiny};
    color: inherit;
    vertical-align: middle;
  }

  path {
    color: inherit;
  }
`;

export default props => {
  const { farm } = props;
  const { creator } = farm;

  return (
    <Panel>
      <Cover>
        <AnchorLink to={farm.url}>
          <CoverImage src={farm.thumbnailUrl} alt="" />
          <Overlay />
        </AnchorLink>

        <ProfileBox profile={creator} />
        <FollowButton icon="user-plus" size="sm">
          Theo dõi
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
                <FontAwesomeIcon icon="map-marker-alt" />
                <span>{farm.address}</span>
              </TopBarInfo>
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
      </PanelHeader>
      <PanelBody>
        <div className="panel-content">
          <Description>
            {farm.description} <AnchorLink to={farm.url}>Xem Thêm</AnchorLink>
          </Description>
        </div>
        <InteractiveToolbar>
          <HorizontalList>
            <InteractiveItem>
              <HorizontalReactBar reactionNumber={farm.reactionNumber} />
            </InteractiveItem>

            <InteractiveItem>
              <FontButtonItem
                icon="comments"
                title="Thảo luận"
                dynamicText={farm.commentNumber}
              />
            </InteractiveItem>
          </HorizontalList>
        </InteractiveToolbar>
      </PanelBody>
    </Panel>
  );
};
