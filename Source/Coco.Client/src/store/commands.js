import * as notifyActions from "./notifyActions";
import * as modalActions from "./modalActions";

function raiseError(dispatch, title, description, url) {
  dispatch({
    type: notifyActions.NOTIFICATION,
    payload: {
      type: "error",
      title: title,
      description: description,
      url: url
    }
  });
}

function openModal(dispatch, data, title, modalType) {
  dispatch({
    type: modalActions.OPEN,
    payload: {
      title: title,
      isOpen: true,
      data,
      modalType: modalType
    }
  });
}

function closeModal(dispatch) {
  dispatch({
    type: modalActions.OPEN,
    payload: {
      isOpen: false
    }
  });
}

function modalPushData(dispatch, data) {
  dispatch({
    type: modalActions.PUSH_DATA,
    payload: {
      ...data
    }
  });
}

export { raiseError, openModal, closeModal, modalPushData };
