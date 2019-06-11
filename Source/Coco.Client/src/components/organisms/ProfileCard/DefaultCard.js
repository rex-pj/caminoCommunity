import React, { Component } from "react";
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

export default class extends Component {
  constructor(props) {
    super(props);

    this.state = {
      menuList: [
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
      ]
    };
  }

  render() {
    const { menuList } = this.state;
    return (
      <Root>
        <UserContext.Consumer>
          {({ isLogin, userInfo }) =>
            isLogin ? <Card userInfo={userInfo} menuList={menuList} /> : null
          }
        </UserContext.Consumer>
      </Root>
    );
  }
}
