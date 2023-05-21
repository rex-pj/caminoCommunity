import * as React from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { VerticalList } from "../../molecules/List";
import { format } from "date-fns";
import { IconProp } from "@fortawesome/fontawesome-svg-core";

const Root = styled.div`
  position: relative;
  border-radius: ${(p) => p.theme.borderRadius.normal};
`;

const InfoList = styled(VerticalList)`
  margin-bottom: ${(p) => p.theme.size.distance};
`;

const ChildItem = styled.li`
  font-size: ${(p) => p.theme.fontSize.small};
  color: ${(p) => p.theme.color.darkText};
  margin-bottom: ${(p) => p.theme.size.exSmall};

  span {
    color: inherit;
    word-break: break-all;
  }

  div {
    color: inherit;
    display: block;
    max-width: 100%;
    word-break: break-all;
  }

  svg {
    margin-right: ${(p) => p.theme.size.exTiny};
  }

  svg,
  path {
    color: ${(p) => p.theme.color.darkText};
  }

  a {
    font-size: ${(p) => p.theme.fontSize.small};
    font-weight: 600;
  }
`;

interface UnserInfoChildProps {
  className?: string;
  children?: any;
  icon?: IconProp;
  isEmail?: boolean;
}

const UnserInfoChild = (props: UnserInfoChildProps) => {
  const { className, children, icon, isEmail } = props;
  return children ? (
    <ChildItem className={className}>
      {icon ? <FontAwesomeIcon icon={icon} /> : null}
      {isEmail ? (
        <a href={`mailto:${children}`}>{children}</a>
      ) : (
        <span>{children}</span>
      )}
    </ChildItem>
  ) : null;
};

type Props = {
  userInfo?: any;
};

const ProfileInfo = (props: Props) => {
  const { userInfo } = props;

  return (
    <Root>
      {userInfo ? (
        <InfoList className="row">
          <UnserInfoChild className="text-justify">
            {userInfo.description}
          </UnserInfoChild>
          <UnserInfoChild
            className="col col-6 col-md-4 col-lg-12"
            icon="map-marked-alt"
          >
            {userInfo.address}
          </UnserInfoChild>
          <UnserInfoChild
            className="col col-6 col-md-4 col-lg-12"
            icon="map-marker-alt"
          >
            {userInfo.country}
          </UnserInfoChild>
          <UnserInfoChild className="col col-6 col-md-4 col-lg-12" icon="baby">
            {userInfo.birthDate
              ? format(new Date(userInfo.birthDate), "MMMM, dd yyyy")
              : null}
          </UnserInfoChild>
          <UnserInfoChild
            className="col col-6 col-md-4 col-lg-12"
            icon="calendar-alt"
          >
            {userInfo.createdDate
              ? format(new Date(userInfo.createdDate), "MMMM, dd yyyy")
              : null}
          </UnserInfoChild>
        </InfoList>
      ) : null}
    </Root>
  );
};

export default ProfileInfo;
