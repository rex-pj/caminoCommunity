import React from "react";
import styled from "styled-components";
import loadable from "@loadable/component";
import { faUserCheck, faComments } from "@fortawesome/free-solid-svg-icons";
import ProfileCardInfo from "./ProfileCardInfo";
import { PanelDefault } from "../../atoms/Panels";
import { UrlConstant } from "../../../utils/Constants";
const UserCard = loadable(() => import("./UserCard"));

const Root = styled(PanelDefault)`
  position: relative;
`;

const Card = styled(UserCard)`
  box-shadow: none;
  border-radius: 0;
  border-bottom: 1px solid ${(p) => p.theme.color.secondaryDivide};
`;

export default function (props) {
  const infos = [
    {
      name: "Job",
      infos: [
        {
          icon: "user",
          name: "Farmer",
        },
      ],
    },
    {
      name: "Farms",
      infos: [
        {
          icon: "warehouse",
          name: "Vườn chú Năm (Chợ Lách)",
          url: `${UrlConstant.Farm.url}1`,
        },
        {
          icon: "warehouse",
          name: "Vườn chú Năm (Mỏ Cày Bắc)",
          url: `${UrlConstant.Farm.url}2`,
        },
      ],
    },
    {
      name: "Address",
      infos: [
        {
          icon: "map-marker-alt",
          name: "ấp Vĩnh Bình, xã Mỹ Thạnh, huyện Cần Thinh, tỉnh Bình Tuy",
        },
      ],
    },
    {
      name: "Contact Information",
      infos: [
        {
          icon: "phone",
          name: "+84.787.888.667",
        },
        {
          icon: "envelope",
          name: "trungle.it@gmail.com",
          url: "/profile/4976920d11d17ddb37cd40c54330ba8e",
        },
      ],
    },
  ];
  const menuList = [
    {
      icon: faUserCheck,
      text: "800",
      description: "Following",
    },
    {
      icon: faComments,
      text: "350",
      description: "Topics",
    },
  ];

  return (
    <Root>
      <Card menuList={menuList} />
      <ProfileCardInfo profileInfos={infos} />
    </Root>
  );
}
