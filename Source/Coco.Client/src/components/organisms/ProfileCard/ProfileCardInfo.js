import React, { Component } from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { VerticalList } from "../../atoms/List";
import { PanelBody } from "../../atoms/Panels";
import { LabelDark } from "../../atoms/Labels";
import { AnchorLink } from "../../atoms/Links";

const Root = styled.div`
  position: relative;
  border-radius: ${p => p.theme.borderRadius.normal};
`;

const Label = styled(LabelDark)`
  font-weight: 600;
  font-size: ${p => p.theme.fontSize.small};
`;

const InfoList = styled(VerticalList)`
  margin-bottom: ${p => p.theme.size.distance};
`;

const ParentItem = styled.div``;

const ChildItem = styled.li`
  font-size: ${p => p.theme.fontSize.small};
  color: ${p => p.theme.color.dark};
  margin-bottom: ${p => p.theme.size.exTiny};

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
  render() {
    const { profileInfos } = this.props;
    return (
      <Root>
        <PanelBody>
          {profileInfos
            ? profileInfos.map((item, index) => {
                const { infos } = item;
                return (
                  <ParentItem key={index}>
                    <Label>{item.name}</Label>
                    {infos ? (
                      <InfoList>
                        {infos.map((info, index) => {
                          return (
                            <ChildItem key={index} isLink={!!info.url}>
                              <FontAwesomeIcon icon={info.icon} />
                              {info.url ? (
                                <AnchorLink to={info.url} as={info.url}>
                                  {info.name}
                                </AnchorLink>
                              ) : (
                                <span>{info.name}</span>
                              )}
                            </ChildItem>
                          );
                        })}
                      </InfoList>
                    ) : null}
                  </ParentItem>
                );
              })
            : null}
        </PanelBody>
      </Root>
    );
  }
}
