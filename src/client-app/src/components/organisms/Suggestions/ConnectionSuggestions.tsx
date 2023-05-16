import * as React from "react";
import styled from "styled-components";
import ConnectionSuggestionItem from "./ConnectionSuggestionItem";
import { VerticalList } from "../../molecules/List";
import { FifthHeadingNeutralTitle } from "../../atoms/Heading";
import { LoadingBar } from "../../molecules/NotificationBars";
import { apiConfig } from "../../../config/api-config";
import { useTranslation } from "react-i18next";

const Root = styled.div`
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  border-radius: ${(p) => p.theme.borderRadius.normal};
  background-color: ${(p) => p.theme.color.whiteBg};
`;

type Props = {
  loading?: boolean;
  data?: any;
};

const ConnectionSuggestions = (props: Props) => {
  const { t } = useTranslation();
  const { loading, data } = props;
  if (loading) {
    return <LoadingBar />;
  }

  if (!data) {
    return null;
  }

  const { users } = data;
  const { collections } = users;

  let connections = collections.map((user: any) => {
    return {
      name: user.displayName,
      description: user.description,
      url: `/profile/${user.userIdentityId}`,
      id: user.userIdentityId,
      imageUrl: user.avatarId
        ? `${apiConfig.paths.userPhotos.get.getAvatar}/${user.avatarId}`
        : null,
    };
  });

  return (
    <div>
      <FifthHeadingNeutralTitle>
        {t("connect_other_farmers")}
      </FifthHeadingNeutralTitle>
      <Root>
        <VerticalList>
          {connections &&
            connections.map((connection: any, index: number) => (
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

export default ConnectionSuggestions;
