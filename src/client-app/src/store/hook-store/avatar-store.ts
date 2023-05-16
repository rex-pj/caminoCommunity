import { initStore } from "./index";

const configureStore = () => {
  const actions = {
    AVATAR_UPDATED: (curState: any, payload: any) => {
      return { type: "AVATAR_UPDATED", payload };
    },
    AVATAR_DELETED: (curState: any, payload: any) => {
      return { type: "AVATAR_DELETED" };
    },
  };
  initStore(actions, {
    data: {},
  });
};

export default configureStore;
