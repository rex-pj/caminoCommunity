import React from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Thumbnail } from "../../molecules/Thumbnails";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { FifthHeadingSecondary } from "../../atoms/Heading";
import { TypographyTitle } from "../../atoms/Typographies";
import { AnchorLink } from "../../atoms/Links";
import Overlay from "../../atoms/Overlay";

const ListGroupItem = styled.li`
  padding-bottom: ${(p) => p.theme.size.tiny};

  &.first-item .thumbnail > img {
    border-top-left-radius: ${(p) => p.theme.borderRadius.normal};
    border-top-right-radius: ${(p) => p.theme.borderRadius.normal};
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
  color: ${(p) => p.theme.color.lightText};
  z-index: 2;
  font-size: 13px;
  right: 5px;

  svg,
  path {
    color: inherit;
  }
`;

const ActionButton = styled(ButtonPrimary)`
  font-weight: 400;
  font-size: ${(p) => p.theme.fontSize.small};
  position: absolute;
  top: ${(p) => p.theme.size.distance};
  right: ${(p) => p.theme.size.distance};
  padding: ${(p) => p.theme.size.exTiny};
  z-index: 1;

  svg {
    margin-right: 3px;
  }
`;

const Description = styled(TypographyTitle)`
  margin-bottom: 0;
  font-size: ${(p) => p.theme.fontSize.tiny};
`;

const Body = styled.div`
  padding: ${(p) => p.theme.size.tiny};
  padding-bottom: 0;
`;

export default (props) => {
  const { data, className, index } = props;
  const listGroupItemClassName = `${className ? className : ""}${
    index === 0 ? "first-item" : ""
  }`;
  return (
    <ListGroupItem className={listGroupItemClassName}>
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
        <FifthHeadingSecondary>
          <AnchorLink to={data.url}>{data.name}</AnchorLink>
        </FifthHeadingSecondary>
        <Description
          dangerouslySetInnerHTML={{ __html: data.description }}
        ></Description>
      </Body>
    </ListGroupItem>
  );
};
