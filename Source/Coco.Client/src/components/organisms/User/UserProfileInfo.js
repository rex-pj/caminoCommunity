import React, { Component } from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { VerticalList } from "../../atoms/List";

const Root = styled.div`
  position: relative;
  border-radius: ${p => p.theme.borderRadius.normal};
`;

const InfoList = styled(VerticalList)`
  margin-bottom: ${p => p.theme.size.distance};
`;

const ChildItem = styled.li`
  font-size: ${p => p.theme.fontSize.small};
  color: ${p => p.theme.color.dark};
  margin-bottom: ${p => p.theme.size.exSmall};

  span {
    color: inherit;
  }

  svg {
    margin-right: ${p => p.theme.size.exTiny};
  }

  svg,
  path {
    color: ${p => p.theme.color.normal};
  }

  a {
    font-size: ${p => p.theme.fontSize.exSmall};
    font-weight: 600;
  }
`;

export default class extends Component {
  componentDidMount() {
    this.props.loadUserInfo();
  }

  render() {
    const { userInfo } = this.props;
    return (
      <Root>
        {userInfo ? (
          <InfoList>
            <ChildItem className="text-justify">
              <span>{userInfo.blast}</span>
            </ChildItem>
            <ChildItem>
              <FontAwesomeIcon icon="map-marked-alt" />
              <span>{userInfo.address}</span>
            </ChildItem>
            <ChildItem>
              <FontAwesomeIcon icon="map-marker-alt" />
              <span>{userInfo.country}</span>
            </ChildItem>
            <ChildItem>
              <FontAwesomeIcon icon="calendar-alt" />
              <span>{userInfo.joinedDate}</span>
            </ChildItem>
            <ChildItem>
              <FontAwesomeIcon icon="envelope" />
              <a href={userInfo.email}>{userInfo.email}</a>
            </ChildItem>
            <ChildItem>
              <FontAwesomeIcon icon="mobile-alt" />
              <span>{userInfo.mobile}</span>
            </ChildItem>
          </InfoList>
        ) : null}
      </Root>
    );
  }
}
