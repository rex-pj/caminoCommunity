import React, { useState, useEffect } from "react";
import styled from "styled-components";
import { faUserCheck, faComments } from "@fortawesome/free-solid-svg-icons";
import UserCard from "./UserCard";
import ProfileCardInfo from "./ProfileCardInfo";
import { PanelDefault } from "../../atoms/Panels";
import { UrlConstant } from "../../../utils/Constant";

const Root = styled(PanelDefault)`
  position: relative;
`;

const Card = styled(UserCard)`
  box-shadow: none;
  border-radius: 0;
  border-bottom: 1px solid ${p => p.theme.rgbaColor.dark};
`;

export default function(props) {

  const [infos, setInfosState] = useState([]);
  const [menu, setMenu] = useState([]);

  useEffect(function(){
    const infos = [
      {
        name: "Công việc",
        infos: [
          {
            icon: "user",
            name: "Nông dân"
          }
        ]
      },
      {
        name: "Nông trại",
        infos: [
          {
            icon: "warehouse",
            name: "Vườn chú Năm (Chợ Lách)",
            url: `${UrlConstant.Farm.url}1`
          },
          {
            icon: "warehouse",
            name: "Vườn chú Năm (Mỏ Cày Bắc)",
            url: `${UrlConstant.Farm.url}2`
          }
        ]
      },
      {
        name: "Địa chỉ",
        infos: [
          {
            icon: "map-marker-alt",
            name: "ấp Vĩnh Bình, xã Mỹ Thạnh, huyện Cần Thinh, tỉnh Bình Tuy"
          }
        ]
      },
      {
        name: "Liên lạc",
        infos: [
          {
            icon: "phone",
            name: "+84.787.888.667"
          },
          {
            icon: "envelope",
            name: "trungle.it@gmail.com",
            url: "trungle.it@gmail.com"
          }
        ]
      }
    ];

    setInfosState(infos);

    const menuList = [
      {
        icon: faUserCheck,
        text: "800",
        description: "Được theo Dõi"
      },
      {
        icon: faComments,
        text: "350",
        description: "Chủ Đề"
      }
    ];

    setMenu(menuList);
  });

  return (
    <Root>
      <Card menuList={menu} />
      <ProfileCardInfo profileInfos={infos} />
    </Root>
  );
}
