import styled from "styled-components";
import { secondaryTitle, TertiaryTitle } from "../Titles";

export const secondaryTitleLink = styled(secondaryTitle)`
  a {
    color: ${(p) => p.theme.color.darkText};

    :hover {
      color: ${(p) => p.theme.color.primaryLink};
    }
  }
`;

export const TertiaryTitleLink = styled(TertiaryTitle)`
  a {
    color: ${(p) => p.theme.color.darkText};

    :hover {
      color: ${(p) => p.theme.color.primaryLink};
    }
  }
`;
