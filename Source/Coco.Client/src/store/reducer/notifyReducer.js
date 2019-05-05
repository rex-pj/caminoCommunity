const initialState = {
  notify: null
};

const reducer = (state = initialState, action) => {
  if (action.type === "NOTIFICATION") {
    return {
      ...state,
      notify: action.payload
    };
  } else if (action.type === "UNNOTIFICATION") {
    return state;
  }

  return state;
};

export default reducer;
