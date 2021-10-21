import React from "react";
import styled from "styled-components";
import ConnectionSuggestionItem from "./ConnectionSuggestionItem";
import { VerticalList } from "../../molecules/List";
import { FifthHeadingNeutralTitle } from "../../atoms/Heading";
import { LoadingBar } from "../../molecules/NotificationBars";

const Root = styled.div`
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  border-radius: ${(p) => p.theme.borderRadius.normal};
  background-color: ${(p) => p.theme.color.whiteBg};
`;

export default (props) => {
  const { loading, data } = props;
  if (loading) {
    return <LoadingBar>Loading</LoadingBar>;
  }

  if (!data) {
    return null;
  }

  const { users } = data;
  const { collections } = users;

  let connections = collections.map((user) => {
    return {
      name: user.displayName,
      description: user.description,
      url: `/profile/${user.userIdentityId}`,
      id: user.userIdentityId,
      imageUrl: user.avatarCode
        ? `${process.env.REACT_APP_CDN_AVATAR_API_URL}${user.avatarCode}`
        : null,
    };
  });

  return (
    <div>
      <FifthHeadingNeutralTitle>
        Connect To Other Farmers
      </FifthHeadingNeutralTitle>
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
