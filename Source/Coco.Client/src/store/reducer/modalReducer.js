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
      options: action.payload
    };
  } else if (action.type === "CLOSE") {
    return {
      ...state,
      options: action.payload
    };
  } else if (action.type === "PUSH_DATA") {
    return {
      ...state,
      payload: action.payload
    };
  }

  return state;
};

export default reducer;
