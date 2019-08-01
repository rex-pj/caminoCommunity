import * as notifyActions from "../actions/notifyActions";
const initialState = {
  notify: null
};

const reducer = (state = initialState, action) => {
  if (action.type === notifyActions.NOTIFICATION) {
    return {
      ...state,
      notify: action.payload
    };
  } else if (action.type === notifyActions.UNNOTIFICATION) {
    return state;
  }

  return state;
};

export default reducer;
