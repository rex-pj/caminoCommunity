import styled from "styled-components";

export default styled.div`
  > *:first-child {
    border-top-right-radius: 0;
    border-bottom-right-radius: 0;
  }

  > *:last-child {
    border-top-left-radius: 0;
    border-bottom-left-radius: 0;
  }

  > *:not(:last-child) {
    border-right: 0;
  }
`;
