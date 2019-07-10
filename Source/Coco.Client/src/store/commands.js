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

function openModal(dispatch, children, title, modalType) {
  dispatch({
    type: modalActions.OPEN,
    payload: {
      title: title,
      children: children,
      isOpen: true,
      modalType: modalType
    }
  });
}

function closeModal(dispatch, isOpen, children, title) {
  dispatch({
    type: modalActions.OPEN,
    payload: {
      title: title,
      children: children,
      isOpen: false
    }
  });
}

export { raiseError, openModal, closeModal };
