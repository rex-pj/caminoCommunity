import { initStore } from "./index";

const configureStore = () => {
  const actions = {
    OPEN_MODAL: (curState, payload) => {
      return { ...payload };
    },
    CLOSE_MODAL: (curState, payload) => {
      return {
        options: {
          isOpen: false
        }
      };
    }
  };
  initStore(actions, {
    data: {
      imageUrl: null,
      title: null,
      canEdit: false
    },
    options: {
      isOpen: false,
      unableClose: false
    }
  });
};

export default configureStore;
