import React from "react";
import styled from "styled-components";
import FarmSuggestionItem from "./FarmSuggestionItem";
import { VerticalList } from "../../molecules/List";
import { FifthHeadingNeutralTitle } from "../../atoms/Heading";
import { LoadingBar } from "../../molecules/NotificationBars";
import { apiConfig } from "../../../config/api-config";

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

const FarmSuggestions = (props) => {
  const { loading, data } = props;
  if (loading) {
    return <LoadingBar>Loading</LoadingBar>;
  }

  if (!data) {
    return null;
  }
  const { farms: farmsData } = data;
  const { collections } = farmsData;

  let farms = collections.map((farm) => {
    let pictureUrl = null;
    if (farm.pictures && farm.pictures.length > 0) {
      const picture = farm.pictures[0];
      if (picture.pictureId > 0) {
        pictureUrl = `${apiConfig.paths.pictures.get.getPicture}/${picture.pictureId}`;
      }
    }
    return {
      info: farm.address,
      name: farm.name,
      description: farm.description,
      url: `/farms/${farm.id}`,
      id: farm.id,
      photoUrl: pictureUrl,
    };
  });

  return (
    farms &&
    farms.length > 0 && (
      <div>
        <FifthHeadingNeutralTitle>Visit other farms</FifthHeadingNeutralTitle>
        <Root>
          <List>
            {farms.map((farm, index) => (
              <FarmSuggestionItem key={farm.id} farm={farm} index={index} />
            ))}
          </List>
        </Root>
      </div>
    )
  );
};

export default FarmSuggestions;
