import { initStore } from "./index";

const configureStore = () => {
  const actions = {
    AVATAR_UPDATED: (curState, payload) => {
      return { type: "AVATAR_UPDATED", payload };
    },
    AVATAR_DELETED: (curState, payload) => {
      return { type: "AVATAR_DELETED" };
    }
  };
  initStore(actions, {
    data: {}
  });
};

export default configureStore;
