import { initStore } from "./index";

const configureStore = () => {
  const actions = {
    NOTIFY: (curState, payload) => {
      curState.notifications.push(payload);
      const updatedNotifications = [...curState.notifications];
      return { notifications: updatedNotifications };
    },
    UNNOTIFY: (curState, payload) => {
      let updatedNotifications = [...curState.notifications];

      if (payload) {
        updatedNotifications = updatedNotifications.filter(
          item => item !== payload
        );
      } else {
        updatedNotifications.splice(0, 1);
      }

      return { notifications: updatedNotifications };
    }
  };
  initStore(actions, {
    notifications: []
  });
};

export default configureStore;
