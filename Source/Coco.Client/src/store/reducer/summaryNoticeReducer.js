const initialState = {
  number: 0
};

const reducer = (state = initialState, action) => {
  if (action.type === "ADDCART") {
    return {
      ...state,
      number: state.number + 1
    };
  } else if (action.type === "REMOVECART") {
    return {
      ...state,
      number: state.number - 1
    };
  }

  return state;
};

export default reducer;
