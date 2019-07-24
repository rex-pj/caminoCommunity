import * as avatarActions from "../actions/avatarActions";

const initialState = {
  payload: null,
  options: {
    isOpen: false,
    children: null,
    title: null
  }
};

const reducer = (state = initialState, action) => {
  if (action.type === avatarActions.AVATAR_UPLOADED) {
    const payload = {
      ...action.payload,
      actionType: action.type
    };
    return {
      ...state,
      payload: payload
    };
  } else if (action.type === avatarActions.AVATAR_DELETED) {
    const payload = {
      actionType: action.type
    };
    return {
      ...state,
      payload: payload
    };
  } else if (action.type === avatarActions.AVATAR_RELOAD) {
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
