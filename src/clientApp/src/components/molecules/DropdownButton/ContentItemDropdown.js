import styled from "styled-components";
import Dropdown from "./Dropdown";
import ModuleMenuListItem from "../MenuList/ModuleMenuListItem";

export default styled(Dropdown)`
  position: absolute;
  right: 0;
  top: calc(100% + ${(p) => p.theme.size.exTiny});
  background: ${(p) => p.theme.color.white};
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  min-width: calc(${(p) => p.theme.size.large} * 3);
  border-radius: ${(p) => p.theme.borderRadius.normal};
  padding: ${(p) => p.theme.size.exTiny} 0;

  ${ModuleMenuListItem} span {
    display: block;
    margin-bottom: 0;
    border-bottom: 1px solid ${(p) => p.theme.color.lighter};
    padding: ${(p) => p.theme.size.exTiny} ${(p) => p.theme.size.tiny};
    cursor: pointer;
    text-align: left;

    :hover {
      background-color: ${(p) => p.theme.color.lighter};
    }

    :last-child {
      border-bottom: 0;
    }
  }
`;
