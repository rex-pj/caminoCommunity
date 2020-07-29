import React from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Thumbnail } from "../../molecules/Thumbnails";
import { ButtonOutlineDark } from "../../atoms/Buttons/OutlineButtons";
import { FifthDarkHeading } from "../../atoms/Heading";
import { TypographySecondary } from "../../atoms/Typographies";
import { AnchorLink } from "../../atoms/Links";
import Overlay from "../../atoms/Overlay";

const ListGroupItem = styled.li`
  padding-bottom: ${p => p.theme.size.tiny};

  &.first-item .thumbnail > img {
    border-top-left-radius: ${p => p.theme.borderRadius.normal};
    border-top-right-radius: ${p => p.theme.borderRadius.normal};
  }
`;

const Cover = styled.div`
  position: relative;

  .thumbnail {
    max-height: 120px;
    overflow: hidden;
    border-radius: 0;

    img {
      border-radius: 0;
    }
  }
`;

const InfoRow = styled.span`
  position: absolute;
  bottom: 8px;
  left: 15px;
  color: ${p => p.theme.color.lighter};
  z-index: 2;
  font-size: 13px;
  right: 5px;

  svg,
  path {
    color: inherit;
  }
`;

const ActionButton = styled(ButtonOutlineDark)`
  font-weight: 400;
  font-size: ${p => p.theme.fontSize.small};
  position: absolute;
  top: ${p => p.theme.size.distance};
  right: ${p => p.theme.size.distance};
  padding: ${p => p.theme.size.exTiny};
  z-index: 1;

  svg {
    margin-right: 3px;
  }
`;

const Title = styled(FifthDarkHeading)`
  color: ${p => p.theme.color.dark};
  a {
    color: inherit;
  }
`;

const Description = styled(TypographySecondary)`
  margin-bottom: 0;
  font-size: ${p => p.theme.fontSize.tiny};
`;

const Body = styled.div`
  padding: ${p => p.theme.size.tiny};
  padding-bottom: 0;
`;

export default props => {
  const { data, className, index } = props;
  return (
    <ListGroupItem
      className={`${className ? className : ""}${className ? " " : ""}${
        index === 0 ? "first-item" : ""
      }`}
    >
      <Cover>
        <InfoRow>
          <FontAwesomeIcon icon={data.infoIcon} /> {data.info}
        </InfoRow>
        <ActionButton>
          {data.actionIcon ? <FontAwesomeIcon icon={data.actionIcon} /> : null}
          {data.actionText}
        </ActionButton>
        <Thumbnail className="thumbnail" src={data.photoUrl} />
        <Overlay />
      </Cover>
      <Body>
        <Title>
          <AnchorLink to={data.url}>{data.name}</AnchorLink>
        </Title>
        <Description>
          <AnchorLink to={data.url}>{data.description}</AnchorLink>
        </Description>
      </Body>
    </ListGroupItem>
  );
};
