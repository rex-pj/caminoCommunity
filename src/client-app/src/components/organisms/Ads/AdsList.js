import React from "react";
import styled from "styled-components";
import AdsItem from "./AdsItem";
import { VerticalList } from "../../molecules/List";
import { FifthHeadingPrimary } from "../../atoms/Heading";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const Root = styled.div`
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  background-color: ${(p) => p.theme.color.whiteBg};
  border-radius: ${(p) => p.theme.borderRadius.normal};
  padding: ${(p) => p.theme.size.exSmall};

  ${FifthHeadingPrimary} {
    span {
      float: left;
    }

    svg {
      float: right;
      color: ${(p) => p.theme.color.primaryBg};
    }

    :after {
      content: " ";
      display: block;
      clear: both;
    }
  }
`;

export default function (props) {
  let listAds = [];
  for (let i = 0; i < 1; i++) {
    listAds.push({
      description:
        "Specializing in supplying fresh coconuts to agents, bars, wholesalers and retailers in the southern provinces...",
      title: "Baro Siamese coconut granary",
      photoUrl: `${process.env.PUBLIC_URL}/photos/coconut-farm.jpg`,
      url: "#",
      icon: "bullhorn",
    });
  }

  return (
    <Root>
      <FifthHeadingPrimary>
        Suggestions
        <FontAwesomeIcon icon="bullhorn" />
      </FifthHeadingPrimary>
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
}
