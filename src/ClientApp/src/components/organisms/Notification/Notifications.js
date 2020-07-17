import React, { useEffect } from "react";
import styled from "styled-components";
import NotifyItem from "./NotifyItem";
import { useStore } from "../../../store/hook-store";

const Root = styled.div`
  position: fixed;
  right: ${p => p.theme.size.distance};
  bottom: ${p => p.theme.size.distance};
  z-index: 100;
`;

export default () => {
  const [state, dispatch] = useStore(true);

  const closePopup = notify => {
    dispatch("UNNOTIFY", notify);
  };

  const closeLatestPopup = () => {
    dispatch("UNNOTIFY");
  };

  useEffect(() => {}, []);

  const { notifications } = state;
  return (
    <Root>
      {notifications
        ? notifications.map((item, index) => {
            return (
              <NotifyItem
                key={index}
                closePopup={closePopup}
                closeLatestPopup={closeLatestPopup}
                notify={item}
              />
            );
          })
        : null}
    </Root>
  );
};
