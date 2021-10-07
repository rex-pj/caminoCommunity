import React from "react";
import styled from "styled-components";
import FarmSuggestionItem from "./FarmSuggestionItem";
import { VerticalList } from "../../molecules/List";
import { FifthHeadingSecondary } from "../../atoms/Heading";
import { LoadingBar } from "../../molecules/NotificationBars";

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

export default (props) => {
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
    var pictureUrl = null;
    if (farm.pictures && farm.pictures.length > 0) {
      const picture = farm.pictures[0];
      if (picture.pictureId > 0) {
        pictureUrl = `${process.env.REACT_APP_CDN_PHOTO_URL}${picture.pictureId}`;
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
    <div>
      <FifthHeadingSecondary>Visit other farms</FifthHeadingSecondary>
      <Root>
        <List>
          {farms &&
            farms.map((farm, index) => (
              <FarmSuggestionItem key={index} farm={farm} index={index} />
            ))}
        </List>
      </Root>
    </div>
  );
};
