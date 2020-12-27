import React from "react";
import styled from "styled-components";
import ConnectionSuggestionItem from "./ConnectionSuggestionItem";
import { VerticalList } from "../../atoms/List";
import { FifthHeadingSecondary } from "../../atoms/Heading";

const Root = styled.div`
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  border-radius: ${(p) => p.theme.borderRadius.normal};
  background-color: ${(p) => p.theme.color.whiteBg};
`;

export default () => {
  let connections = [];
  for (let i = 0; i < 3; i++) {
    connections.push({
      name: "Mr. Fifth",
      description:
        "Donec id elit non mi porta gravida at eget metus. Maecenas sed diam eget risus varius blandit.",
      url: "/profile/4976920d11d17ddb37cd40c54330ba8e",

      id: "1212234r5423",
      ImageUrl: `${process.env.PUBLIC_URL}/photos/conn-farmer.png`,
    });
  }

  return (
    <div>
      <FifthHeadingSecondary>Connect To Other Farmers</FifthHeadingSecondary>
      <Root>
        <VerticalList>
          {connections &&
            connections.map((connection, index) => (
              <ConnectionSuggestionItem
                key={index}
                connection={connection}
                index={index}
              />
            ))}
        </VerticalList>
      </Root>
    </div>
  );
};
