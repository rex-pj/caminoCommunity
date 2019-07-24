import React, { Component } from "react";
import styled from "styled-components";
import { HorizontalList } from "../../atoms/List";
import DropdownButton from "../../molecules/DropdownButton";
import NavLinkActived from "../../molecules/Links/ProfileNavLink";

const Root = styled.div`
  position: relative;
`;

const ListItem = styled.li`
  display: inline-block;
  margin: 0 ${p => p.theme.size.distance};

  a.actived {
    color: ${p => p.theme.color.warning};
    text-decoration: none;
  }

  :hover a {
    color: ${p => p.theme.color.warning};
    text-decoration: none;
  }

  a {
    color: ${p => p.theme.color.neutral};
    font-weight: 500;
    font-size: ${p => p.theme.fontSize.small};
    border: 0;
    height: ${p => p.theme.size.medium};
    line-height: ${p => p.theme.size.medium};
  }
`;

const UserDropdown = styled(DropdownButton)`
  padding: 7px;
`;

export default (class extends Component {
  render() {
    const { className, userId } = this.props;
    return (
      <Root>
        <div className="row">
          <div className="col-auto mr-auto">
            <HorizontalList className={className}>
              <ListItem>
                <NavLinkActived {...this.props} userId={userId}>
                  Tất cả
                </NavLinkActived>
              </ListItem>
              <ListItem>
                <NavLinkActived pageNav="posts" {...this.props} userId={userId}>
                  Bài Viết
                </NavLinkActived>
              </ListItem>
              <ListItem>
                <NavLinkActived
                  pageNav="products"
                  {...this.props}
                  userId={userId}
                >
                  Sản Phẩm
                </NavLinkActived>
              </ListItem>
              <ListItem>
                <NavLinkActived pageNav="farms" {...this.props} userId={userId}>
                  Nông Trại
                </NavLinkActived>
              </ListItem>
              <ListItem>
                <NavLinkActived
                  pageNav="followings"
                  {...this.props}
                  userId={userId}
                >
                  Được Theo Dõi
                </NavLinkActived>
              </ListItem>
              <ListItem>
                <NavLinkActived pageNav="about" {...this.props} userId={userId}>
                  Giới thiệu
                </NavLinkActived>
              </ListItem>
            </HorizontalList>
          </div>
          <div className="col-auto">
            <UserDropdown
              icon="ellipsis-v"
              dropdown={[
                { url: "", name: "Cập nhật thông tin cá nhân" },
                { url: "", name: "Đổi email" },
                { url: "", name: "Đổi mật khẩu" }
              ]}
            />
          </div>
        </div>
      </Root>
    );
  }
});
