import React from "react";
import styled from "styled-components";
import { ImageRound } from "../../atoms/Images";
import { TertiaryTitle } from "../../atoms/Titles";
import { TypographyTitle } from "../../atoms/Typographies";
import { AnchorLink } from "../../atoms/Links";

const ListItem = styled.li`
  border-top: 1px solid ${(p) => p.theme.color.secondaryDivide};
  padding-top: ${(p) => p.theme.size.exSmall};
`;

const Title = styled(TertiaryTitle)`
  margin-bottom: 3px;
`;

const Description = styled(TypographyTitle)`
  margin-bottom: 0;
`;

const Body = styled.div`
  padding-bottom: ${(p) => p.theme.size.exTiny};
`;

export default (props) => {
  const { data, className, index } = props;
  return (
    <ListItem
      className={`${className ? className : ""}${className ? " " : ""}${
        index === 0 ? "first-item" : ""
      }`}
    >
      <div>
        <Body>
          <a href={data.url}>
            <ImageRound
              className="thumbnail"
              src={data.photoUrl}
              alt={data.photoUrl}
            />
          </a>
        </Body>
        <div>
          <Title>
            <AnchorLink to={data.url}>{data.title}</AnchorLink>
          </Title>

          <Description>{data.description}</Description>
        </div>
      </div>
    </ListItem>
  );
};
