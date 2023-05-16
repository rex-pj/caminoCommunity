import * as React from "react";
import styled from "styled-components";
import Logo from "./Logo";
import SearchBar from "../../Search/SearchBar";
import TopMenuContainer from "./TopMenuContainer";

const NavRoot = styled.nav`
  background-image: ${(p) => p.theme.gradientColor.primary};
  padding: 2px 16px;
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  position: relative;
  z-index: 100;
`;

interface HeaderProps {
  className?: string;
}

const Header: React.FC<HeaderProps> = (props) => {
  return (
    <NavRoot
      className={`${
        props.className ? `${props.className} ` : ""
      }"navbar px-0 px-sm-3 px-md-1 px-lg-3"`}
    >
      <div className="container-fluid px-1 px-sm-3">
        <div className="row gx-0 gx-lg-4">
          <div className="col-1 col-sm-1 col-md-auto col-lg-auto pe-2 pe-sm-0 pe-md-4">
            <Logo />
          </div>

          <div className="col-7 col-sm-7 col-md-6 col-lg-auto me-auto">
            <SearchBar className="ms-1 ms-sm-0" />
          </div>
          <div className="col-auto">
            <TopMenuContainer />
          </div>
        </div>
      </div>
    </NavRoot>
  );
};

export default Header;
