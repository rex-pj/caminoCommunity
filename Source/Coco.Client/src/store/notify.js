import * as actionTypes from "./actions";

function raiseError(dispatch, title, description, url) {
  dispatch({
    type: actionTypes.NOTIFICATION,
    payload: {
      type: "error",
      title: title,
      description: description,
      url: url
    }
  });
}

export { raiseError };
