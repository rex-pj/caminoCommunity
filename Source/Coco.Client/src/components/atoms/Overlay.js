import styled from "styled-components";

export default styled.div`
  position: absolute;
  top: 0;
  left: 0;
  bottom: 0;
  width: 100%;
  height: 100%;
  border-top-left-radius: ${p => p.theme.borderRadius.normal};
  border-top-right-radius: ${p => p.theme.borderRadius.normal};
  background: linear-gradient(transparent, ${p => p.theme.rgbaColor.exDark});
  z-index: 0;
`;
