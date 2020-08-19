import React, { Component } from "react";
import styled from "styled-components";
import AdsItem from "./AdsItem";
import { VerticalList } from "../../atoms/List";
import { FifthDarkHeading } from "../../atoms/Heading";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const Root = styled.div`
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  background-color: ${(p) => p.theme.color.white};
  border-radius: ${(p) => p.theme.borderRadius.normal};
  padding: ${(p) => p.theme.size.exSmall};

  ${FifthDarkHeading} {
    span {
      float: left;
    }

    svg {
      float: right;
      color: ${(p) => p.theme.color.neutral};
    }

    :after {
      content: " ";
      display: block;
      clear: both;
    }
  }
`;

export default class AdsList extends Component {
  constructor(props) {
    super(props);

    const listAds = [];
    for (let i = 0; i < 1; i++) {
      listAds.push({
        description:
          "Chuyên cung cấp dừa tươi cho các đại lý, quán nước, sỉ và lẻ cho các tỉnh thành phía Nam...",
        title: "Vựa dừa xiêm anh Ba Rô",
        photoUrl: `${process.env.PUBLIC_URL}/photos/coconut-farm.jpg`,
        url: "#",
        icon: "bullhorn",
      });
    }

    this.state = {
      listAds: listAds,
    };
  }

  render() {
    const { listAds } = this.state;
    return (
      <Root>
        <FifthDarkHeading>
          <span>Suggestions</span>
          <FontAwesomeIcon icon="bullhorn" />
        </FifthDarkHeading>
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
}
