import React from "react";
import styled from "styled-components";
import AssociationSuggestionItem from "./AssociationSuggestionItem";
import { VerticalList } from "../../atoms/List";
import { FifthDarkHeading } from "../../atoms/Heading";
import { UrlConstant } from "../../../utils/Constants";

const Root = styled.div`
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  border-radius: ${(p) => p.theme.borderRadius.normal};
  background-color: ${(p) => p.theme.color.white};
`;

const List = styled(VerticalList)`
  li {
    margin-bottom: ${(p) => p.theme.size.exSmall};
  }
`;

export default () => {
  let associations = [];
  for (let i = 0; i < 3; i++) {
    associations.push({
      info: "125 participants",
      name: "Hội trái cây Mini",
      description:
        "Donec id elit non mi porta gravida at eget metus. Maecenas sed diam eget risus varius blandit.",
      url: `${UrlConstant.Association.url}1`,
      id: "1212234r5423",
      photoUrl: `${process.env.PUBLIC_URL}/photos/farm-group-cover.jpg`,
    });
  }

  return (
    <div>
      <FifthDarkHeading>
        Join The Farm Association To Connect Better!
      </FifthDarkHeading>
      <Root>
        <List>
          {associations &&
            associations.map((association, index) => (
              <AssociationSuggestionItem
                key={index}
                association={association}
                index={index}
              />
            ))}
        </List>
      </Root>
    </div>
  );
};
