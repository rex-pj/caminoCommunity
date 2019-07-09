import React from "react";
import styled from "styled-components";
import ImageUpload from "../../molecules/UploadControl/ImageUpload";
import { ImageCircle } from "../../atoms/Images";

const ProfileImage = styled(ImageCircle)`
  display: block;
`;

const Wrap = styled.div`
  display: block;
`;

const AvatarUpload = styled(ImageUpload)`
  position: absolute;
  top: 0;
  right: 0;
  z-index: 2;
  text-align: center;
  margin: auto;

  span {
    color: ${p => p.theme.color.light};
    width: ${p => p.theme.size.medium};
    height: ${p => p.theme.size.medium};
    border-radius: 100%;
    padding: 0 ${p => p.theme.size.exTiny};
    background-color: ${p => p.theme.rgbaColor.exDark};
    border: 1px solid ${p => p.theme.rgbaColor.dark};
    cursor: pointer;
    font-weight: 600;

    svg {
      display: block;
      margin: 10px auto 0 auto;
    }
  }
`;

const AvatarLink = styled.a`
  display: block;
  width: 110px;
  height: 110px;
  border: 5px solid ${p => p.theme.rgbaColor.cyanMoreLight};
  background-color: ${p => p.theme.rgbaColor.cyanMoreLight};
  border-radius: 100%;
`;

export default function(props) {
  const { userInfo, canEdit } = props;

  return (
    <Wrap className={props.className}>
      <AvatarLink href={userInfo.url}>
        <ProfileImage src={userInfo.avatarUrl} />
      </AvatarLink>
      {!!canEdit ? <AvatarUpload /> : null}
    </Wrap>
  );
}
