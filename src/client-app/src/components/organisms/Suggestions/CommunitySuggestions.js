import React from "react";
import styled from "styled-components";
import CommunitySuggestionItem from "./CommunitySuggestionItem";
import { VerticalList } from "../../molecules/List";
import { FifthHeadingNeutralTitle } from "../../atoms/Heading";
import { UrlConstant } from "../../../utils/Constants";

const Root = styled.div`
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  border-radius: ${(p) => p.theme.borderRadius.normal};
  background-color: ${(p) => p.theme.color.whiteBg};
`;

const List = styled(VerticalList)`
  li {
    margin-bottom: ${(p) => p.theme.size.exSmall};
  }
`;

const CommunitySuggestions = () => {
  let communities = [];
  for (let i = 0; i < 3; i++) {
    communities.push({
      info: "125 participants",
      name: "Mini fruits group",
      description:
        "Donec id elit non mi porta gravida at eget metus. Maecenas sed diam eget risus varius blandit.",
      url: `${UrlConstant.Community.url}1`,
      id: "1212234r5423",
      photoUrl: null,
    });
  }

  return (
    <div>
      <FifthHeadingNeutralTitle>
        Join The Farm Communities To Connect Better!
      </FifthHeadingNeutralTitle>
      <Root>
        <List>
          {communities &&
            communities.map((community, index) => (
              <CommunitySuggestionItem
                key={index}
                community={community}
                index={index}
              />
            ))}
        </List>
      </Root>
    </div>
  );
};

export default CommunitySuggestions;
