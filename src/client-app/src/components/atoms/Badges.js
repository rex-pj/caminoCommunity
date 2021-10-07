import styled from "styled-components";

export const BadgeOutlinePrimary = styled.label`
  font-size: ${(p) => p.theme.fontSize.normal};
  color: ${(p) => p.theme.color.lightBg};
  border-color: ${(p) => p.theme.color.lightBg};
  border-width: 1px;
  border-style: solid;
  padding: ${(p) =>
    p.size === "xs"
      ? "5px 8px"
      : p.size === "sm"
      ? ".5rem .75rem"
      : "10px 15px"};
  border-radius: ${(p) => p.theme.borderRadius.normal};
`;
