import * as modalActions from "../actions/modalActions";

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
  }

  return state;
};

export default reducer;
