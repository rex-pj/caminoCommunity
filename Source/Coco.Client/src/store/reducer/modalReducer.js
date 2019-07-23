const initialState = {
  payload: null,
  options: {
    isOpen: false,
    children: null,
    title: null
  }
};

const reducer = (state = initialState, action) => {
  if (action.type === "OPEN") {
    return {
      ...state,
      payload: action.payload
    };
  } else if (action.type === "CLOSE") {
    return {
      ...state,
      payload: action.payload
    };
  } else if (action.type === "PUSH_DATA") {
    const payload = {
      ...action.payload,
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
