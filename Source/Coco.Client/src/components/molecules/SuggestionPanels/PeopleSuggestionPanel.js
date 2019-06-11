import React from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { ButtonOutlineNormal } from "../../atoms/Buttons";
import { ThumbnailCircle } from "../Thumbnails";
import { TertiaryTitle } from "../../atoms/Titles";
import { TypographySecondary } from "../../atoms/Typographies";
import { AnchorLink } from "../../atoms/Links";

const ListItem = styled.li`
  padding: ${p => p.theme.size.distance};
  border-top: 1px solid ${p => p.theme.color.exLight};

  &.first-item {
    border-top: 0;
  }
`;

const ActionButton = styled(ButtonOutlineNormal)`
  font-weight: 500;
  font-size: ${p => p.theme.fontSize.small};
  top: ${p => p.theme.size.distance};
  right: ${p => p.theme.size.distance};
  padding: ${p => p.theme.size.exTiny};
  z-index: 1;

  svg > path {
    margin-right: 3px;
  }
`;

const Description = styled(TypographySecondary)`
  margin-bottom: 0;
  padding-top: ${p => p.theme.size.exTiny};
`;

const Avatar = styled(ThumbnailCircle)`
  border: 2px solid ${p => p.theme.color.light};
  width: calc(100% + 4px);
  padding-top: 100%;
  position: relative;

  img {
    position: absolute;
    top: 0;
    left: 0;
    bottom: 0;
    right: 0;
  }
`;

export default function (props) {
  const { data, className, index } = props;
  return (
    <ListItem
      className={`${className ? className : ""}${className ? " " : ""}${
        index === 0 ? "first-item" : ""
        }`}
    >
      <div className="row">
        <div className="col col-3 col-sm-3 col-md-4 col-lg-4 col-xl-3">
          <AnchorLink to={data.url}>
            <Avatar src={data.ImageUrl} alt={data.ImageUrl} />
          </AnchorLink>
        </div>
        <div className="col col-auto">
          <TertiaryTitle>
            <AnchorLink to={data.url}>{data.name}</AnchorLink>
          </TertiaryTitle>
          <ActionButton>
            <FontAwesomeIcon icon={data.actionIcon} /> {data.actionText}
          </ActionButton>
        </div>
      </div>
      <Description>{data.description}</Description>
    </ListItem>
  );
};
