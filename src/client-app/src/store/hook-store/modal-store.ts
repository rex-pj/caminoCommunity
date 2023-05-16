import { initStore } from "./index";

const configureStore = () => {
  const actions = {
    OPEN_MODAL: (curState: any, payload: any) => {
      return { ...payload };
    },
    CLOSE_MODAL: (curState: any, payload: any) => {
      return {
        options: {
          isOpen: false,
        },
      };
    },
  };
  initStore(actions, {
    data: {
      imageUrl: null,
      title: null,
      canEdit: false,
    },
    options: {
      isOpen: false,
      unableClose: false,
    },
  });
};

export default configureStore;
