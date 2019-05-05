import React from "react";
import styled from "styled-components";
import Logo from "./Logo";
import SearchBar from "../../Search/SearchBar";
import NavigationMenu from "../../NavigationMenu/NavigationMenu";

const NavRoot = styled.nav`
  background-color: ${p => p.theme.color.primary};
  padding: 2px 16px;
  box-shadow: ${p => p.theme.shadow.BoxShadow};
  position: relative;
  z-index: 100;
`;

const NavMenu = styled(NavigationMenu)`
  float: right;
`;

function Header() {
  return (
    <NavRoot className="navbar">
      <div className="container-fluid">
        <div className="row justify-content-sm-center">
          <div className="col-xs-4 col-sm-4 col-md-auto col-lg-auto">
            <Logo />
          </div>

          <div className="col-xs-8 col-sm-8 col-md-5 col-lg-auto">
            <SearchBar />
          </div>
          <div className="col offset-1 offset-sm-0 offset-md-0 offset-lg-1">
            <div className="clearfix">
              <NavMenu />
            </div>
          </div>
        </div>
      </div>
    </NavRoot>
  );
}

export default Header;
