import React, { useEffect, useState } from "react";
import styled from "styled-components";
import AdsItem from "./AdsItem";
import { VerticalList } from "../../atoms/List";
import { QuaternaryHeading } from "../../atoms/Heading";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const Root = styled.div`
  box-shadow: ${p => p.theme.shadow.BoxShadow};
  background-color: ${p => p.theme.color.white};
  border-radius: ${p => p.theme.borderRadius.normal};
  padding: ${p => p.theme.size.exSmall};

  ${QuaternaryHeading} {
    & * {
      color: ${p => p.theme.color.normal};
      font-size: ${p => p.theme.fontSize.exSmall};
    }

    span {
      float: left;
    }

    svg {
      float: right;
    }

    :after {
      content: " ";
      display: block;
      clear: both;
    }
  }
`;

export default function (props) {
  const [advertisements, setAdvertisements] = useState([]);

  useEffect(function () {
    const listAds = [];
    for (let i = 0; i < 1; i++) {
      listAds.push({
        description:
          "Chuyên cung cấp dừa tươi cho các đại lý, quán nước, sỉ và lẻ cho các tỉnh thành phía Nam...",
        title: "Vựa dừa xiêm anh Ba Rô",
        photoUrl: `${process.env.PUBLIC_URL}/photos/coconut-farm.jpg`,
        url: "#",
        icon: "bullhorn"
      });
    }

    setAdvertisements(listAds);
  })

  return (
    <Root>
      <QuaternaryHeading>
        <span>Gợi Ý</span>
        <FontAwesomeIcon icon="bullhorn" />
      </QuaternaryHeading>
      <div>
        <VerticalList>
          {advertisements &&
            advertisements.map((ads, index) => (
              <AdsItem key={index} ads={ads} index={index} />
            ))}
        </VerticalList>
      </div>
    </Root>
  );
}