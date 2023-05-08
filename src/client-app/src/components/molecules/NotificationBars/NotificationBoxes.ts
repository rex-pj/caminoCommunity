import styled from "styled-components";

const NotificationBox = styled.div`
  text-align: center;
  padding: ${(p) => p.theme.size.tiny};
  border-radius: ${(p) => p.theme.borderRadius.normal};
  width: 100%;
  svg,
  path,
  span {
    vertical-align: middle;
    color: inherit;
  }
`;

export const SuccessBox = styled(NotificationBox)`
  color: ${(p) => p.theme.color.secondaryText};
  background-color: ${(p) => p.theme.color.neutralBorder};
  border: 1px solid ${(p) => p.theme.color.neutralBorder};
`;

export const ErrorBox = styled(NotificationBox)`
  color: ${(p) => p.theme.color.dangerText};
  background-color: ${(p) => p.theme.color.warnBg};
  border: 1px solid ${(p) => p.theme.color.warnBg};
`;

export const LoadingBox = styled(NotificationBox)`
  color: ${(p) => p.theme.color.secondaryText};
  background-color: ${(p) => p.theme.color.neutralBg};
  border: 1px solid ${(p) => p.theme.color.neutralBg};
`;

export const NoDataBox = styled(NotificationBox)`
  color: ${(p) => p.theme.color.secondaryText};
  background-color: ${(p) => p.theme.color.neutralBg};
  border: 1px solid ${(p) => p.theme.color.neutralBg};
`;
