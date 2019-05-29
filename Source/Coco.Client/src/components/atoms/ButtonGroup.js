import styled from "styled-components";

export default styled.div`
  > span,
  a,
  button {
    border-radius: 0;
  }

  > span:first-child,
  a:first-child,
  button:first-child {
    border-top-left-radius: ${p => p.theme.borderRadius.normal};
    border-bottom-left-radius: ${p => p.theme.borderRadius.normal};
  }

  > span:last-child,
  a:last-child,
  button:last-child {
    border-top-right-radius: ${p => p.theme.borderRadius.normal};
    border-bottom-right-radius: ${p => p.theme.borderRadius.normal};
  }

  > span:not(:last-child),
  a:not(:last-child),
  button:not(:last-child) {
    border-right: 0;
  }
`;
