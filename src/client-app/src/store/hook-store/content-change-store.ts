import { initStore } from "./index";

const configureStore = () => {
  const actions = {
    ARTICLE_UPDATE: (curState: any, payload: any) => {
      return { id: payload.id, type: "ARTICLE_UPDATE", store: "UPDATE" };
    },
    FARM_UPDATE: (curState: any, payload: any) => {
      return { id: payload.id, type: "FARM_UPDATE", store: "UPDATE" };
    },
    PRODUCT_UPDATE: (curState: any, payload: any) => {
      return { id: payload.id, type: "PRODUCT_UPDATE", store: "UPDATE" };
    },
    ARTICLE_DELETE: (curState: any, payload: any) => {
      return { id: payload.id, type: "ARTICLE_DELETE", store: "UPDATE" };
    },
    FARM_DELETE: (curState: any, payload: any) => {
      return { id: payload.id, type: "FARM_DELETE", store: "UPDATE" };
    },
    PRODUCT_DELETE: (curState: any, payload: any) => {
      return { id: payload.id, type: "PRODUCT_DELETE", store: "UPDATE" };
    },
  };
  initStore(actions, {
    notifications: [],
  });
};

export default configureStore;
