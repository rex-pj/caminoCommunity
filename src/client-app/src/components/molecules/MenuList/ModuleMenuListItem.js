import styled from "styled-components";
import { ListItem } from "../../molecules/List";

export default styled(ListItem)`
  margin-bottom: 3px;

  a {
    color: ${(p) => p.theme.color.darkText};
    display: block;
    text-decoration: none;
    padding: 11px 3px 10px 0;
    font-size: ${(p) => p.theme.fontSize.small};
    line-height: 1;
    position: relative;
    .menu-item-text {
      display: inline-block;
      text-overflow: ellipsis;
      white-space: nowrap;
    }
  }

  &.actived a,
  & a.actived {
    font-weight: 600;
    :before {
      display: block;
      content: "";
      position: absolute;
      left: -7px;
      top: 50%;
      bottom: 50%;
      height: 12px;
      width: 3px;
      background: ${(p) => p.theme.color.neutralBg};
      margin: auto 0;
    }
  }

  &:hover a {
    font-weight: 600;
  }

  a > span > svg {
    margin-right: 3px;
    width: auto !important;
    font-size: ${(p) => p.theme.fontSize.tiny};
    text-align: left;
    color: ${(p) => p.theme.color.darkText};
  }

  span,
  svg,
  path {
    color: inherit;
  }
`;
