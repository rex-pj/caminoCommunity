import * as React from "react";
import styled from "styled-components";
import AdsItem from "./AdsItem";
import { VerticalList } from "../../molecules/List";

const Root = styled.div`
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  background-color: ${(p) => p.theme.color.whiteBg};
  border-radius: ${(p) => p.theme.borderRadius.normal};
  padding: ${(p) => p.theme.size.exSmall};
`;

interface AdsListProps {}

const AdsList: React.FC<AdsListProps> = (props) => {
  let listAds: any[] = [];
  for (let i = 0; i < 1; i++) {
    listAds.push({
      description:
        "Specializing in supplying fresh coconuts to agents, bars, wholesalers and retailers in the southern provinces...",
      title: "Baro Siamese coconut granary",
      photoUrl: null,
      url: "#",
      icon: "bullhorn",
    });
  }

  return (
    <Root>
      <div>
        <VerticalList>
          {listAds &&
            listAds.map((ads, index) => (
              <AdsItem key={index} ads={ads} index={index} />
            ))}
        </VerticalList>
      </div>
    </Root>
  );
};

export default AdsList;
