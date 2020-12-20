import React from "react";
import styled from "styled-components";
import CommunitySuggestionItem from "./CommunitySuggestionItem";
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
  let communities = [];
  for (let i = 0; i < 3; i++) {
    communities.push({
      info: "125 participants",
      name: "Hội trái cây Mini",
      description:
        "Donec id elit non mi porta gravida at eget metus. Maecenas sed diam eget risus varius blandit.",
      url: `${UrlConstant.Community.url}1`,
      id: "1212234r5423",
      photoUrl: `${process.env.PUBLIC_URL}/photos/farm-group-cover.jpg`,
    });
  }

  return (
    <div>
      <FifthDarkHeading>
        Join The Farm Community To Connect Better!
      </FifthDarkHeading>
      <Root>
        <List>
          {communities &&
            communities.map((communitie, index) => (
              <CommunitySuggestionItem
                key={index}
                communitie={communitie}
                index={index}
              />
            ))}
        </List>
      </Root>
    </div>
  );
};
