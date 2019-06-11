import React, { useState, useEffect } from "react";
import styled from "styled-components";
import { faUserCheck, faComments } from "@fortawesome/free-solid-svg-icons";
import UserCard from "./UserCard";
import { PanelDefault } from "../../atoms/Panels";
import UserContext from "../../../utils/Context/UserContext";

const Root = styled(PanelDefault)`
  position: relative;
`;

const Card = styled(UserCard)`
  box-shadow: none;
  border-radius: 0;
`;

export default function (props) {
  const [menu, setMenu] = useState([]);

  useEffect(function () {
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
  })

  return (
    <Root>
      <UserContext.Consumer>
        {({ isLogin, userInfo }) =>
          isLogin ? <Card userInfo={userInfo} menuList={menu} /> : null
        }
      </UserContext.Consumer>
    </Root>
  );
}
