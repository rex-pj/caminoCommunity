const initialState = {
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
  }

  return state;
};

export default reducer;
