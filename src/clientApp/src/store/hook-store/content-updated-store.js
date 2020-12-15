import { initStore } from "./index";

const configureStore = () => {
  const actions = {
    ARTICLE_UPDATE: (curState, payload) => {
      return { id: payload.id, type: "ARTICLE" };
    },
  };
  initStore(actions, {
    notifications: [],
  });
};

export default configureStore;
