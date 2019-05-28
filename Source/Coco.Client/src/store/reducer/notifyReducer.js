import * as actionTypes from "../actions";
const initialState = {
  notify: null
};

const reducer = (state = initialState, action) => {
  if (action.type === actionTypes.NOTIFICATION) {
    return {
      ...state,
      notify: action.payload
    };
  } else if (action.type === actionTypes.UNNOTIFICATION) {
    return state;
  }

  return state;
};

export default reducer;
