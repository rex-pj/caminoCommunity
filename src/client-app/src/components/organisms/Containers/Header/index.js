import React from "react";
import styled from "styled-components";
import Logo from "./Logo";
import SearchBar from "../../Search/SearchBar";
import NavigationMenu from "../../Navigation/NavigationMenu";

const NavRoot = styled.nav`
  background-image: ${(p) => p.theme.gradientColor.primary};
  padding: 2px 16px;
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  position: relative;
  z-index: 100;
`;

const NavMenu = styled(NavigationMenu)`
  float: right;
`;

export default () => {
  return (
    <NavRoot className="navbar px-0 px-sm-3 px-md-1 px-lg-3">
      <div className="container-fluid">
        <div className="row gx-0 gx-lg-4">
          <div className="col-auto col-sm-1 col-md-auto col-lg-auto pe-2 pe-sm-0 pe-md-4 ">
            <Logo />
          </div>

          <div className="col-8 col-sm-7 col-md-6 col-lg-auto me-auto">
            <SearchBar className="ms-1 ms-sm-0" />
          </div>
          <div className="col">
            <div className="clearfix">
              <NavMenu />
            </div>
          </div>
        </div>
      </div>
    </NavRoot>
  );
};
