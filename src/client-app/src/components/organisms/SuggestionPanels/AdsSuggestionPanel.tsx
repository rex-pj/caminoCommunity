import * as React from "react";
import styled from "styled-components";
import { ImageRound } from "../../atoms/Images";
import { TertiaryTitle } from "../../atoms/Titles";
import { TypographyPrimary } from "../../atoms/Typographies";
import { AnchorLink } from "../../atoms/Links";
import { HTMLAttributes } from "react";

const ListItem = styled.li`
  border-top: 1px solid ${(p) => p.theme.color.neutralBg};
  padding-top: ${(p) => p.theme.size.exSmall};
`;

const Title = styled(TertiaryTitle)`
  margin-bottom: 3px;
`;

const Description = styled(TypographyPrimary)`
  margin-bottom: 0;
  font-size: ${(p) => p.theme.fontSize.tiny};
`;

const Body = styled.div`
  padding-bottom: ${(p) => p.theme.size.exTiny};
`;

export interface AdsSuggestionData {
  url?: string;
  photoUrl?: string;
  title?: string;
  description?: string;
}

interface AdsSuggestionPanelProps extends HTMLAttributes<HTMLLIElement> {
  index: number;
  data: AdsSuggestionData;
}

const AdsSuggestionPanel = (props: AdsSuggestionPanelProps) => {
  const { data, className, index } = props;
  return (
    <ListItem
      className={`${className ?? ""}${index === 0 ? "first-item" : ""}`}
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

export default AdsSuggestionPanel;
