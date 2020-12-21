import React, { useEffect } from "react";
import styled from "styled-components";
import { ButtonTransparent } from "../../atoms/Buttons/Buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const LinkTo = styled.a`
  display: block;
  width: 280px;
  background-color: ${(p) => p.theme.color.neutralBg};
  border: 1px solid ${(p) => p.theme.color.neutralBg};
  padding: ${(p) => p.theme.size.tiny};
  border-radius: ${(p) => p.theme.borderRadius.normal};

  :hover {
    background-color: ${(p) => p.theme.color.neutralBg};
    text-decoration: none;
  }
`;

const Title = styled.div`
  padding-right: ${(p) => p.theme.size.exTiny};
  margin-bottom: ${(p) => p.theme.size.exTiny};
  color: ${(p) => p.theme.color.secondaryText};
`;

const Description = styled.p`
  font-size: ${(p) => p.theme.fontSize.tiny};
  color: ${(p) => p.theme.fontSize.normal};
  margin-bottom: 0;
`;

const ClearButton = styled(ButtonTransparent)`
  position: absolute;
  top: 0;
  right: ${(p) => p.theme.size.exTiny};
  font-size: ${(p) => p.theme.fontSize.normal};
  padding: ${(p) => p.theme.size.exTiny};
`;

const Root = styled.div`
  position: relative;
  margin-top: ${(p) => p.theme.size.exTiny};

  &.error ${LinkTo} {
    background-color: ${(p) => p.theme.color.secondaryWarnBg};
    border-color: ${(p) => p.theme.color.primaryWarnBg};
  }

  &.error ${Title} {
    color: ${(p) => p.theme.color.primaryWarnText};
  }

  &.error ${ClearButton} {
    color: ${(p) => p.theme.color.primaryWarnBg};
  }
`;

export default (props) => {
  const { closeLatestPopup, notify } = props;
  useEffect(() => {
    const timeOut = setTimeout(() => {
      closeLatestPopup && closeLatestPopup(notify.id);
      clearTimeout();
    }, 9000);

    return () => {
      clearTimeout(timeOut);
    };
  });

  const className = notify.type === "error" ? "error" : "info";
  return (
    <Root className={className}>
      <ClearButton onClick={() => props.closePopup(notify)}>
        <FontAwesomeIcon icon="times" />
      </ClearButton>
      <LinkTo href={notify.url} type={notify.type}>
        <Title>{notify.title}</Title>
        <Description>{notify.message}</Description>
      </LinkTo>
    </Root>
  );
};
