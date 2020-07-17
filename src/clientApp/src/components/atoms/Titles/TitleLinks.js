import styled from "styled-components";
import { SecondaryTitle, TertiaryTitle } from "../Titles";

export const SecondaryTitleLink = styled(SecondaryTitle)`
  a {
    color: ${p => p.theme.color.dark};

    :hover {
      color: ${p => p.theme.color.link};
    }
  }
`;

export const TertiaryTitleLink = styled(TertiaryTitle)`
  a {
    color: ${p => p.theme.color.dark};

    :hover {
      color: ${p => p.theme.color.link};
    }
  }
`;
