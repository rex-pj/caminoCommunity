import React from "react";
import styled from "styled-components";
import { Image } from "../../atoms/Images";
import { AnchorLink } from "../../atoms/Links";

const CreatorAvatar = styled(AnchorLink)`
  margin-right: ${p => p.theme.size.distance};
  display: block;

  ${Image} {
    width: ${p => p.theme.size.normal};
    height: ${p => p.theme.size.normal};
    border-radius: ${p => p.theme.size.normal};
  }
`;

const CreatorDetail = styled.div`
  line-height: 1;
`;

const CreatorName = styled(AnchorLink)`
  font-weight: 700;
  font-size: ${p => p.theme.fontSize.small};
  display: inline-block;
  margin-bottom: 5px;
  margin-top: 1px;
`;

const MoreInfo = styled.p`
  font-size: ${p => p.theme.fontSize.tiny};
  color: ${p => p.theme.color.normal};
  font-weight: 400;
  margin-bottom: 0;
`;

export default function(props) {
  const { profile, className } = props;
  return (
    <div className={`${className} row no-gutters`}>
      <div className="col col-auto">
        <CreatorAvatar to={profile.profileUrl}>
          <Image src={profile.photoUrl} alt="" />
        </CreatorAvatar>
      </div>
      <div className="col col-auto">
        <CreatorDetail>
          <CreatorName to={profile.profileUrl}>{profile.name}</CreatorName>
          {profile.info && <MoreInfo>{profile.info}</MoreInfo>}
        </CreatorDetail>
      </div>
    </div>
  );
};
