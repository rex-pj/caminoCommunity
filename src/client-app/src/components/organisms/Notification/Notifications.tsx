import * as React from "react";
import { useEffect } from "react";
import styled from "styled-components";
import NotifyItem from "./NotifyItem";
import { useStore } from "../../../store/hook-store";

const Root = styled.div`
  position: fixed;
  right: ${(p) => p.theme.size.distance};
  bottom: ${(p) => p.theme.size.distance};
  z-index: 100;
`;

type Props = {};

const Notifications = (props: Props) => {
  const [state, dispatch] = useStore(true);

  const closePopup = (notify: any) => {
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
        ? notifications.map((item: any, index: number) => {
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

export default Notifications;
