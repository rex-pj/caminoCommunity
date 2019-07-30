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

function openModal(dispatch, e) {
  dispatch({
    type: modalActions.OPEN,
    payload: {
      ...e,
      isOpen: true
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

function modalUploadAvatar(dispatch, data) {
  dispatch({
    type: modalActions.AVATAR_UPLOADED,
    payload: {
      ...data
    }
  });
}

function modalDeleteAvatar(dispatch) {
  dispatch({
    type: modalActions.AVATAR_DELETED
  });
}

export {
  raiseError,
  openModal,
  closeModal,
  modalUploadAvatar,
  modalDeleteAvatar
};
