import styled from "styled-components";
import { ListItem } from "../../atoms/List";

export default styled(ListItem)`
  margin-bottom: 3px;

  a {
    color: ${p => p.theme.color.dark};
    display: block;
    text-decoration: none;
    border-radius: ${p => p.theme.borderRadius.normal};
    padding: 8px 3px 8px 3px;
    font-size: ${p => p.theme.fontSize.small};
    line-height: 1;
    width: 100%;
    text-overflow: ellipsis;
    overflow: hidden;
    white-space: nowrap;
  }

  &.actived a,
  & a.actived {
    font-weight: 600;
    background-color: ${p => p.theme.color.lighter};
  }

  &:hover a {
    background-color: ${p => p.theme.color.lighter};
  }

  a > span > svg {
    margin-right: 3px;
    width: auto !important;
    font-size: ${p => p.theme.fontSize.normal};
    text-align: left;
    color: ${p => p.theme.color.primary};
  }

  span {
    font-weight: bold;
  }

  span,
  svg,
  path {
    color: inherit;
  }
`;
