import React, { Component } from "react";
import styled from "styled-components";
import loadable from "@loadable/component";
import { faUserCheck, faComments } from "@fortawesome/free-solid-svg-icons";
import { PanelDefault } from "../../atoms/Panels";
import UserContext from "../../../utils/Context/UserContext";
const UserCard = loadable(() => import("./UserCard"));

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
          {({ user }) =>
            user && user.isLogin ? (
              <Card userInfo={user.userInfo} menuList={menuList} />
            ) : null
          }
        </UserContext.Consumer>
      </Root>
    );
  }
}
