import * as modalActions from "../modalActions";

const initialState = {
  payload: null,
  options: {
    isOpen: false,
    children: null,
    title: null
  }
};

const reducer = (state = initialState, action) => {
  if (action.type === modalActions.OPEN) {
    return {
      ...state,
      payload: action.payload
    };
  } else if (action.type === modalActions.CLOSE) {
    return {
      ...state,
      payload: action.payload
    };
  } else if (action.type === modalActions.UPLOAD_AVTARA) {
    const payload = {
      ...action.payload,
      actionType: action.type
    };
    return {
      ...state,
      payload: payload
    };
  } else if (action.type === modalActions.DELETE_AVTARA) {
    const payload = {
      actionType: action.type
    };
    return {
      ...state,
      payload: payload
    };
  }

  return state;
};

export default reducer;
