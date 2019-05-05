import React, { Fragment } from "react";
import { withRouter } from "react-router-dom";
import styled from "styled-components";
import { RouterLinkButton } from "../../atoms/RouterLinkButtons";

const List = styled.ul`
  list-style: none;
  padding-left: 0;
  margin-bottom: 0;
  height: calc(${p => p.theme.size.normal} - 2px);
  margin: 2px 0;

  > li {
    display: inline-block;
  }
`;

const ListItem = styled.li`
  display: inline-block;
  height: calc(${p => p.theme.size.normal} - 2px);
`;

const Devided = styled.li`
  width: 1px;
  height: 15px;
  margin-left: 0;
  background: ${p => p.theme.rgbaColor.dark};
  vertical-align: middle;
  margin: 0 2px;
`;

const AuthButton = styled(RouterLinkButton)`
  color: ${p => p.theme.color.normal};
  font-weight: 500;
  height: 100%;
  padding-top: 5px;
  padding-bottom: 5px;
  font-size: ${p => p.theme.fontSize.small};

  border: 1px solid ${p => p.theme.color.secondary};

  :hover {
    color: ${p => p.theme.color.light};
  }
`;

export default withRouter(props => {
  const { location } = props;
  const { pathname } = location;
  return (
    <List className={props.className}>
      {pathname !== "/auth/signup" ? (
        <ListItem>
          <AuthButton to="/auth/signup">Đăng Ký</AuthButton>
        </ListItem>
      ) : null}

      {pathname !== "/auth/signin" ? (
        <Fragment>
          <Devided />
          <ListItem>
            <AuthButton to="/auth/signin">Đăng Nhập</AuthButton>
          </ListItem>
        </Fragment>
      ) : null}
    </List>
  );
});
