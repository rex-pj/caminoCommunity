import * as React from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Thumbnail } from "../../molecules/Thumbnails";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { FifthHeadingNeutralTitle } from "../../atoms/Heading";
import { TypographyPrimary } from "../../atoms/Typographies";
import { AnchorLink } from "../../atoms/Links";
import Overlay from "../../atoms/Overlay";
import { IconProp } from "@fortawesome/fontawesome-svg-core";

const ListGroupItem = styled.li`
  padding-bottom: ${(p) => p.theme.size.tiny};

  &.first-item .thumbnail > img {
    border-top-left-radius: ${(p) => p.theme.borderRadius.normal};
    border-top-right-radius: ${(p) => p.theme.borderRadius.normal};
  }
`;

const Cover = styled.div`
  position: relative;
  min-height: 80px;

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

const Description = styled(TypographyPrimary)`
  margin-bottom: 0;
  font-size: ${(p) => p.theme.fontSize.tiny};
`;

const Body = styled.div`
  padding: ${(p) => p.theme.size.tiny};
  padding-bottom: 0;
`;

type Props = {
  data: {
    description: string;
    actionText: string;
    url: string;
    imageUrl: string;
    name: string;
    actionIcon?: IconProp;
    infoIcon?: IconProp;
    photoUrl?: string;
    info: string;
  };
  className?: string;
  index?: number;
};

const SuggestionPanel = (props: Props) => {
  const { data, className, index } = props;
  const listGroupItemClassName = `${className ? className : ""}${
    index === 0 ? "first-item" : ""
  }`;
  return (
    <ListGroupItem className={listGroupItemClassName}>
      <Cover>
        <InfoRow>
          {data.infoIcon ? <FontAwesomeIcon icon={data.infoIcon} /> : null}
          {data.info}
        </InfoRow>
        <ActionButton>
          {data.actionIcon ? <FontAwesomeIcon icon={data.actionIcon} /> : null}
          {data.actionText}
        </ActionButton>
        <Thumbnail className="thumbnail" src={data.photoUrl} />
        <Overlay />
      </Cover>
      <Body>
        <FifthHeadingNeutralTitle>
          <AnchorLink to={data.url}>{data.name}</AnchorLink>
        </FifthHeadingNeutralTitle>
        <Description
          dangerouslySetInnerHTML={{ __html: data.description }}
        ></Description>
      </Body>
    </ListGroupItem>
  );
};

export default SuggestionPanel;
