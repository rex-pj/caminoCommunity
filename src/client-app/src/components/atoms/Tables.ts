import styled from "styled-components";

export const TableResponsive = styled.div`
  overflow-x: auto;
  -webkit-overflow-scrolling: touch;
`;

export const Table = styled.table`
  width: 100%;
  margin-bottom: 1rem;
  color: ${(p) => p.theme.color.darkText};
  vertical-align: top;
  border-color: ${(p) => p.theme.color.neutralBorder};

  &.table-striped > tbody > tr::nth-of-type(odd) > td {
    background-color: ${(p) => p.theme.color.whiteBg};
  }

  tbody tr td {
    padding: 0.5rem 0.5rem;
    background-color: ${(p) => p.theme.color.lightBg};
    border-bottom-width: 1px;
  }

  > :not(:first-child) {
    border-top: 2px solid ${(p) => p.theme.color.primaryBg};
  }
`;
